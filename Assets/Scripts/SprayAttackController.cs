using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SprayAttackController : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject turretsRig;
    float timeSinceFired;
    const float turretRotationSpeed = 20f;
    // float turretRotationStep;
    ShootSprayBullet[] turrets;
    bool hasNotFired;

    void Start()
    {
        // Grab the 'turrets' empty object
        turretsRig = GameObject.Find("turrets");
        hasNotFired = true;
        timeSinceFired = 0f;
        turrets = FindObjectsOfType<ShootSprayBullet>();
    }

    // Update is called once per frame
    void Update()
    {
        // Test the rotation of the turretsRig
        // StartCoroutine(RotateToTargetCoroutine());


        // Trigger the firing of turrets
        timeSinceFired += Time.deltaTime;
        if (hasNotFired || timeSinceFired > 12)
        {
            // foreach (ShootSprayBullet turret in turrets)
            // {
            StartCoroutine(TripleShootAndRotate(turrets));
            hasNotFired = false;
            timeSinceFired = 0;
            // }
        }
    }

    IEnumerator TripleShootAndRotate(ShootSprayBullet[] turrets)
    {
        // After rotation is fixed, use a coroutine that waits for the 15 degree rotation to complete before firing again.
        foreach (ShootSprayBullet turret in turrets)
        {
            turret.Shoot();
        }

        Quaternion currentRotation = turretsRig.transform.rotation;
        Quaternion rotationToAdd = Quaternion.Euler(0f, -15f, 0f);
        Quaternion targetRotation = currentRotation * rotationToAdd;
        // Debug.Log($"old: {turretsRig.transform.rotation.y}");
        // targetRotation.eulerAngles = new Vector3(turretsRig.transform.rotation.x, turretsRig.transform.rotation.y + 15f, turretsRig.transform.rotation.z);
        // Debug.Log($"new: {targetRotation.y}");
        while (turretsRig.transform.rotation != targetRotation)
        {
            turretsRig.transform.rotation = Quaternion.RotateTowards(turretsRig.transform.rotation, targetRotation, turretRotationSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        foreach (ShootSprayBullet turret in turrets)
        {
            turret.Shoot();
        }
        // CODE WORKS UNTIL HERE.
        currentRotation = turretsRig.transform.rotation;
        targetRotation = currentRotation * rotationToAdd;
        while (turretsRig.transform.rotation != targetRotation)
        {
            turretsRig.transform.rotation = Quaternion.RotateTowards(turretsRig.transform.rotation, targetRotation, turretRotationSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        foreach (ShootSprayBullet turret in turrets)
        {
            turret.Shoot();
        }
    }
}
