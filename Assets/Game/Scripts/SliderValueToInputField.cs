using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SliderValueToInputField : MonoBehaviour
{
    public int sliderValueMaxDigit;
    public TMP_InputField inputfield;
    public Slider slider;

    private void Start()
    {
        inputfield.onValueChanged.AddListener(UpdateSlider);
        slider.onValueChanged.AddListener(UpdateInputField);
    }

    private void OnApplicationQuit()
    {
        inputfield.onValueChanged.RemoveListener(UpdateSlider);
        slider.onValueChanged.RemoveListener(UpdateInputField);
    }

    private void UpdateInputField(float value)
    {
        float roundValue = Round(value, sliderValueMaxDigit);
        slider.value = roundValue;
        inputfield.text = roundValue.ToString();
    }

    private void UpdateSlider(string value)
    {
        float parsed = slider.value;
        
        if (!float.TryParse(value, out parsed))
        {
            UpdateInputField(slider.value);
            return;
        }

        slider.value = parsed;
    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }
}
