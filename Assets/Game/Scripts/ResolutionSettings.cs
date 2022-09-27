using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionSettings : MonoBehaviour
{
    public Vector2 resolutionPreviewSize = new Vector2(480, 270);
    public TMP_InputField resolutionWidth;
    public TMP_InputField resolutionHeight;
    public RectTransform resolutionPreviewTransform;
    public string saveTag;

    private Vector2Int currentResolution = new Vector2Int(1920, 1080);

    private void Start()
    {
        if (PlayerPrefs.HasKey(saveTag + "_w"))
        {
            currentResolution.x = PlayerPrefs.GetInt(saveTag + "_w");
            currentResolution.y = PlayerPrefs.GetInt(saveTag + "_h");
        }

        resolutionWidth.text = currentResolution.x.ToString();
        resolutionHeight.text = currentResolution.y.ToString();

        resolutionWidth.onValueChanged.AddListener(OnWidthChanged);
        resolutionHeight.onValueChanged.AddListener(OnHeightChanged);

        UpdateResolution();
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt(saveTag + "_w", currentResolution.x);
        PlayerPrefs.SetInt(saveTag + "_h", currentResolution.y);
    }

    private Vector2 GetAdaptedSize(float width, float height)
    {
        return new Vector2(width / 1920 * resolutionPreviewSize.x, height / 1080 * resolutionPreviewSize.y);
    }

    private void OnHeightChanged(string value) { UpdateResolution(); }
    private void OnWidthChanged(string value) { UpdateResolution(); }

    public void ApplyResolution()
    {
        Screen.SetResolution(currentResolution.x, currentResolution.y, Screen.fullScreen);
    }

    private void UpdateResolution()
    {
        int w = 1920;
        int h = 1080;

        if (!int.TryParse(resolutionWidth.text, out w)) { return; }
        if (!int.TryParse(resolutionHeight.text, out h)) { return; }

        currentResolution = new Vector2Int(w, h);
        resolutionPreviewTransform.sizeDelta = GetAdaptedSize(w,h);
    }
}
