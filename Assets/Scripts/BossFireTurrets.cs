using UnityEngine;

/**
The purpose of this class is to capture X number of beats, grab the list of boss guns, and use modulus to sort
out which set of guns should fire on each beat.  It will use the BossGun class's "FireTurret" method to instantiate
bullets at the edge of the gun, pointing in the correct direction.
*/
public class BossFireTurrets : MonoBehaviour
{
    private GameObject bossGun;

    public GameObject bulletPrefab; // Assign this in the Inspector


    private Transform bossTransform;
    private int turretNumber = 0;

    // void Start()
    // {
    //     // boss = GameObject.FindWithTag("Boss");

    //     // bossTransform = boss.transform;
    // }

    // void Update()
    // {
    //     if (Input.GetButtonDown("Jump")) // Use GetButtonDown for a single event
    //     {
    //         ShootBullet();
    //     }
    // }

    // private void ShootBullet()
    // {
    //     Vector3 direction = (_bossTransform.position - _playerTransform.position).normalized;
    //     Quaternion bulletRotation = Quaternion.LookRotation(direction);

    //     GameObject bullet = Instantiate(bulletPrefab, _playerTransform.position, bulletRotation);

    //     Rigidbody bulletBody = bullet.GetComponent<Rigidbody>();
    //     bulletBody.AddForce(-(direction * 5), ForceMode.VelocityChange);
    // }
}
