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
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceFired += Time.deltaTime;
        if (timeSinceFired > 6)
        {
            foreach (ShootSprayBullet turret in turrets)
            {
                StartCoroutine(TripleShootAndRotate(turret));
                timeSinceFired = 0;
            }
        }
    }

    IEnumerator TripleShootAndRotate(ShootSprayBullet turret)
    {
        // After rotation is fixed, use a coroutine that waits for the 15 degree rotation to complete before firing again.
        turret.Shoot();
        yield return new WaitForSeconds(1);
        turret.Shoot();
        yield return new WaitForSeconds(1);
        turret.Shoot();
    }
}
