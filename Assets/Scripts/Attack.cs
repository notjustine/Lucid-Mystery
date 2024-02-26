using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            // Debug.Log("Collision");
            BossHealth bossHealth = collision.gameObject.GetComponent<BossHealth>();
            bossHealth.TakeDamage(20f);
            AudioManager.instance.PlayOneShotAttached(SoundRef.Instance.attackSound, gameObject);
            Debug.Log("Enemy Hit");

            // Destroy(collision.gameObject);
        }
    }
}
