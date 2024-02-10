using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
Dylan:  this handles the flight of our sniper bullets from the boss

*/
public class BulletController : MonoBehaviour
{

    const float sniperBulletSpeed = 80f;
    const float rotationSpeed = 100f;
    const float maxLifetime = 1f;

    float bulletLifetime;
    private Vector3 rot;
    GameObject sniper;
    // Start is called before the first frame update
    void Start()
    {
        sniper = GameObject.Find("Sniper");
        bulletLifetime = 0f;
        // Vector3 direction = transform.forward;
        rot = Vector3.RotateTowards(transform.forward, sniper.transform.forward, rotationSpeed, 0.0f); // last one is the error margin of rotation

        transform.rotation = Quaternion.LookRotation(rot);

        Rigidbody bulletBody = GetComponent<Rigidbody>();
        bulletBody.AddForce(transform.forward * sniperBulletSpeed, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        bulletLifetime += Time.deltaTime;

        if (bulletLifetime > maxLifetime)
        {
            Destroy(gameObject);
        }
    }

    /**
    If bullet collides with player, bullet should be destroyed.
    */
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is not the player
        if (collision.gameObject.tag == "Player" | collision.gameObject.tag == "Arena")
        {
            Debug.Log("collided with player or arena");
            Destroy(gameObject); // Destroy bullet on collision
        }
        else
        {
            Debug.Log("Something else?");
        }
    }
}
