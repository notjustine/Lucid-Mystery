using UnityEngine;

public class NailCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                float damage = DifficultyManager.Instance.GetValue(DifficultyManager.StatName.HAZARD_DAMAGE);
                playerStatus.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}