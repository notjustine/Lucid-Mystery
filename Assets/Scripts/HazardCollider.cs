using UnityEngine;

public class HazardCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Tile")
        {
            HazardAttack hazardAttack = FindObjectOfType<HazardAttack>();
            if (hazardAttack != null)
            {
                hazardAttack.OnHazardLanded(gameObject, collision.gameObject.name);
                Destroy(gameObject);
            }
        }
    }
}

