using UnityEngine;
using System.Collections;
using UnityEngine.VFX;

public class HazardCollider : MonoBehaviour
{
    public GameObject dustEffectPrefab;
    public float timeUntilDestruction = 10f;

    private void Start()
    {
        StartCoroutine(DestroyIfNotCollided());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Tile")
        {
            if (dustEffectPrefab != null)
            {
                StartCoroutine(PlayDustEffect());
            }

            HazardAttack hazardAttack = FindObjectOfType<HazardAttack>();
            if (hazardAttack != null)
            {
                hazardAttack.OnHazardLanded(gameObject, collision.gameObject, collision.gameObject.name);
            }
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyIfNotCollided()
    {
        yield return new WaitForSeconds(timeUntilDestruction);
        Destroy(gameObject);
    }

    private IEnumerator PlayDustEffect()
    {
        GameObject dustEffect = Instantiate(dustEffectPrefab, transform.position, Quaternion.identity);
        VisualEffect vfxComponent = dustEffect.GetComponent<VisualEffect>();
        if (vfxComponent != null)
        {
            vfxComponent.Play();
            yield return new WaitForSeconds(1f);
            vfxComponent.Stop();
            yield return new WaitForSeconds(5f);
            Destroy(dustEffect);
        }
    }
}
