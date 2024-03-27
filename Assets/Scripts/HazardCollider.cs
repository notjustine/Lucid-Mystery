using UnityEngine;
using System.Collections;

public class HazardCollider : MonoBehaviour
{
    public float timeUntilDestruction = 10f;

    private void Start()
    {
        StartCoroutine(DestroyIfNotCollided());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Tile")
        {
            HazardAttack hazardAttack = FindObjectOfType<HazardAttack>();
            if (hazardAttack != null)
            {
                hazardAttack.OnHazardLanded(gameObject, collision.gameObject.name);
            }
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyIfNotCollided()
    {
        yield return new WaitForSeconds(timeUntilDestruction);
        Destroy(gameObject);
    }
}
