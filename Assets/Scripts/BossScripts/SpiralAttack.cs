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
    private Dictionary<int, List<string>> turretGroups;

    void Start()
    {
        turretsRig = GameObject.Find("turretsRotate");
        targetsParent = GameObject.Find("SpiralTargets");
        currTargetIndex = 0;
        turrets = FindObjectsOfType<ShootSpiralBullet>();
        warningManager = WarningManager.Instance;
        turretGroups = new Dictionary<int, List<string>>();
        InitTurretGroups();
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
        // Offer warning blinks
        List<string> warned = warningManager.ToggleWarning(GetWarningObjects(), true, WarningManager.WarningType.SPIRAL);
        yield return new WaitForSeconds(warningTime);  // adds some padding between shoot and rotations
        // First round of shots.
        warningManager.ToggleWarning(warned, false, WarningManager.WarningType.SPIRAL);
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
        warned = warningManager.ToggleWarning(GetWarningObjects(), true, WarningManager.WarningType.SPIRAL);
        yield return new WaitForSeconds(warningTime);  // adds some padding between shoot and rotations
        warningManager.ToggleWarning(warned, false, WarningManager.WarningType.SPIRAL);
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
        warned = warningManager.ToggleWarning(GetWarningObjects(), true, WarningManager.WarningType.SPIRAL);
        yield return new WaitForSeconds(warningTime);
        warningManager.ToggleWarning(warned, false, WarningManager.WarningType.SPIRAL);

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
        List<string> warnings = new List<string>
        {
            "turret1",
            "turret2",
            "turret3",
            "turret4",
            "turret5",
            "turret6",
            "turret7",
            "turret8",
        };
        warnings.AddRange(turretGroups[currTargetIndex % 3]);
        return warnings;
    }


    private void InitTurretGroups()
    {
        turretGroups[0] = new List<string>
        {
            $"R1_18", $"R2_18", $"R3_18", $"R4_18",
            $"R1_15", $"R2_15", $"R3_15", $"R4_15",
            $"R1_12", $"R2_12", $"R3_12", $"R4_12",
            $"R1_09", $"R2_09", $"R3_09", $"R4_09",
            $"R1_06", $"R2_06", $"R3_06", $"R4_06",
            $"R1_03", $"R2_03", $"R3_03", $"R4_03",
            $"R1_24", $"R2_24", $"R3_24", $"R4_24",
            $"R1_21", $"R2_21", $"R3_21", $"R4_21",
        };

        turretGroups[1] = new List<string>
        {
            $"R1_17", $"R2_17", $"R3_17", $"R4_17",
            $"R1_14", $"R2_14", $"R3_14", $"R4_14",
            $"R1_11", $"R2_11", $"R3_11", $"R4_11",
            $"R1_08", $"R2_08", $"R3_08", $"R4_08",
            $"R1_05", $"R2_05", $"R3_05", $"R4_05",
            $"R1_02", $"R2_02", $"R3_02", $"R4_02",
            $"R1_23", $"R2_23", $"R3_23", $"R4_23",
            $"R1_20", $"R2_20", $"R3_20", $"R4_20",
        };

        turretGroups[2] = new List<string>
        {
            $"R1_16", $"R2_16", $"R3_16", $"R4_16",
            $"R1_13", $"R2_13", $"R3_13", $"R4_13",
            $"R1_10", $"R2_10", $"R3_10", $"R4_10",
            $"R1_07", $"R2_07", $"R3_07", $"R4_07",
            $"R1_04", $"R2_04", $"R3_04", $"R4_04",
            $"R1_01", $"R2_01", $"R3_01", $"R4_01",
            $"R1_22", $"R2_22", $"R3_22", $"R4_22",
            $"R1_19", $"R2_19", $"R3_19", $"R4_19",
        };
    }
}
