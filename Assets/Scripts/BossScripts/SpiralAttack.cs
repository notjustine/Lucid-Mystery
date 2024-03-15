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
    const float warningTime = 0.5f;
    ShootSpiralBullet[] turrets;
    int currTargetIndex;
    const int NUM_SLICES = 24;
    private WarningManager warningManager;

    void Start()
    {
        turretsRig = GameObject.Find("turretsRotate");
        targetsParent = GameObject.Find("SpiralTargets");
        currTargetIndex = 0;
        turrets = FindObjectsOfType<ShootSpiralBullet>();
        // Flash warning on turrets themselves when attacking
        warningManager = WarningManager.Instance;
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
        Shoot(turrets);
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
        Shoot(turrets);
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
        Shoot(turrets);
    }

    private void Shoot(ShootSpiralBullet[] turrets)
    {
        foreach (ShootSpiralBullet turret in turrets)
        {
            turret.Shoot();
        }
        turrets[0].PlaySound();
    }

    public List<string> GetWarningObjects()
    {
        return new List<string> {
            "turret1",
            "turret2",
            "turret3",
            "turret4",
            "turret5",
            "turret6",
            "turret7",
            "turret8",
        };
    }
}
