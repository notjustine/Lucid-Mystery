using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SprayAttackController : MonoBehaviour
{
    GameObject turretsRig;
    GameObject targetsParent;
    float timeSinceFired;
    const float turretRotationSpeed = 20f;
    ShootSprayBullet[] turrets;
    int currTargetIndex;
    bool hasNotFired;
    const int NUM_SLICES = 24;

    void Start()
    {
        turretsRig = GameObject.Find("turretsRotate");
        targetsParent = GameObject.Find("SpiralTargets");
        currTargetIndex = 0;
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

    /**
   Can be called by some AI controller to trigger the Shoot and Rotate attack.
   Note:  intentionally not in use while we are using timer above.
   */
    public void TriggerShootAndRotate()
    {
        StartCoroutine(TripleShootAndRotate(turrets));
    }


    IEnumerator TripleShootAndRotate(ShootSprayBullet[] turrets)
    {
        // First round of shots.
        foreach (ShootSprayBullet turret in turrets)
        {
            turret.Shoot();  // assume shoots at first index of 0 
        }
        turrets[0].PlaySound();
        // increment the target index because we already shot at above.
        currTargetIndex = (currTargetIndex + 1) % NUM_SLICES;
        // Determine position of child to shoot at.
        Vector3 directionToTarget1 = targetsParent.transform.GetChild(currTargetIndex).position - turretsRig.transform.position;
        Quaternion firstTargetRotation = Quaternion.LookRotation(directionToTarget1, Vector3.up);
        turretsRig.transform.rotation = Quaternion.RotateTowards(turretsRig.transform.rotation, firstTargetRotation, turretRotationSpeed * Time.deltaTime);

        while (turretsRig.transform.rotation != firstTargetRotation)
        {
            turretsRig.transform.rotation = Quaternion.RotateTowards(turretsRig.transform.rotation, firstTargetRotation, turretRotationSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);  // adds some padding between shoot and rotations
        // Second round of shots.
        foreach (ShootSprayBullet turret in turrets)
        {
            turret.Shoot();
        }
        turrets[0].PlaySound();
        // increment the target index
        currTargetIndex = (currTargetIndex + 1) % NUM_SLICES;
        // Determine position of child to shoot at
        Vector3 directionToTarget2 = targetsParent.transform.GetChild(currTargetIndex).position - turretsRig.transform.position;
        Quaternion secondTargetRotation = Quaternion.LookRotation(directionToTarget2, Vector3.up);
        turretsRig.transform.rotation = Quaternion.RotateTowards(turretsRig.transform.rotation, secondTargetRotation, turretRotationSpeed * Time.deltaTime);

        while (turretsRig.transform.rotation != secondTargetRotation)
        {
            turretsRig.transform.rotation = Quaternion.RotateTowards(turretsRig.transform.rotation, secondTargetRotation, turretRotationSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // Third and final round of shots until attack is triggered again.
        foreach (ShootSprayBullet turret in turrets)
        {
            turret.Shoot();
        }
        turrets[0].PlaySound();
    }
}
