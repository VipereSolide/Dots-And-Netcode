using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movements : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody rigidbody;

    [SerializeField] private float groundDrag;
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private float jumpForce;
    [SerializeField] private float airMultiplier;

    [SerializeField] private float walkingSpeed;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float grapplingSpeed;

    private float currentSpeed;
    
    private bool isGrounded;

    private float inputX;
    private float inputZ;

    [HideInInspector] public bool isGrappling;

    private bool isRunning;
    private Vector3 moveDirection;

    private void Update()
    {
        GetInputs();
        currentSpeed = GetCurrentSpeed();

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.22f, groundMask);

        if (Input.GetKeyDown(KeycodeManager.jump) && isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (!isGrappling)
            MovePlayer();
    }

    private void Jump()
    {
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * inputZ + orientation.right * inputX;

        if (isGrounded)
        {
            rigidbody.AddForce(Vector3.ClampMagnitude(moveDirection, 1) * 10 * currentSpeed, ForceMode.Force);
            rigidbody.drag = groundDrag;
        }
        else
        {
            rigidbody.AddForce(Vector3.ClampMagnitude(moveDirection, 1) * 10 * currentSpeed * airMultiplier, ForceMode.Force);
            rigidbody.drag = 0;
        }

        SpeedControl();
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
        float maxSpeed = (isGrappling) ? grapplingSpeed : currentSpeed;

        if (flatVelocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * maxSpeed;
            rigidbody.velocity = new Vector3(limitedVelocity.x, rigidbody.velocity.y, limitedVelocity.z);
        }
    }

    private void GetInputs()
    {
        if (Input.GetKey(KeycodeManager.forward)) inputZ = Mathf.Lerp(inputZ, 1, Time.deltaTime * KeycodeManager.inputLerpSpeed);
        if (Input.GetKey(KeycodeManager.backward)) inputZ = Mathf.Lerp(inputZ, -1, Time.deltaTime * KeycodeManager.inputLerpSpeed);
        if ((Input.GetKey(KeycodeManager.forward) && Input.GetKey(KeycodeManager.backward)) || (!Input.GetKey(KeycodeManager.forward) && !Input.GetKey(KeycodeManager.backward)))
            inputZ = Mathf.Lerp(inputZ, 0, Time.deltaTime * KeycodeManager.inputLerpSpeed);

        if (Input.GetKey(KeycodeManager.right)) inputX = Mathf.Lerp(inputX, 1, Time.deltaTime * KeycodeManager.inputLerpSpeed);
        if (Input.GetKey(KeycodeManager.left)) inputX = Mathf.Lerp(inputX, -1, Time.deltaTime * KeycodeManager.inputLerpSpeed);
        if ((Input.GetKey(KeycodeManager.right) && Input.GetKey(KeycodeManager.left)) || (!Input.GetKey(KeycodeManager.right) && !Input.GetKey(KeycodeManager.left)))
            inputX = Mathf.Lerp(inputX, 0, Time.deltaTime * KeycodeManager.inputLerpSpeed);

        isRunning = Input.GetKey(KeycodeManager.run);
    }

    public float GetCurrentSpeed()
    {
        if (isRunning)
        {
            return runningSpeed;
        }
        else
        {
            return walkingSpeed;
        }
    }
}