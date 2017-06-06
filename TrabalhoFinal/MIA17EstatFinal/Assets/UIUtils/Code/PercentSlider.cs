using UnityEngine;
using UnityEngine.UI;

public class PercentSlider : MonoBehaviour {

    public string labelBeforeValue;
    public Text uiText;

    public float Value
    {
        get
        {
            return slider.value / 100;
        }
        set
        {
            slider.value = value * 100;
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
        uiText.text = labelBeforeValue + slider.value + "%";
    }
}
