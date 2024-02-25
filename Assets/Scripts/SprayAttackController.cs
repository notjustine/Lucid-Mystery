using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SprayAttackController : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject turretsRig;
    float timeSinceFired;
    ShootSprayBullet[] turrets;

    void Start()
    {
        // Grab the 'turrets' empty object
        turretsRig = GameObject.Find("turrets");
        // Will eventually rotate this thing.  Currently the pivot is way off.  Do shooting part for now.
        timeSinceFired = 0f;
        turrets = FindObjectsOfType<ShootSprayBullet>();
        for (int i = 0; i < turrets.Length; i++)
        {

            Debug.Log("turret, ");
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceFired += Time.deltaTime;
        if (timeSinceFired > 2)
        {
            foreach (ShootSprayBullet turret in turrets)
            {
                turret.Shoot();
                timeSinceFired = 0;
            }
        }
    }
}
