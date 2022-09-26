using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SliderValueToInputField : MonoBehaviour
{
    public int sliderValueMaxDigit;
    public TMP_InputField inputfield;
    public Slider slider;

    [SerializeField]
    public UnityEvent<float> onValueChanged;

    private void Start()
    {
        slider.onValueChanged.AddListener(UpdateInputField);

        UpdateInputField(slider.value);
        inputfield.onValueChanged.AddListener(UpdateSlider);
    }

    private void OnApplicationQuit()
    {
        slider.onValueChanged.RemoveListener(UpdateInputField);
        inputfield.onValueChanged.RemoveListener(UpdateSlider);
    }

    private void UpdateInputField(float value)
    {
        float roundValue = Round(value, sliderValueMaxDigit);
        slider.value = roundValue;
        inputfield.text = roundValue.ToString();

        onValueChanged.Invoke(roundValue);
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
        onValueChanged.Invoke(parsed);
    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }
}
