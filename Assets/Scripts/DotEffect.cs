using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotEffect : MonoBehaviour
{
    private bool isUnderEffect = false;
    private float effectDuration = 5f; // Duration of the effect in seconds
    private float damageInterval = 0.5f; // How often damage is applied
    private float damageAmount = 2f; // The amount of damage applied each interval

    public void ApplyEffect()
    {
        if (!isUnderEffect)
        {
            Debug.Log("try applying effect");
            StartCoroutine(ApplyDamageOverTime(effectDuration, damageInterval, damageAmount));
        }
    }

    IEnumerator ApplyDamageOverTime(float duration, float interval, float damage)
    {
        isUnderEffect = true;
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            GetComponent<PlayerStatus>().TakeDamage(damage);
            yield return new WaitForSeconds(interval);
        }

        isUnderEffect = false;
    }
}