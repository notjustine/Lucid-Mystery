using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    private GameObject _player;
    private GameObject _boss;
    private GameObject _bullet;
    // private const float deathHeight = -20f;
    private Transform _playerTransform;
    private Transform _bossTransform;
    private Transform _bulletTransform;
    private Rigidbody _playerRB;
    private Vector3 _startPos = new Vector3(3.89f, 2.06f, -15.21f);
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
