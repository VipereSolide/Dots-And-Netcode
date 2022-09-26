using UnityEngine;

public static class KeycodeManager
{
    public static KeyCode fire = KeyCode.Mouse0;
    public static KeyCode aim = KeyCode.Mouse1;
    public static KeyCode forward = KeyCode.Z;
    public static KeyCode backward = KeyCode.S;
    public static KeyCode right = KeyCode.D;
    public static KeyCode left = KeyCode.Q;
    public static KeyCode jump = KeyCode.Space;
    public static KeyCode run = KeyCode.A;
    public static KeyCode crouch = KeyCode.X;
    public static KeyCode precisionGrapple = KeyCode.Mouse0;
    public static KeyCode swingingGrapple = KeyCode.Mouse1;
    public static KeyCode extendGrapplingCable = KeyCode.S;
    public static KeyCode shortenGrapplingCable = KeyCode.Space;
    public static KeyCode wallrunUpwards = KeyCode.A;
    public static KeyCode wallrunDownwards = KeyCode.X;
    public static KeyCode slide = KeyCode.C;

    public static float inputLerpSpeed = 1000f;

    [System.Serializable]
    public class Key
    {

    }
}