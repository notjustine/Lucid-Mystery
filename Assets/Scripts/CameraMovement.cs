using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 3f, -10f);
    public float smoothSpeed = 0.125f;
    public float sensitivity = 2f;

    private float currentRotationX;
    private float currentRotationY;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        //rotation
        currentRotationX -= mouseY;
        currentRotationY += mouseX;
        currentRotationX = Mathf.Clamp(currentRotationX, -90, 90);
        transform.eulerAngles = new Vector3(currentRotationX, currentRotationY, 0);
    }

    void LateUpdate()
    {
        // with smooth moving
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

    }
}