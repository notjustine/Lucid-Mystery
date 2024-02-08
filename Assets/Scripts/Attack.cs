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
            AudioManager.instance.PlayOneShot(SoundRef.Instance.attackSound, gameObject.transform.position);
            Debug.Log("Enemy Hit");
            // Destroy(collision.gameObject);
        }
    }
}
