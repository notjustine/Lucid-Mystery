using UnityEngine;
public class HealingItem : MonoBehaviour
{
    public float healAmount = 5f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStatus>().Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
