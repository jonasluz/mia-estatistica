using System.Collections.Generic;
using UnityEngine;

class ConsoleMonitor : IStepListener
{

    public void NotifyBorn(IEnumerable<int> born)
    {
        string msg = "New born: ";
        foreach (int i in born)
            msg += IdxToPos(i) + " ";
        Debug.Log(msg);
    }

    public void NotifyDead(IEnumerable<int> dead, bool byAccident)
    {
        string msg = "Dead " + ( byAccident ? "by accident" : "of age" ) + ": ";
        foreach (int i in dead)
            msg += IdxToPos(i) + " ";
        Debug.Log(msg);
    }

    public void NotifyInfection(IEnumerable<int> infected)
    {
        string msg = "Infected: ";
        foreach (int i in infected)
            msg += IdxToPos(i) + " ";
        Debug.Log(msg);
    }

    public void NotifyMove(int from, int to)
    {
        int n = Board.B.N;
        string msg =
            string.Format(
                "({0})->({1}) = [{2}, {3}]->[{4}, {5}]",
                from, to, from / n, from % n, to / n, to % n
                );
        Debug.Log(msg);
    }

    public void NotifyStep(CycleStep step)
    {
        string board = "";
        int n = Board.B.N;
        for (int i = 0; i < n; ++i)
        {
            for (int j = 0; j < n; ++j)
            {
                int idx = i * n + j;
                Being b = Board.B.Get(idx);
                char mark = ' ';
                if (b != null)
                    switch (b.Health)
                    {
                        case Being.HealthStatus.HEALTHY:
                            mark = 'H';
                            break;
                        case Being.HealthStatus.IMUNE:
                            mark = 'I';
                            break;
                        case Being.HealthStatus.INFECTED:
                            mark = 'X';
                            break;
                        case Being.HealthStatus.PSEUDOIMUNE:
                            mark = 'P';
                            break;
                    }
                board += mark;
            }
            board += '\n';
        }
        string msg = string.Format(
            "<color=blue>Step {0} =></color> H:{1} I:{2} P:{3} X:{4} [i:{5} d:{6} c:{7} b:{8}]",
            step.num, step.healthies, step.imunes, step.pseudoimunes, step.infected,
            step.infections, step.agedeads, step.casualties, step.newborns
            );
        Debug.Log(board + "\n" + msg);
    }

    private string IdxToPos(int idx)
    {
        int n = Board.B.N;
        return "[" + (idx / n) + "," + (idx % n) + "]";
    }
}
