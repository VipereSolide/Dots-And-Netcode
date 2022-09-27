using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainRipple : MonoBehaviour
{
    public Transform player;
    public float heightOffset;
    public float raycastHeight;
    public float recalculateHeightOffset;
    public LayerMask groundLayer;

    private Vector3 playerLastPosition;
    private float height;

    private void Update()
    {
        if (Vector3.Distance(player.position, playerLastPosition) >= recalculateHeightOffset)
        {
            RecalculateHeight();
        }

        transform.position = new Vector3(player.position.x, height, player.position.z);
        playerLastPosition = player.position;
    }

    private void Start()
    {
        RecalculateHeight();
    }

    private void RecalculateHeight()
    {
        RaycastHit hit;

        if (Physics.Raycast(player.position + Vector3.up * raycastHeight, Vector3.down, out hit, raycastHeight + 10f, groundLayer))
        {
            height = hit.point.y + heightOffset;
        }
    }
}
