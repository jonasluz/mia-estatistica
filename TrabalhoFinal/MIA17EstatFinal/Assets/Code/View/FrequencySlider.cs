using UnityEngine;
using UnityEngine.UI;

public class FrequencySlider : MonoBehaviour {

    public string labelBeforeValue;
    public Text uiText;

    public float CycleTimeInSecs
    {
        get
        {
            return 1 / slider.value;
        }
        set
        {
            slider.value = 1 / value;
        }
    }

    [HideInInspector]
    public Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        uiText.text = labelBeforeValue + slider.value + "Hz";
    }
}
