using UnityEngine;


/** 
This handles the flight direction of our sniper bullets from the boss, collision
detection, and deletion after time of bullets that missed.
*/
public class SpiralBulletController : MonoBehaviour
{
    const float maxLifetime = 2f;
    float bulletLifetime;

    GameObject boss;
    private CapsuleCollider bulletCollider;
    private CapsuleCollider bossCollider;
    private DifficultyManager difficultyManager;
    private float spiralBulletDamage;
    private float spiralBulletSpeed;

    void Start()
    {
        // Set up difficulty settings
        difficultyManager = DifficultyManager.Instance;
        spiralBulletDamage = difficultyManager.GetValue(DifficultyManager.StatName.SPIRAL_BULLET_DAMAGE);
        spiralBulletSpeed = difficultyManager.GetValue(DifficultyManager.StatName.SPIRAL_BULLET_SPEED);

        boss = GameObject.Find("Cube");  // We should fix this name.

        bulletLifetime = 0f;

        Rigidbody bulletBody = GetComponent<Rigidbody>();
        bulletBody.AddForce(transform.forward * spiralBulletSpeed, ForceMode.VelocityChange);

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
            PlayerStatus playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
            if (playerStatus != null) // Check if the PlayerStatus component is found
            {
                playerStatus.TakeDamage(spiralBulletDamage);
            }
            else
            {
                // Debug.Log("PlayerStatus component not found on the collided object.");
            }
            Destroy(gameObject);
        }
    }
}
