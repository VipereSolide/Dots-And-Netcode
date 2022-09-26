using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;

    public Vector2 DirectionInput
    {
        get { return new Vector2(horizontalInput, verticalInput); }
    }

    public bool isMoving
    {
        get { return DirectionInput.x != 0 || DirectionInput.y != 0; }
    }

    private void Update()
    {
        if (CustomInputManager.GetKey(KeycodeManager.right)) horizontalInput = 1;
        if (CustomInputManager.GetKey(KeycodeManager.left)) horizontalInput = -1;
        if ((CustomInputManager.GetKey(KeycodeManager.left) && CustomInputManager.GetKey(KeycodeManager.right)) || (!CustomInputManager.GetKey(KeycodeManager.left) && !CustomInputManager.GetKey(KeycodeManager.right)))
            horizontalInput = 0;

        if (CustomInputManager.GetKey(KeycodeManager.forward)) verticalInput = 1;
        if (CustomInputManager.GetKey(KeycodeManager.backward)) verticalInput = -1;
        if ((CustomInputManager.GetKey(KeycodeManager.backward) && CustomInputManager.GetKey(KeycodeManager.forward)) || (!CustomInputManager.GetKey(KeycodeManager.backward) && !CustomInputManager.GetKey(KeycodeManager.forward)))
            verticalInput = 0;
    }
}
