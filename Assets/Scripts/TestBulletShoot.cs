using UnityEngine;

public class Shooter : MonoBehaviour
{
    public TestHPBullet bulletPrefab;
    public float shootingRate = 2f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > shootingRate)
        {
            ShootBullet();
            timer = 0;
        }
    }

    void ShootBullet()
    {
        // Instantiate bullet and shoot it in a direction (for simplicity, directly forward)
        TestHPBullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * 10; // Adjust speed as necessary
    }
}