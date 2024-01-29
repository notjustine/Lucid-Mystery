using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform playerTransform;
    public Transform bossTransform; // Assign the boss's transform here in the inspector
    public float distanceBehindPlayer = 10f; // Distance behind the player
    public float heightAbove = 5f;
    public float smoothSpeed = 5f; // Adjust this for smoother camera movement

    private Vector3 cameraDirection;
    private Vector3 desiredPosition;

    void LateUpdate()
    {
        // Calculate the direction from the boss to the player
        cameraDirection = (playerTransform.position - bossTransform.position).normalized;

        // Calculate the desired position: a point on the line defined by the boss and player positions
        desiredPosition = playerTransform.position + cameraDirection * distanceBehindPlayer;
        desiredPosition.y += heightAbove; // Add height offset

        // Smoothly interpolate to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Set the camera's position
        transform.position = smoothedPosition;

        // Make sure the camera is always looking at the boss
        transform.LookAt(bossTransform.position);
    }
}





