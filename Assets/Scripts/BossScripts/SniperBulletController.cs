using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/** 
    This handles the flight direction of our sniper bullets from the boss, collision
    detection, and deletion after time of bullets that missed.
*/
public class SniperBulletController : MonoBehaviour
{
    const float rotationSpeed = 100f;
    const float maxLifetime = 1.65f; // TO-DO:  Change this according to difficulty.  Use this value for Easy
    float bulletLifetime;
    private Vector3 rot;
    GameObject sniper;
    GameObject boss;
    private CapsuleCollider bulletCollider;
    private CapsuleCollider bossCollider;
    private DifficultyManager difficultyManager;
    private WarningManager warningManager;
    private float sniperDamage;
    private float sniperBulletSpeed;


    // NOTE:  Since we spawn bullets after a trigger, we can just set the difficulty at that time.  No need to "update" it,
    //  since the next bullet spawned with get the updated values. 
    void Start()
    {
        // Set stats' values based on difficulty setting
        difficultyManager = DifficultyManager.Instance;
        sniperDamage = difficultyManager.GetValue(DifficultyManager.StatName.SNIPER_DAMAGE);
        sniperBulletSpeed = difficultyManager.GetValue(DifficultyManager.StatName.SNIPER_BULLET_SPEED);

        // For highlighting tiles being shot at
        warningManager = FindObjectOfType<WarningManager>();

        // Orient the bullet direction
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
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            PlayerStatus playerStatus = collision.gameObject.GetComponent<PlayerStatus>();
            if (playerStatus != null) // Check if the PlayerStatus component is found
            {
                playerStatus.TakeDamage(sniperDamage);
            }
            else
            {
                // Debug.Log("PlayerStatus component not found on the collided object.");
            }
            Destroy(gameObject);
        }
        else
        {
            // Debug.Log("Something else?");
        }
    }

    void OnDestroy()
    {
        List<string> sniperWarnings = warningManager.GetWarningsOfType(WarningManager.WarningType.SNIPER);
        warningManager.ToggleWarning(sniperWarnings.GetRange(0, 1), false, WarningManager.WarningType.SNIPER);
    }
}
