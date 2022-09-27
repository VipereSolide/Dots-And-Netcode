using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingDani : MonoBehaviour
{
    [Header("References")]
    public Transform gunTip, cam, player;
    public Rigidbody rigidbody;
    public PlayerMovementAdvanced movements;
    public LayerMask whatIsGrappleable;

    [Header("Swinging")]
    public float maxSwingDistance = 25f;
    public float grapplingCooldown = 1f;
    public float forwardSpeed;
    private SpringJoint joint;

    [Header("Input")]
    public KeyCode swingKey = KeyCode.Mouse0;

    public bool askForSwinging;
    public bool isSwinging;
    public Vector3 swingPoint;

    private float grapplingCdTimer;

    void Update()
    {
        //if (Input.GetKeyDown(swingKey)) StartSwing();
        if (Input.GetKeyUp(swingKey)) StopSwing();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartSwing();
        }

        if (isSwinging && CustomInputManager.GetKey(KeycodeManager.forward))
        {
            rigidbody.AddForce(movements.moveDirection.normalized * forwardSpeed * 10f * Time.deltaTime, ForceMode.Impulse);
        }

        movements.grappling = isSwinging;
    }

    /*void LateUpdate()
    {
        DrawRope();
    }*/

    void StartSwing()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxSwingDistance, whatIsGrappleable))
        {
            swingPoint = hit.point;
            isSwinging = true;

            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

            // the distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            // customize values as you like
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            currentGrapplePosition = gunTip.position;
        }
    }

    void StopSwing()
    {
        Destroy(joint);

        isSwinging = false;
        askForSwinging = false;
        grapplingCdTimer = grapplingCooldown;
    }

    private Vector3 currentGrapplePosition;

    void DrawRope()
    {
        // if not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);
    }
}
