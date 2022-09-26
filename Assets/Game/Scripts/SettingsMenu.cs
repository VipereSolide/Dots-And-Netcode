using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public SliderValueToInputField sensitivity;
    public KeySelector moveForward;
    public KeySelector moveBackward;
    public KeySelector moveLeft;
    public KeySelector moveRight;
    public KeySelector jump;
    public KeySelector crouch;
    public KeySelector sprint;
    public KeySelector mantle;

    private void Start()
    {
        sensitivity.onValueChanged.AddListener((float f) => { KeycodeManager.sensitivity = f; });
        moveForward.onValueChanged.AddListener((CustomKeyCode k) => { KeycodeManager.forward = k; });
        moveBackward.onValueChanged.AddListener((CustomKeyCode k) => { KeycodeManager.backward = k; });
        moveLeft.onValueChanged.AddListener((CustomKeyCode k) => { KeycodeManager.left = k; });
        moveRight.onValueChanged.AddListener((CustomKeyCode k) => { KeycodeManager.right = k; });
        jump.onValueChanged.AddListener((CustomKeyCode k) => { KeycodeManager.jump = k; });
        crouch.onValueChanged.AddListener((CustomKeyCode k) => { KeycodeManager.crouch = k; });
        sprint.onValueChanged.AddListener((CustomKeyCode k) => { KeycodeManager.run = k; });
        mantle.onValueChanged.AddListener((CustomKeyCode k) => { KeycodeManager.mantle = k; });
    }
}
