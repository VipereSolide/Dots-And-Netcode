using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomInputManager
{
    public static bool GetKey(CustomKeyCode keycode)
    {
        if (keycode == CustomKeyCode.MouseWheelUp)
        {
            return Input.GetAxisRaw("Mouse ScrollWheel") > 0;
        }
        
        if (keycode == CustomKeyCode.MouseWheelDown)
        {
            return Input.GetAxisRaw("Mouse ScrollWheel") < 0;
        }
        
            return Input.GetKey(ToNormal(keycode));
    }

    public static bool GetKeyDown(CustomKeyCode keycode)
    {
        if (keycode == CustomKeyCode.MouseWheelUp)
        {
            return Input.GetAxisRaw("Mouse ScrollWheel") > 0;
        }
        
        if (keycode == CustomKeyCode.MouseWheelDown)
        {
            return Input.GetAxisRaw("Mouse ScrollWheel") < 0;
        }
        
            return Input.GetKeyDown(ToNormal(keycode));
    }

    public static bool GetKeyUp(CustomKeyCode keycode)
    {
        bool wasDown = GetKey(keycode);

        if (keycode == CustomKeyCode.MouseWheelUp || keycode == CustomKeyCode.MouseWheelDown)
        {
            return wasDown != GetKey(keycode);
        }
        else
        {
            return Input.GetKeyUp(ToNormal(keycode));
        }
    }

    public static KeyCode ToNormal(this CustomKeyCode keycode)
    {
        return (KeyCode)(int)keycode;
    }
}