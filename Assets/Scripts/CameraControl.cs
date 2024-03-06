using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform bossTransform; 
    public float distanceBehindPlayer = 10f;
    public float heightAbove = 5f;
    public float smoothSpeed = 5f; 

    private Vector3 cameraDirection;
    private Vector3 desiredPosition;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        bossTransform = GameObject.FindGameObjectWithTag("Boss").transform;
    }

    void LateUpdate()
    {
        // Calculate the direction from the boss to the player
        cameraDirection = (playerTransform.position - bossTransform.position).normalized;

        // Calculate a point on the line defined by the boss and player positions
        desiredPosition = playerTransform.position + cameraDirection * distanceBehindPlayer;
        desiredPosition.y += heightAbove;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Set the camera's position
        transform.position = smoothedPosition;

        // Make sure the camera is always looking at the boss
        transform.LookAt(bossTransform.position);
    }
}





