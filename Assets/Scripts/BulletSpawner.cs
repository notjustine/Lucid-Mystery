using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    private GameObject _player;
    private GameObject _boss;

    public GameObject bulletPrefab; // Assign this in the Inspector

    private Transform _playerTransform;
    private Transform _bossTransform;

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _boss = GameObject.FindWithTag("Boss");

        _playerTransform = _player.transform;
        _bossTransform = _boss.transform;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump")) // Use GetButtonDown for a single event
        {
            ShootBullet();
        }
    }

    private void ShootBullet()
    {
        Vector3 direction = (_bossTransform.position - _playerTransform.position).normalized;
        Quaternion bulletRotation = Quaternion.LookRotation(direction);

        GameObject bullet = Instantiate(bulletPrefab, _playerTransform.position, bulletRotation);

        Rigidbody bulletBody = bullet.GetComponent<Rigidbody>();
        bulletBody.AddForce(-(direction * 5), ForceMode.VelocityChange);
    }
}
