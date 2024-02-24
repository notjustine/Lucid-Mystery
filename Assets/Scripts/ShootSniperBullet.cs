using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 Attached to an empty object that is a child of the given gun/turret that's firing it.
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
