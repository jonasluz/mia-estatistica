using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board
{
    #region Singleton
    private static Board instance = null;
    public static Board B
    {
        get
        {
            if (instance == null) instance = new Board();
            return instance;
        }
    }
    protected Board() { ; }
    #endregion Singleton

    /// <summary>
    /// N define o tamanho da matriz de seres.
    /// </summary>
    public int N
    {
        get; set;
    }

    /// <summary>
    /// Retorna o Been guardado no índice indicado.
    /// </summary>
    /// <param name="idx">índice</param>
    /// <returns>Been gurdado no índice dado.</returns>
    public Been Get(int idx)
    {
        return data[idx];
    }

    /// <summary>
    /// Infecção foi extinta? 
    /// </summary>
    public bool InfectionExtincted
    {
        get
        {
            return indexes[Been.HealthStatus.INFECTED].Count == 0;
        }
    }

    public CycleStep[] StepLog
    {
        get
        {
            return steps.ToArray();
        }
    }

    /// <summary>
    /// Dados da rede de seres.
    /// </summary>
    private Been[] data;
    /// <summary>
    /// Índices dos tipos na rede, para acesso randômico.
    /// </summary>
    private Dictionary<Been.HealthStatus, List<int>> indexes;
    /// <summary>
    /// Lugares disponíveis na rede antes e durante a inicialização desta.
    /// </summary>
    private List<int> canChoose;
    /// <summary>
    /// Sequência de passos ocorridos.
    /// </summary>
    private List<CycleStep> steps;

    // Totais.
    private int maximum;

    #region Listeners 
    private List<IStepListener> listeners = new List<IStepListener>();

    public void AddListener(IStepListener listener)
    {
        listeners.Add(listener);
    }
    #endregion Listeners

    #region INICIALIZAÇÃO DA REDE
    /// <summary>
    /// Inicializa a rede de seres.
    /// </summary>
    public void Init(int n)
    {
        // Inicializa rede.
        N = n;
        maximum = N * N;
        canChoose = new List<int>(maximum);
        data = new Been[maximum];
        for (int i = 0; i < maximum; ++i)
        {
            data[i] = new Been();
            canChoose.Add(i);
        }
        indexes =
        new Dictionary<Been.HealthStatus, List<int>>() {
            {  Been.HealthStatus.DEAD, new List<int>() },
            {  Been.HealthStatus.IMUNE, new List<int>() },
            {  Been.HealthStatus.HEALTHY, new List<int>() },
            {  Been.HealthStatus.PSEUDOIMUNE, new List<int>() },
            {  Been.HealthStatus.INFECTED, new List<int>() }
        };

        // Seleciona e gera o infectado. 
        int pos = RandomPosition();
        data[pos].Health = Been.HealthStatus.INFECTED;
        indexes[Been.HealthStatus.INFECTED].Add(pos);

        // Gera os seres imunes e pseudoimunes com iguais chances de predominar com os saudáveis.
        List<Been.HealthStatus> missingStates = new List<Been.HealthStatus>() {
            Been.HealthStatus.IMUNE,
            Been.HealthStatus.PSEUDOIMUNE,
            Been.HealthStatus.HEALTHY
        };
        int choosen = Random.Range(0, 3);
        Distribute(missingStates[choosen]);
        missingStates.RemoveAt(choosen);
        choosen = Random.Range(0, 2);
        Distribute(missingStates[choosen]);
        missingStates.RemoveAt(choosen);
        Distribute(missingStates[0], true);
        canChoose = null;

        // Guarda passo 0.
        steps = new List<CycleStep>(1);
        saveStep(0, 0, 0, 0);
    }

    /// <summary>
    /// Distribui seres com a saúde especificada pela rede.
    /// </summary>
    /// <param name="health">estado de saúde dos seres.</param>
    /// <param name="remaining">distribuir por todos os disponíveis remanescentes?</param>
    private IEnumerable<int> Distribute(Been.HealthStatus health, bool remaining = false)
    {
        List<int> distributed = new List<int>();
        int qtd = remaining ? canChoose.Count : UnityEngine.Random.Range(1, canChoose.Count);
        for (int i = 0; i < qtd; ++i)
        {
            int idx = RandomPosition();
            data[idx].Health = health;
            distributed.Add(idx);
        }
        indexes[health].AddRange(distributed);
        return distributed;
    }

    /// <summary>
    /// Escolhe uma posição aleatória na rede, dentre as disponíveis, retirando-a de disponibilidade.
    /// </summary>
    /// <returns>índice escolhido da posição na rede.</returns>
    private int RandomPosition()
    {
        int lim = Mathf.Min(canChoose.Count, maximum / 2);
        int r = UnityEngine.Random.Range(0, lim);
        int pos = canChoose[r];
        canChoose.RemoveAt(r);
        return pos;
    }

    /// <summary>
    /// Create and save a new step. 
    /// </summary>
    /// <param name="newInfected">number of new infected.</param>
    /// <param name="deadByAge">number of dead by age.</param>
    /// <param name="casualties">number of casualties.</param>
    /// <param name="births">number of births.</param>
    private void saveStep(int newInfected, int deadByAge, int casualties, int births)
    {
        int num = steps.Count + 1;
        steps.Add(new CycleStep(num,
                    indexes[Been.HealthStatus.HEALTHY].Count,
                    indexes[Been.HealthStatus.IMUNE].Count,
                    indexes[Been.HealthStatus.PSEUDOIMUNE].Count,
                    indexes[Been.HealthStatus.INFECTED].Count,
                    indexes[Been.HealthStatus.DEAD].Count,
                    newInfected, deadByAge, casualties, births)
                );

        // Notifica listeners.
        foreach (IStepListener listener in listeners)
            listener.NotifyStep(steps[num - 1]);
    }

    #endregion INICIALIZAÇÃO DA REDE

    #region CICLO DE VIDA DA REDE

    /// <summary>
    /// Ciclo de vida dos seres. 
    /// 1. Infecção.
    /// 2. Movimento dos infectados.
    /// 3. Mortandade.
    /// 4. Acidentes.
    /// 5. Renascimento.
    /// </summary>
    public void Cycle()
    {
        int infections = Infection();
        int agedeads = Deaths();
        int casualties = Deaths(true);
        int newborns = Reborn(); 

        // Registra passo.
        saveStep(infections, agedeads, casualties, newborns);
    }

    /// <summary>
    /// Transfere o índice da rede para a lista especificada.
    /// </summary>
    /// <param name="idx">índice a transferir.</param>
    /// <param name="state">região da rede.</param>
    private void MoveIndexTo(int idx, Been.HealthStatus state)
    {
        MoveIndexTo(new List<int>() { idx }, state);
    }
    /// <summary>
    /// Transfere a lista de índices da rede para a lista especificada.
    /// </summary>
    /// <param name="idxs">enumerável com os índices a transferir.</param>
    /// <param name="state">região da rede.</param>
    private void MoveIndexTo(IEnumerable<int> idxs, Been.HealthStatus state)
    {
        foreach (Been.HealthStatus stateIdx in indexes.Keys)
            indexes[stateIdx].RemoveAll(i => idxs.Contains(i));
        indexes[state].AddRange(idxs);
    }

    /// <summary>
    /// Fase da infecção.
    /// </summary>
    /// <returns>Quantidade de novos infectados.</returns>
    private int Infection()
    {
        // A lista de contaminados original não pode ser alterada enquanto estiver sendo varrida.
        List<int> contamination = new List<int>();
        // A lista de movimentos dos infectados.
        List<int[]> moves = new List<int[]>();

        // Marca infecções e movimentos.
        foreach (int pos in indexes[Been.HealthStatus.INFECTED])
        {
            // Infecção dos vizinhos.
            int[] neighbours = Neighbours(pos);
            foreach (int neighbour in neighbours)
                if (data[neighbour].Infect())
                    contamination.Add(neighbour);

            // Movimentação deste infectado.
            int moveTo = neighbours[UnityEngine.Random.Range(0, neighbours.Length)];
            moves.Add(new int[] { pos, moveTo });
        }

        // Atualiza a rede.
        MoveIndexTo(contamination, Been.HealthStatus.INFECTED);
        foreach (int[] move in moves)
        {
            // Troca seres de lugar.
            Been m = data[move[1]];
            data[move[1]] = data[move[0]];
            data[move[0]] = m;

            // Atualiza as listas.
            MoveIndexTo(move[0], m.Health);
            MoveIndexTo(move[1], data[move[1]].Health); // já está fisicamente no lugar.

            // Notifica listeners.
            foreach (IStepListener listener in listeners)
                listener.NotifyInfection(contamination);
            foreach (IStepListener listener in listeners)
                listener.NotifyMove(move[0], move[1]);
        }

        return contamination.Count;
    }

    /// <summary>
    /// A morte.
    /// </summary>
    /// <param name="byAccident">mortes por acidente</param>
    /// <returns>Lista de mortos</returns>
    private int Deaths(bool byAccident = false)
    {
        // Ativa morte natural.
        List<int> deads = new List<int>();
        for (int i = 0; i < maximum; ++i)
        {
            bool death = byAccident ? data[i].DieInAccident() : data[i].TimeToDie();
            if (death) deads.Add(i);
        }
        MoveIndexTo(deads, Been.HealthStatus.DEAD);

        // Notifica listeners.
        foreach (IStepListener listener in listeners)
            listener.NotifyDead(deads, byAccident);

        return deads.Count;
    }

    /// <summary>
    /// Nascimentos.
    /// </summary>
    /// <returns>Quantidade de nascidos.</returns>
    private int Reborn()
    {
        // Ativa renascimentos.
        List<int> reborn = new List<int>();
        foreach (int i in indexes[Been.HealthStatus.DEAD])
            if (data[i].Spawn())
                reborn.Add(i);
        foreach (int i in reborn)
            MoveIndexTo(i, data[i].Health);
        
        // Notifica listeners.
        foreach (IStepListener listener in listeners)
            listener.NotifyBorn(reborn);

        return reborn.Count;
    }

    /// <summary>
    /// Retorna os vizinhos válidos da posição indicada.
    /// </summary>
    /// <param name="x">posição central.</param>
    /// <returns>array de posições de vizinhos válidos.</returns>
    int[] Neighbours(int x)
    {
        List<int> result = new List<int>(2);            // pelo menos, dois vizinhos.
        int n = x - N;                                  // vizinho de cima.
        if (n > 0) result.Add(n);
        n = x + N;                                      // vizinho de baixo.
        if (n < maximum) result.Add(n);
        n = x - 1;                                      // vizinho da esquerda.
        if (n > 0 && x % N != 0) result.Add(n);
        n = x + 1;                                      // vizinho da direita.
        if (n < maximum && n % N != 0) result.Add(n);
        return result.ToArray();
    }

    #endregion CICLO DE VIDA DA REDE

}