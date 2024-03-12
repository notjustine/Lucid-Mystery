using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/** 
Dylan:  this handles the flight direction of our sniper bullets from the boss, collision
detection, and deletion after time of bullets that missed.
*/
public class SprayBulletController : MonoBehaviour
{
    const float maxLifetime = 2f;
    float bulletLifetime;

    GameObject boss;
    private CapsuleCollider bulletCollider;
    private CapsuleCollider bossCollider;
    private DifficultyManager difficultyManager;
    private float sprayBulletDamage;
    private float sprayBulletSpeed;

    void Start()
    {
        // Set up difficulty settings
        difficultyManager = FindObjectOfType<DifficultyManager>();
        sprayBulletDamage = difficultyManager.GetValue(DifficultyManager.StatName.SPRAY_BULLET_DAMAGE);
        sprayBulletSpeed = difficultyManager.GetValue(DifficultyManager.StatName.SPRAY_BULLET_SPEED);
        
        boss = GameObject.Find("Cube");  // We should fix this name.

        bulletLifetime = 0f;

        Rigidbody bulletBody = GetComponent<Rigidbody>();
        bulletBody.AddForce(transform.forward * sprayBulletSpeed, ForceMode.VelocityChange);

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
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Weapon")
        {
            PlayerStatus playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>(); ;
            if (playerStatus != null) // Check if the PlayerStatus component is found
            {
                playerStatus.TakeDamage(sprayBulletDamage);
            }
            else
            {
                // Debug.Log("PlayerStatus component not found on the collided object.");
            }
            Destroy(gameObject);
        }
    }
}
