using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraplingGun : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Movements movements;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform camera;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask grappleable;
    [SerializeField] private float maxSwingDistance = 25f;
    [SerializeField] private Vector3 swingPoint;
    [SerializeField] private SpringJoint joint;
    [SerializeField] private float spring = 4.5f;
    [SerializeField] private float damper = 7f;
    [SerializeField] private float massScale = 4.5f;
    [SerializeField] private float horizontalForce;
    [SerializeField] private float forwardForce;
    [SerializeField] private float extendCableSpeed;

    bool grappling = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeycodeManager.fire)) StartSwing();    
        if (Input.GetKeyUp(KeycodeManager.fire)) StopSwing();

        DrawRope();
        OnGrapplingMovement();
    }

    private void DrawRope()
    {
        if (joint == null)
            return;

        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, swingPoint);
    }

    private void StartSwing()
    {
        grappling = true;
        movements.isGrappling = true;

        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxSwingDistance, grappleable))
        {
            swingPoint = hit.point;
            
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            RecalculateJointDistance();

            joint.spring = spring;
            joint.damper = damper;
            joint.massScale = massScale;

            lineRenderer.positionCount = 2;
        }
    }

    private void OnGrapplingMovement()
    {
        if (!grappling) return;

        if (Input.GetKey(KeycodeManager.right)) rb.AddForce(orientation.right * horizontalForce * Time.deltaTime);
        if (Input.GetKey(KeycodeManager.left)) rb.AddForce(-orientation.right * horizontalForce * Time.deltaTime);
        if (Input.GetKey(KeycodeManager.forward)) rb.AddForce(orientation.forward * forwardForce * Time.deltaTime);

        if (Input.GetKey(KeycodeManager.jump))
        {
            Vector3 directionToPoint = swingPoint - transform.position;
            rb.AddForce(directionToPoint.normalized * forwardForce * extendCableSpeed * Time.deltaTime);
            RecalculateJointDistance();
        }

        if (Input.GetKey(KeycodeManager.backward))
        {
            float distanceFromPoint = Vector3.Distance(player.position, swingPoint) * extendCableSpeed;

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
        }
    }

    private void RecalculateJointDistance()
    {
        float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;
    }

    private void StopSwing()
    {
        grappling = false;
        movements.isGrappling = false;
        if (joint == null) return;

        lineRenderer.positionCount = 0;
        Destroy(joint);
    }
}