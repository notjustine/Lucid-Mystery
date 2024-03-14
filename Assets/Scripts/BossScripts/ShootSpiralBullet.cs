using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;

public class ShootSpiralBullet : MonoBehaviour
{
    [SerializeField] GameObject bullet;

    /** 
    This is only responsible for spawning the bullet and orienting it, but not making it move or destroying.
    */
    private GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    public void Shoot()
    {
        Instantiate(bullet, transform.position, transform.rotation);
    }

    public void PlaySound()
    {
        // Temp solution. Idk why positional audio is not working right
        AudioManager.instance.PlayOneShotAttached(SoundRef.Instance.turrentShot, player);
    }
}
