using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SpiralAttack : MonoBehaviour
{
    GameObject turretsRig;
    GameObject targetsParent;
    const float turretRotationSpeed = 20f;
    ShootSpiralBullet[] turrets;
    int currTargetIndex;
    const int NUM_SLICES = 24;

    void Start()
    {
        turretsRig = GameObject.Find("turretsRotate");
        targetsParent = GameObject.Find("SpiralTargets");
        currTargetIndex = 0;
        turrets = FindObjectsOfType<ShootSpiralBullet>();
    }


    /**
        Begins the turret rotate-and-shoot attack.
    */
    public void TriggerAttack()
    {
        StartCoroutine(TripleShootAndRotate(turrets));
    }


    IEnumerator TripleShootAndRotate(ShootSpiralBullet[] turrets)
    {
        // First round of shots.
        foreach (ShootSpiralBullet turret in turrets)
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
        foreach (ShootSpiralBullet turret in turrets)
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
        foreach (ShootSpiralBullet turret in turrets)
        {
            turret.Shoot();
        }
        turrets[0].PlaySound();
    }
}
