using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Dylan: working on shooting sniper bullets from a tower. 
Phase 2 (current):  When spacerbar is clicked, instantiate a bullet
Phase 3 (next): the bullet should be fired when a missed beat is detected

 */
public class ShootSniperBullet : MonoBehaviour
{
    [SerializeField] GameObject bullet;

    /** 
    This is only responsible for spawning the bullet, but not making it move or destroying.
    */
    public void Shoot()
    {
        Instantiate(bullet, transform.position, Quaternion.identity);
    }
}
