using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/** 
Dylan:  this handles the flight direction of our sniper bullets from the boss, collision
detection, and deletion after time of bullets that missed.
*/
public class BulletController : MonoBehaviour
{
    const float sniperBulletSpeed = 60f;
    const float rotationSpeed = 100f;
    const float maxLifetime = 1f;
    float bulletLifetime;
    private Vector3 rot;
    GameObject sniper;
    GameObject boss;
    private CapsuleCollider bulletCollider;
    private CapsuleCollider bossCollider;
    public float damage = 20f;

    void Start()
    {
        sniper = GameObject.Find("Sniper");
        boss = GameObject.Find("Cube");  // We should fix this name.

        bulletLifetime = 0f;
        // Below: calculates a vector that is one "frame" closer to sniper.transform.forward direction 
        rot = Vector3.RotateTowards(transform.forward, sniper.transform.forward, rotationSpeed, 0.0f); // last one is the error margin of rotation
        // Below: applies the calculated vector to the rotation of the bullet.
        transform.rotation = Quaternion.LookRotation(rot);

        Rigidbody bulletBody = GetComponent<Rigidbody>();
        bulletBody.AddForce(transform.forward * sniperBulletSpeed, ForceMode.VelocityChange);

        //  ignore collisions between bullet and boss:
        bulletCollider = GetComponent<CapsuleCollider>();
        bossCollider = boss.GetComponent<CapsuleCollider>();
        Physics.IgnoreCollision(bulletCollider, bossCollider);
    }


    void Update()
    {
        // Destroy bullet after maxLifetime has passed.
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
        if (collision.gameObject.tag == "Arena")
        {
            Debug.Log("collided with arena");
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            PlayerStatus playerStatus = collision.gameObject.GetComponent<PlayerStatus>();
            if (playerStatus != null) // Check if the PlayerStatus component is found
            {
                playerStatus.TakeDamage(damage);
                Debug.Log("collided with player");
            }
            else
            {
                Debug.Log("PlayerStatus component not found on the collided object.");
            }
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Something else?");
        }
    }
}
