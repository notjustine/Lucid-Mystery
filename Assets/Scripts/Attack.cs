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
        // Debug.Log("Collision");
        if (collision.gameObject.tag == "Boss")
        {
            Debug.Log("Enemy Hit");
            // Destroy(collision.gameObject);
        }
    }
}
