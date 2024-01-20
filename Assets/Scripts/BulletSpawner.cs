using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    private GameObject _player;
    private GameObject _boss;
    private GameObject _bullet;

    private Transform _playerTransform;
    private Transform _bossTransform;
    private Transform _bulletTransform;
    private Rigidbody _bulletBody;
    private Vector3 _bulletDirection;
    private bool _hasShot;

    // should have a public class that generate a "fired" event

    // // Start is called before the first frame update
    void Start()
    {
        // get position of player
        _player = GameObject.FindWithTag("Player");
        _boss = GameObject.FindWithTag("Boss");

        _playerTransform = _player.GetComponent<Transform>();
        // get position of boss
        _bossTransform = _boss.GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {

        // capture space input
        _hasShot = Input.GetButton("Jump");   // maps to spacebar by default

    }

    void FixedUpdate()
    {

        if (_hasShot)
        {
            // apply player position to the bullet
            // _bulletDirection = _player.transform.position - _bossTransform.position;
            _bulletDirection = Vector3.MoveTowards(_playerTransform.position, _bossTransform.position, 5f);

            Quaternion bulletRotation = Quaternion.LookRotation(_bulletDirection);
            _bullet = GameObject.Instantiate(Resources.Load("Bullet"),
         _playerTransform.position, bulletRotation) as GameObject;

            // apply a velocity to bullet in direction of boss
            _bulletBody = _bullet.GetComponent<Rigidbody>();
            _bulletBody.AddForce(_bulletDirection * 5, ForceMode.VelocityChange);
            _hasShot = false;
        }

    }
}
