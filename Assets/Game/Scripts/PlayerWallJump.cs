using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJump : MonoBehaviour
{
    public PlayerMovementAdvanced movement;
    public Transform cameraTransform;
    public Rigidbody rigidbody;
    public float walljumpDistance;
    public float walljumpForce;
    public float walljumpVerticalForce;
    public int walljumpCount;
    [HideInInspector] public int currentWalljumpCount;

    private bool wasGrounded;

    private void Update()
    {
        if (currentWalljumpCount > 0 && !wasGrounded && movement.grounded)
        {
            movement.restricted = false;
            currentWalljumpCount = 0;
            return;
        }

        if (movement.state == PlayerMovementAdvanced.MovementState.air && currentWalljumpCount < walljumpCount)
        {
            if (CustomInputManager.GetKeyDown(KeycodeManager.jump))
            {
                RaycastHit hit;
                if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, walljumpDistance))
                {
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.AddForce(transform.up * walljumpVerticalForce + hit.normal * walljumpForce, ForceMode.Impulse);
                    currentWalljumpCount++;
                    movement.restricted = true;
                }
            }
        }

        wasGrounded = movement.grounded;
    }
}
