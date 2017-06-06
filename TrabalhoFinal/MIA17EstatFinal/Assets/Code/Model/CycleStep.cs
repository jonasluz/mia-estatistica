public struct CycleStep
{
    public int num;
    public int imunes;
    public int healthies;
    public int pseudoimunes;
    public int infected;
    public int dead;
    public int infections;
    public int agedeads;
    public int casualties;
    public int newborns;

    public CycleStep(int num, int healthies, int imunes, int pseudoimunes, int infected, int dead,
                     int infections, int agedeads, int casualties, int newborns)
    {
        this.num = num;
        this.healthies = healthies; this.imunes = imunes;
        this.pseudoimunes = pseudoimunes; this.infected = infected;
        this.dead = dead;
        this.infections = infections; this.casualties = casualties;
        this.agedeads = agedeads; this.newborns = newborns;
    }

    public override string ToString()
    {
        return string.Format(
                "{0} :: H:{1} | I:{2} | P:{3} | X:{4} | D: {5} [ i:{6} | d:{7} | c:{8} | b:{9} ]",
                num, healthies, imunes, pseudoimunes, infected, dead, 
                infections, agedeads, casualties, newborns
            );
    }

    public string ToCSV()
    {
        return string.Format(
                "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                num, healthies, imunes, pseudoimunes, infected, dead, 
                infections, agedeads, casualties, newborns
            );
    }
}
