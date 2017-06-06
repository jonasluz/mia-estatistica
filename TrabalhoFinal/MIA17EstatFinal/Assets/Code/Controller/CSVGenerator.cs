using System.Runtime.InteropServices;
using UnityEngine;

public class CSVGenerator : MonoBehaviour {

    [DllImport("__Internal")]
    private static extern void Hello();

    [DllImport("__Internal")]
    private static extern void HelloString(string str);

    [DllImport("__Internal")]
    private static extern void ExportCSV(string csv, int dl);

    public void GenerateCSV(bool download=false)
    {
        string content = 
            "ciclo,saudáveis,imunes,pseudoimunes,infectados,total de mortos," +
            "infecções,mortes por idade,acidentados,nascimentos\n";

        foreach (CycleStep step in Board.B.StepLog)
            content += step.ToCSV() + "\n";

        ExportCSV(content, download ? 1 : 0);
    }
}
