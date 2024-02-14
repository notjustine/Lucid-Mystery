using UnityEngine;

public class DangerTile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit player");
            other.GetComponent<DotEffect>().ApplyEffect();
        }
    }
}