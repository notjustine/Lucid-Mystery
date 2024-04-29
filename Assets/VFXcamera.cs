using UnityEngine;

public class VFXcamera : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust this value to change the movement speed
    public float rotationSpeed = 100f; // Adjust this value to change the rotation speed
    public float verticalSpeed = 3f; // Adjust this value to change the vertical movement speed

    // Limit the vertical rotation angle
    public float maxVerticalAngle = 80f;
    public float minVerticalAngle = -80f;

    private float verticalRotation = 0f;

    void Update()
    {
        // Get input from the WASD keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction relative to the player's forward direction
        Vector3 movement = (transform.forward * verticalInput + transform.right * horizontalInput) * moveSpeed * Time.deltaTime;

        // Move the GameObject
        transform.Translate(movement, Space.World);

        // Get mouse input for rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculate rotation based on mouse input
        float rotationAmountX = mouseX * rotationSpeed * Time.deltaTime;
        float rotationAmountY = mouseY * rotationSpeed * Time.deltaTime;

        // Rotate the GameObject around the Y axis
        transform.Rotate(Vector3.up, rotationAmountX);

        // Update vertical rotation
        verticalRotation -= rotationAmountY;
        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);

        // Apply vertical rotation
        transform.localRotation = Quaternion.Euler(verticalRotation, transform.localEulerAngles.y, 0f);

        // Move the player up when space bar is pressed
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(Vector3.up * verticalSpeed * Time.deltaTime, Space.World);
        }

        // Move the player down when left shift is pressed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.down * verticalSpeed * Time.deltaTime, Space.World);
        }
    }
}
