using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class PercentSliderEditor : MonoBehaviour {

    static IEnumerable<PercentSlider> sliders;

    static PercentSliderEditor()
    {
        Debug.Log("Rotina de editor PercentSliderEditor ativada.");
        EditorApplication.update += Update;
    }

    static void Update() 
    {
        sliders = FindObjectsOfType<PercentSlider>();
        if (sliders == null) return;
        
        foreach (PercentSlider slider in sliders)
            slider.uiText.text = slider.labelBeforeValue;
    }
}
