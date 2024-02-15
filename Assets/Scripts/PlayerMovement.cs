using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float directionSize = 120f;
    public Transform cam;
    public static PlayerMovement Instance { get; private set; }

    public KeyCode moveForward = KeyCode.UpArrow;
    public KeyCode moveBackward = KeyCode.DownArrow;
    public KeyCode moveLeft = KeyCode.LeftArrow;
    public KeyCode moveRight = KeyCode.RightArrow;
    public KeyCode attack = KeyCode.Mouse0;

    public Color color;
    public Material material;

    private float movementCooldown = 0.3f;
    private float lastMoveTime;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Input Indicator");
        }

        Instance = this;
    }

    public void UpdateInputHelper()
    {
        
        if (Time.time - lastMoveTime >= movementCooldown)
        {
            Vector3 direction = Vector3.zero;

            if (Input.GetKey(moveForward))
            {
                direction += cam.forward;
            }
            else if (Input.GetKey(moveBackward))
            {
                direction -= cam.forward;
            }
            else if (Input.GetKey(moveLeft))
            {
                direction -= cam.right;
            }
            else if (Input.GetKey(moveRight))
            {
                direction += cam.right;
            }

            if (direction != Vector3.zero)
            {
                direction.Normalize(); // Normalize the direction vector
                direction.y = 0f;
                controller.Move(direction * directionSize); // Multiply by Time.deltaTime
                lastMoveTime = Time.time; // Update the last movement time 
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        material.color = color;
        UpdateInputHelper();
        if (Input.GetKey(attack))
        {
            Attack();
        }
    }

    void Attack()
    {
        return;
    }
}
