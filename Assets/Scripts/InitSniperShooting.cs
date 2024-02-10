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
    private bool aiming;
    private bool readyToShoot;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        sniper = FindObjectOfType<ShootSniperBullet>();

        aiming = false;
        readyToShoot = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            aiming = true;
            playerShootPosition = player.transform.position;  // The location that the player was when they whiffed, not current.
            StartCoroutine(ShootAfterRotation());
        }
        if (aiming)
        {
            AimAtPlayer();  // Frame by frame rotates towards where the player was.
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

        // Once we are aiming at the correct location, the Fire() method should run.
        if (newDirection == prevRotation)
        {
            aiming = false;
            readyToShoot = true;  // this allows our coroutine to start executing.
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
        readyToShoot = false;
    }
}
