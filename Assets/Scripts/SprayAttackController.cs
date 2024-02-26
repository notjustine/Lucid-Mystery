using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SprayAttackController : MonoBehaviour
{
    GameObject turretsRig;
    float timeSinceFired;
    const float turretRotationSpeed = 20f;
    ShootSprayBullet[] turrets;
    bool hasNotFired;


    void Start()
    {
        turretsRig = GameObject.Find("turrets");
        hasNotFired = true;
        timeSinceFired = 0f;
        turrets = FindObjectsOfType<ShootSprayBullet>();
    }


    void Update()
    {

        // Trigger the firing of turrets on a timer for now.
        timeSinceFired += Time.deltaTime;
        if (hasNotFired || timeSinceFired > 6)
        {
            StartCoroutine(TripleShootAndRotate(turrets));
            hasNotFired = false;
            timeSinceFired = 0;
        }
    }

    IEnumerator TripleShootAndRotate(ShootSprayBullet[] turrets)
    {
        // First round of shots.
        foreach (ShootSprayBullet turret in turrets)
        {
            turret.Shoot();
        }

        Quaternion currentRotation = turretsRig.transform.rotation;
        Quaternion rotationToAdd = Quaternion.Euler(0f, -15f, 0f);
        Quaternion targetRotation = currentRotation * rotationToAdd;

        while (turretsRig.transform.rotation != targetRotation)
        {
            turretsRig.transform.rotation = Quaternion.RotateTowards(turretsRig.transform.rotation, targetRotation, turretRotationSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);  // adds some padding between shoot and rotations

        // Second round of shots.
        foreach (ShootSprayBullet turret in turrets)
        {
            turret.Shoot();
        }

        currentRotation = turretsRig.transform.rotation;
        targetRotation = currentRotation * rotationToAdd;
        while (turretsRig.transform.rotation != targetRotation)
        {
            turretsRig.transform.rotation = Quaternion.RotateTowards(turretsRig.transform.rotation, targetRotation, turretRotationSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f); // adds some padding between shoot and rotations

        // Third and final round of shots until attack is triggered again.
        foreach (ShootSprayBullet turret in turrets)
        {
            turret.Shoot();
        }
    }
}