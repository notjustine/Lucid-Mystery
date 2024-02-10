using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitSniperShooting : MonoBehaviour
{
    private ShootSniperBullet sniper;
    private GameObject player;
    private Vector3 playerShootPosition;
    private Vector3 prevRotation;
    private const float turretRotationSpeed = 4f;
    private bool shouldShoot;
    private bool shotRequested;
    private bool readyToShoot;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        sniper = FindObjectOfType<ShootSniperBullet>();
        playerShootPosition = player.transform.position;
        shotRequested = false;
        readyToShoot = false;
        shouldShoot = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shouldShoot = true;
            playerShootPosition = player.transform.position;  // the location that the player was when they whiffed, not current.
        }
        if (shouldShoot)
        {
            AimAtPlayer();
            if (!shotRequested)
            {
                // We don't want to trigger the coroutine on every frame, only once per Spacebar tap.
                StartCoroutine(ShootAfterRotation());
                shotRequested = true;
            };
        }
    }


    /**
    This will tilt the sniper cube to face the player, and call Shoot() on the sniper;
    */
    private void AimAtPlayer()
    {
        Vector3 playerDirection = playerShootPosition - transform.position;
        float turretRotationStep = turretRotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, playerDirection, turretRotationStep, 0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        if (Quaternion.Equals(newDirection, prevRotation))
        {
            Debug.Log("reached target");
            readyToShoot = true;
        }

        prevRotation = newDirection;
    }


    /**
    This ensures that the shot won't be fired until the turret has rotated to face where the player
    was when they missed a beat.  
    We don't track continuously because the sniper shouldn't know where you are if you can stay on beat.
    */
    IEnumerator ShootAfterRotation()
    {
        yield return new WaitUntil(() => readyToShoot);
        sniper.Shoot();
        shouldShoot = false;
        readyToShoot = false;
        shotRequested = false;

    }
}
