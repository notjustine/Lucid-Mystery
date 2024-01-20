using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float directionSize = 10f;
    public Transform cam;

    public KeyCode moveForward = KeyCode.UpArrow;
    public KeyCode moveBackward = KeyCode.DownArrow;
    public KeyCode moveLeft = KeyCode.LeftArrow;
    public KeyCode moveRight = KeyCode.RightArrow;
    public KeyCode attack = KeyCode.Mouse0;

    private float movementCooldown = 0.3f;
    private float lastMoveTime;

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastMoveTime >= movementCooldown)
        {
            Vector3 direction = Vector3.zero;

            if (Input.GetKey(moveForward))
            {
                direction += Vector3.forward;
            }
            if (Input.GetKey(moveBackward))
            {
                direction += Vector3.back;
            }
            if (Input.GetKey(moveLeft))
            {
                direction += Vector3.left;
            }
            if (Input.GetKey(moveRight))
            {
                direction += Vector3.right;
            }

            if (direction != Vector3.zero)
            {
                direction = Quaternion.Euler(0f, cam.eulerAngles.y, 0f) * direction;
                controller.Move(direction * directionSize);
                lastMoveTime = Time.time; // Update the last movement time
            }
        }
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