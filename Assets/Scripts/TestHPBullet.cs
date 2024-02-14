using UnityEngine;
public class TestHPBullet : MonoBehaviour
{
    public float damage = 20f;

    private void Start()
    {
        Destroy(gameObject, 10f);
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