using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    public PlayerMovementAdvanced player;
    public PlayerCam camera;
    public GameObject escapeMenu;
    public GameObject settingsMenu;

    private bool isPaused;
    private bool isFullscreened;

    public void Pause(bool value)
    {
        isPaused = value;

        camera.freeze = value;
        player.freeze = value;

        if (value)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void SetEscapeMenu(bool value)
    {
        escapeMenu.SetActive(value);
        settingsMenu.SetActive(false);
    }

    public void SetSettingsMenu(bool value)
    {
        escapeMenu.SetActive(false);
        settingsMenu.SetActive(value);
    }

    public void ToggleFullScreen()
    {
        isFullscreened = !isFullscreened;
        UpdateFullScreen();
    }

    private void UpdateFullScreen()
    {
        if (isFullscreened && !Screen.fullScreen)
        {
            Screen.fullScreen = true;
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        
        if (!isFullscreened && Screen.fullScreen)
        {
            Screen.fullScreen = false;
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Start()
    {
        Pause(false);
        SetEscapeMenu(false);
        SetSettingsMenu(false);
    }

    private void Update()
    {
        if (CustomInputManager.GetKeyDown(KeycodeManager.pauseMenu))
        {
            Pause(!isPaused);
            SetEscapeMenu(isPaused);
        }

        if (CustomInputManager.GetKeyDown(KeycodeManager.toggleFullscreen))
        {
            ToggleFullScreen();
        }
    }
}
