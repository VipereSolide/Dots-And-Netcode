using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class KeySelector : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text selectedKeyText;
    public GameObject unSelected;
    [SerializeField] private CustomKeyCode heldKey;
    public bool saveValue;
    public string valueTag;

    [SerializeField]
    public UnityEvent<CustomKeyCode> onValueChanged;

    private bool isListening;

    private List<char> capsAlphabet = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

    public void Null()
    {
        heldKey = CustomKeyCode.None;
        onValueChanged.Invoke(CustomKeyCode.None);

        UpdateDisplay();
    }

    public void SetKey(CustomKeyCode key)
    {
        if (key == CustomKeyCode.None) Null();
        if (key == heldKey) return;

        heldKey = key;
        onValueChanged.Invoke(key);

        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        string keyText = heldKey.ToString();

        int addedLetters = 0;
        string newText = keyText;

        for(int i = 0; i < keyText.Length; i++)
        {
            // if key is a caps and not the first letter
            if (capsAlphabet.Contains(keyText[i]) && i > 0)
            {
                newText = newText.Insert(i + addedLetters, " ");
                addedLetters++;
            }
        }

        selectedKeyText.text = newText;
        selectedKeyText.gameObject.SetActive(heldKey != CustomKeyCode.None);
        unSelected.SetActive(heldKey == CustomKeyCode.None);
    }

    private void Update()
    {
        if (isListening)
        {
            selectedKeyText.text = "...";

            foreach (string keyName in System.Enum.GetNames(typeof(CustomKeyCode)))
            {
                CustomKeyCode key = (CustomKeyCode)System.Enum.Parse(typeof(CustomKeyCode), keyName);

                if (CustomInputManager.GetKeyDown(key))
                {
                    if (key == heldKey)
                    {
                        isListening = false;
                        UpdateDisplay();
                        return;
                    }

                    if (key == CustomKeyCode.Escape || key == CustomKeyCode.Delete)
                    {
                        Null();
                        break;
                    }

                    SetKey(key);
                    isListening = false;
                    break;
                }
            }
        }
    }

    private void Start()
    {
        if (saveValue)
        {
            if (PlayerPrefs.HasKey(valueTag))
            {
                SetKey((CustomKeyCode)PlayerPrefs.GetInt(valueTag));
            }
        }

        UpdateDisplay();
    }

    private void OnApplicationQuit()
    {
        if (saveValue) PlayerPrefs.SetInt(valueTag, (int)heldKey);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isListening = true;
    }
}
