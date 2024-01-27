using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Assign the player object
    private float offset = 15f; // Offset from the player position
    public Transform centerPoint; // Center point of the circle
    public Vector3 vertical = new Vector3(0, 5f, 0);

    void Update()
    {
        Vector3 direction = (player.position - centerPoint.position).normalized;
        // Move camera to player's position with an offset
        transform.position = player.position + (direction) * offset + vertical;

        // Rotate camera to always look at the center point
        transform.LookAt(centerPoint.position);
    }
}