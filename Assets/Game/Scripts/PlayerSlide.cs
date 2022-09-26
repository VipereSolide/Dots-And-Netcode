using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{
    public Transform cameraPos;
    public PlayerMovementAdvanced movement;
    public Rigidbody rigidbody;
    public PlayerInputs inputs;
    public CameraShake shake;

    [Tooltip("The position of the camera when you're sliding.")]
    public float slidingCameraPosition;
    [Tooltip("How hard will sliding shake the camera.")]
    public float cameraShakeAmount;
    [Tooltip("How many shake will there be when sliding.")]
    public int cameraShakeCount;
    [Tooltip("The time between each shakes.")]
    public float cameraShakeDelay;
    [Tooltip("The minimum time before 2 concecutive slides.")]
    public float slideCooldown;
    [Tooltip("How fast the player will slide.")]
    public float slideSpeed;
    [Tooltip("How long in seconds will the slide last if not canceled before.")]
    public float slideDuration;
    [Tooltip("Starting from what time will the slide be cancelable.")]
    public float slideCancelTime;

    private float elapsedSlideTime;

    private bool canSlide = true;
    private bool isSliding = false;

    private Vector3 storedDirection;

    private void Update()
    {
        if (CustomInputManager.GetKeyDown(KeycodeManager.slide) && canSlide && inputs.isMoving && movement.state != PlayerMovementAdvanced.MovementState.freeze && movement.state != PlayerMovementAdvanced.MovementState.air)
        {
            storedDirection = movement.moveDirection.normalized;
            StartSlide();
        }

        if (isSliding)
        {
            rigidbody.AddForce(storedDirection * 10 * slideSpeed / (slideDuration / elapsedSlideTime), ForceMode.Force);

            elapsedSlideTime -= Time.deltaTime;

            if (slideDuration - elapsedSlideTime > slideCancelTime)
            {
                if (CustomInputManager.GetKeyDown(KeycodeManager.jump))
                {
                    StopSlide();
                }
            }

            if (elapsedSlideTime <= 0)
            {
                StopSlide();
            }
        }
    }

    private void StopSlide()
    {
        isSliding = false;
        canSlide = true;

        movement.sliding = false;
        movement.restricted = false;

        transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
        elapsedSlideTime = 0;
    }

    private void StartSlide()
    {
        isSliding = true;
        canSlide = false;

        movement.sliding = true;
        movement.restricted = true;

        shake.PlayShake(cameraShakeAmount, cameraShakeCount, cameraShakeDelay);

        transform.localScale = new Vector3(transform.localScale.x, slidingCameraPosition, transform.localScale.z);
        rigidbody.AddForce(Vector3.down * 4f, ForceMode.Impulse);

        elapsedSlideTime = slideDuration;
    }
}
