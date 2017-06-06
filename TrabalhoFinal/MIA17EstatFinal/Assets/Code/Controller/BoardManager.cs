using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour, IStepListener {

    const int SPRITE_SIZE = 4;

    /// <summary>
    /// Play/Pause do ciclo de vida da rede.
    /// </summary>
    public bool Running
    {
        get; set;
    }

    /// <summary>
    /// Referência ao controle do simulador, que armazena os parâmetros.
    /// </summary>
    [Header("Connections")]
    public GuiConfigController config;
    public GameObject beenIcon;
    
    BeenView[] icons;
    private int maximum;

    private float timer;
    private bool stop = false;

    #region Monobehaviour functions
    private void Start()
    {
        maximum = config.n * config.n;
        int paddingx = (int)(config.n * 2.5);
        int paddingy = (int)(config.n * 2);
        icons = new BeenView[maximum];
        for (int i = 0; i < maximum; ++i)
        {
            int y = -i / config.n;
            int x = i % config.n;
            GameObject icon = 
                Instantiate(beenIcon, new Vector3(x * SPRITE_SIZE - paddingx, y * SPRITE_SIZE + paddingy, 0), 
                            Quaternion.identity, this.gameObject.transform);
            icons[i] = icon.GetComponent<BeenView>();
            if (config.debug) icons[i].DebugID = i;
        }
        Board.B.AddListener(this);
        Board.B.AddListener(new ConsoleMonitor());
        timer = Time.timeSinceLevelLoad;
        Board.B.Init(config.n);
    }

    private void Update()
    {
        if (stop || !Running) return;
        if (Time.timeSinceLevelLoad > timer)
        {
            timer = Time.timeSinceLevelLoad + +config.cycleDurationInSec;
            Board.B.Cycle();
            //UIRefresh();
        }
    }
    #endregion Monobehaviour functions

    private void UIRefresh()
    {
        for (int i=0; i < maximum; ++i)
        {
            Been b = Board.B.Get(i);
            if (b == null) icons[i].SetSprite();
            else icons[i].SetSprite(b.Health);
        }
    }

    #region IStepListener Functions

    public void NotifyStep(CycleStep step)
    {
        UIRefresh();
    }

    public void NotifyMove(int from, int to)
    {
        //icons[to].MoveTo(to / sim.n, to % sim.n);
    }

    public void NotifyDead(IEnumerable<int> dead, bool byAccident)
    {
        /*foreach (int i in dead)
        {
            icons[i].SetSprite(byAccident);
        }*/
    }

    public void NotifyBorn(IEnumerable<int> born)
    {
        /*foreach (int i in born)
        {
            icons[i].SetSprite();
        }*/
    }

    public void NotifyInfection(IEnumerable<int> infected)
    {
        /*foreach(int idx in infected)
            icons[idx].SetSprite(HealthStatus.INFECTED);
            */
    }

    #endregion IStepListener Functions
}
