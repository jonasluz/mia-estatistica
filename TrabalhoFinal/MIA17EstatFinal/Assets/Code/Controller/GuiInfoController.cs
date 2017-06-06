using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GuiConfigController))]
public class GuiInfoController : MonoBehaviour, IStepListener {

    [Header("GUI Elements")]
    public Text textInfectionChances;
    public Text textAccidentChances;
    public Text textBirthChances;
    public Text textFrequency;

    public Text textStepNum;
    public Text textTotalHealthy;
    public Text textTotalImunes;
    public Text textTotalPseudo;
    public Text textTotalInfection;
    public Text textTotalGraves;

    public Text textStepInfection;
    public Text textStepDeadbyAge;
    public Text textStepCasualties;
    public Text textStepBirths;

    GuiConfigController config; 

    // Use this for initialization
    void Awake()
    {
        config = GetComponent<GuiConfigController>();
        Board.B.AddListener(this);
    }

    public void NotifyBorn(IEnumerable<int> born)
    {
        //throw new NotImplementedException();
    }

    public void NotifyDead(IEnumerable<int> dead, bool byAccident)
    {
        //throw new NotImplementedException();
    }

    public void NotifyInfection(IEnumerable<int> infected)
    {
        //throw new NotImplementedException();
    }

    public void NotifyMove(int from, int to)
    {
        //throw new NotImplementedException();
    }

    public void NotifyStep(CycleStep step)
    {
        UpdateInfo(step);
    }

    void UpdateInfo(CycleStep step)
    {
        textStepNum.text = "Passo: " + step.num;
        textTotalHealthy.text = step.healthies + " saudáveis.";
        textTotalImunes.text = step.imunes + " imunes.";
        textTotalInfection.text = step.infected + " infectados.";
        textTotalPseudo.text = step.pseudoimunes + " pseudoimunes.";
        textTotalGraves.text = step.dead + " mortos.";
        textStepInfection.text = step.infections + " novos infectados.";
        textStepDeadbyAge.text = step.agedeads + " mortos por idade.";
        textStepCasualties.text = step.casualties + " mortos por acidente.";
        textStepBirths.text = step.newborns + " novos nascidos.";

        textInfectionChances.text = string.Format("Chances de infecção: {0}%", config.sliderInfectionChance.slider.value);
        textAccidentChances.text = string.Format("Chances de acidentes: {0}%", config.sliderAccidentChance.slider.value);
        textBirthChances.text = string.Format("Chances de nascimentos: {0}%", config.sliderBirthChance.slider.value);
        //textFrequency.text = string.Format("Frequência: {0} Hz", config.sliderFrequency.slider.value);
    }
}
