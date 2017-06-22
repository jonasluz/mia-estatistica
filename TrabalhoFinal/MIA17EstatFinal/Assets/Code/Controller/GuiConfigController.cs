using UnityEngine;
using UnityEngine.UI;

public class GuiConfigController : MonoBehaviour {

    [Header("Probabilities GUI Controls")]
    public PercentSlider sliderInfectionChance;
    public PercentSlider sliderBirthChance;
    public PercentSlider sliderAccidentChance;
    public FrequencySlider sliderFrequency;

    [Header("Absolute values")]
    [Range(4, 32)]
    public int n = 32;
    [HideInInspector]
    public float cycleDurationInSec = 1;

    [Header("Connections")]
    public Camera cam;
    public Canvas configCanvas;
    public Toggle runningToggle;

    [Header("Debug")]
    public bool debug = true;

    float previousInfectionChance, previousAccidentChance, previousBirthChance, previousFrequency;
    bool previousRunning;
    
    #region Monobehaviour functions
    private void Awake()
    {
        CloseConfigPanel();
        if (cam == null) cam = Camera.main;
        cam.orthographicSize = 2 * (n + 1);
        Propagate();
    }
    #endregion Monobehaviour functions

    public void OpenConfigPanel()
    {
        previousRunning = runningToggle.isOn;
        runningToggle.isOn = false;
        SavePrevious();
        configCanvas.gameObject.SetActive(true);
    }

    public void Apply()
    {
        CloseConfigPanel();
        Propagate();
    }

    public void Cancel()
    {
        ResetPrevious();
        CloseConfigPanel();
    }

    void CloseConfigPanel()
    {
        configCanvas.gameObject.SetActive(false);
        runningToggle.isOn = previousRunning;
    }

    void SavePrevious()
    {
        previousInfectionChance = sliderInfectionChance.slider.value;
        previousAccidentChance = sliderAccidentChance.slider.value;
        previousBirthChance = sliderBirthChance.slider.value;
        //previousFrequency = sliderFrequency.slider.value;
    }

    void ResetPrevious()
    {
        sliderInfectionChance.slider.value = previousInfectionChance;
        sliderAccidentChance.slider.value = previousAccidentChance;
        sliderBirthChance.slider.value = previousBirthChance;
        //sliderFrequency.slider.value = previousFrequency;
    }

    /// <summary>
    /// Propaga as configurações.
    /// </summary>
    void Propagate()
    { 
        Being.PseudoimuneChance = sliderInfectionChance.Value;
        Being.AccidentChance = sliderAccidentChance.Value;
        Being.SpawningChance = sliderBirthChance.Value;
        //cycleDurationInSec = sliderFrequency.CycleTimeInSecs;
    }
}
