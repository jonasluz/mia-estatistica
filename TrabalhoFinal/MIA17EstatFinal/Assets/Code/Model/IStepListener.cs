using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IStepListener
{
    void NotifyStep(CycleStep step);
    void NotifyMove(int from, int to);
    void NotifyDead(IEnumerable<int> dead, bool byAccident);
    void NotifyBorn(IEnumerable<int> born);
    void NotifyInfection(IEnumerable<int> infected);
}
