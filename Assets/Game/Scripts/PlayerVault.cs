using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementAdvanced))]
public class PlayerVault : MonoBehaviour
{
    [Header("Values")]
    [Tooltip("From how far can you vault over an edge.")]
    public float vaultRange = 3f;
    [Tooltip("From how much below the edge a vault will be valided.")]
    public float maxVaultHeight = 0.5f;
    [Tooltip("Counts of detection the edge detector will proceed to to find the closest edge.")]
    public int edgeDetectorInstances = 10;
    [Tooltip("How far from each other every instances of the edge detector will be.")]
    public float edgeDetectorDistance = 0.25f;
    [Tooltip("The bigger this value will be, the more \"thin\" obstacle on a surface will be considered as grabbable.")]
    public float edgeDetectorLength = 0.1f;
    [Tooltip("On what type of surface you are allowed to vault. Select \"everything\" to be able to vault on any surfaces..")]
    public LayerMask vaultLayer;

    [Header("References")]
    public Transform cameraTransform;
    public PlayerMovementAdvanced movements;
    public PlayerInputs inputs;

    private Vector3 vaultSpot;
    private bool hasFoundEdge;
    private List<Vector3> vaultDetectorSpots = new List<Vector3>();

    private void CalculateVaultSpot()
    {
        vaultDetectorSpots.Clear();

        RaycastHit cameraHit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out cameraHit, vaultRange, vaultLayer))
        {
            for (int i = 0; i < edgeDetectorInstances; i++)
            {
                RaycastHit instanceHit;
                Vector3 startPos = cameraHit.point + (cameraHit.normal * edgeDetectorLength);
                Vector3 start = startPos + Vector3.up * i * edgeDetectorDistance;

                if (Physics.Raycast(start, cameraTransform.forward, out instanceHit, edgeDetectorLength * 2, vaultLayer))
                {
                    if (instanceHit.transform.gameObject != cameraHit.transform.gameObject)
                    {
                        break;
                    }

                    vaultDetectorSpots.Add(instanceHit.point);
                }
                else
                {
                    break;
                }
            }
        }

        // Found an edge
        if (vaultDetectorSpots != null)
        {
            if (vaultDetectorSpots.Count < 10 && vaultDetectorSpots.Count > 0)
            {
                Vector3 edge = vaultDetectorSpots[vaultDetectorSpots.Count - 1];
                hasFoundEdge = true;

                vaultSpot = edge;
            }
            else
            {
                hasFoundEdge = false;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeycodeManager.jump))
        {
            CalculateVaultSpot();

            if (hasFoundEdge)
            {
                hasFoundEdge = false;
                transform.position = vaultSpot + Vector3.up * (movements.playerHeight / 2 + 0.1f);
                movements.HasJumped();
            }
        }
    }
}
