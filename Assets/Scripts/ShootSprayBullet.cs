using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSprayBullet : MonoBehaviour
{
    [SerializeField] GameObject bullet;

    /** 
    This is only responsible for spawning the bullet and orienting it, but not making it move or destroying.
    */
    public void Shoot()
    {
        Instantiate(bullet, transform.position, transform.rotation);
    }
}
