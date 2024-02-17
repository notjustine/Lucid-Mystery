using UnityEngine;
public class TestHPBullet : MonoBehaviour
{
    public float damage = 20f;
    public float speed = 20f;

    private void Start()
    {
        //Vector3 direction = (Vector3.zero - transform.position).normalized;
        //GetComponent<Rigidbody>().velocity = direction * speed; //speed
        Destroy(gameObject, 10f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is not the player
        if (collision.gameObject.tag != "Player" | collision.gameObject.tag != "Enviorment")
        {
            Destroy(gameObject); // Destroy bullet on collision
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStatus>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}