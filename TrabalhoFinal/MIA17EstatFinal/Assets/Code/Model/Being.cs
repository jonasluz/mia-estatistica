using UnityEngine;

/// <summary>
/// Um ser, sujeito a viver, adoecer e morrer...
/// </summary>
public class Being {

    public enum HealthStatus
    {
        HEALTHY, IMUNE, PSEUDOIMUNE, INFECTED, DEAD
    }

    #region Constantes
    /// <summary>
    /// Idade padrão para quem é saudável ou imune.
    /// </summary>
    const int AGE_HEALTHY       = 10;
    /// <summary>
    /// Idade padrão para quem é pseudo-imune.
    /// </summary>
    const int AGE_PSEUDOIMUNE   = 4;
    /// <summary>
    /// Idade padrão para quem é infectado.
    /// </summary>
    const int AGE_INFECTED      = 3;    
    #endregion Constantes

    #region Variáveis de classe
    // Probabilidades... 
    public static float PseudoimuneChance
    {
        get; set;
    }
    public static float AccidentChance
    {
        get; set;
    }
    public static float SpawningChance
    {
        get; set;
    }
    #endregion Variáveis de classe

    #region Atributos
    /// <summary>
    /// O estado de saúde deste ser.
    /// </summary>
    public HealthStatus Health
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
            age =
                state == HealthStatus.INFECTED ? (age > AGE_INFECTED ? AGE_INFECTED : AGE_INFECTED) : (
                state == HealthStatus.PSEUDOIMUNE ? AGE_PSEUDOIMUNE : (
                state == HealthStatus.DEAD ? 0 : 
                AGE_HEALTHY
                ));
        }
    }
    /// <summary>
    /// Idade deste ser.
    /// </summary>
    private int age = AGE_HEALTHY;
    /// <summary>
    /// Estado de saúde deste ser.
    /// </summary>
    private HealthStatus state;
    #endregion Atributos

    /// <summary>
    /// Nascimento de um ser.
    /// </summary>
    /// <returns>Nasceu?.</returns>
    public bool Spawn()
    {
        if (this.state != HealthStatus.DEAD || Random.value > SpawningChance)
            return false;

        this.Health = (Random.value <= .5f) ? HealthStatus.IMUNE : HealthStatus.PSEUDOIMUNE;
        return true;
    }

    /// <summary>
    /// É chegada a hora? 
    /// </summary>
    /// <returns>Se morreu.</returns>
    public bool TimeToDie()
    {
        if (Health == HealthStatus.DEAD) return false;
        bool die = --age <= 0;
        if (die) this.Health = HealthStatus.DEAD;
        return die;
    }

    /// <summary>
    /// Acidentou-se? Sobreviveu? 
    /// </summary>
    /// <returns>Se morreu.</returns>
    public bool DieInAccident()
    {
        if (Health == HealthStatus.DEAD) return false;
        bool die = Random.value <= AccidentChance;
        if (die) this.Health = HealthStatus.DEAD;
        return die;
    }

    /// <summary>
    /// Tenta infectar este ser.
    /// </summary>
    /// <returns>Foi infectado?</returns>
    public bool Infect()
    {
        switch(state)
        {
            case HealthStatus.HEALTHY:
                Health = HealthStatus.INFECTED;
                return true;
            case HealthStatus.PSEUDOIMUNE:
                bool infected = Random.value <= PseudoimuneChance;
                if (infected) Health = HealthStatus.INFECTED;
                return infected;
            default:
                return false;
        }
    }
}
