using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float spawnDistance = 1.0f; // Distance in front of the player to spawn the bullet

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Replace with your input
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 spawnPos = bulletSpawnPoint.position + bulletSpawnPoint.forward * spawnDistance;
        Instantiate(bulletPrefab, spawnPos, bulletSpawnPoint.rotation);
    }
}

