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
    
    // Camera shake parameters
    private const float shakeDuration = 0.15f;
    private const float shakeMagnitude = 0.5f;
    private bool isShaking = false;

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
        if (!isShaking)
            transform.LookAt(bossTransform.position);
    }
     public void TriggerShake()
    {
        StartCoroutine(ShakeCamera());
    }

    private IEnumerator ShakeCamera()
    {
        Quaternion originalRotation = transform.localRotation;
        float elapsed = 0.0f;
        isShaking = true;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localRotation = originalRotation * Quaternion.Euler(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }
        isShaking = false;
        transform.localRotation = originalRotation;
    }

}





