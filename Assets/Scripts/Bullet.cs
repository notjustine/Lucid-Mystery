using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;

    private void Start()
    {
        Vector3 direction = (Vector3.zero - transform.position).normalized;
        GetComponent<Rigidbody>().velocity = direction * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is not the player
        if (collision.gameObject.tag != "Player" | collision.gameObject.tag != "Enviorment")
        {
            Destroy(gameObject); // Destroy bullet on collision
        }
    }
}


