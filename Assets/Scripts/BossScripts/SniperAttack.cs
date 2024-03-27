using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


/** 
This script can be attached to a blank object in the scene.
*/
public class SniperAttack : MonoBehaviour, IWarningGenerator
{
    [SerializeField] private BeatCheckController beatChecker;
    private ShootSniperBullet sniper;
    private TutorialManager tutorialManager;
    private GameObject player;
    private PlayerControl playerControl;
    private Vector3 playerShootPosition;
    private Vector3 prevRotation;
    private const float turretRotationSpeed = 5f;
    private bool aiming;
    private bool readyToShoot;

    WarningManager warningManager;
    List<string> warnings; // so that we can undo them based on a bullet resolution


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerControl = player.GetComponent<PlayerControl>();
        sniper = FindObjectOfType<ShootSniperBullet>();
        beatChecker = FindObjectOfType<BeatCheckController>();
        warningManager = WarningManager.Instance;
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            tutorialManager = FindObjectOfType<TutorialManager>();
        }
        aiming = false;
        readyToShoot = false;
    }


    void Update()
    {
        if (aiming)
        {
            AimAtPlayer();
        }
    }


    /**
    Trigger to start running these functions, only runs when the player makes an input mistake.
    */
    public void TriggerAttack()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial" && (tutorialManager.currentState == TutorialState.Start || tutorialManager.currentState == TutorialState.OnBeat))
        {
            return;
        }else if (beatChecker.GetVulnerable())
        {
            AudioManager.instance.PlayOneShotAttached(SoundRef.Instance.missBeatSniperShot, player);
            beatChecker.SetVulnerable(false);
            aiming = true;
            playerShootPosition = player.transform.position;  // The location that the player WAS when they missed a beat, not current.
            // Show a warning based on current location of player
            warningManager.ToggleWarning(GetWarningObjects(), true, WarningManager.WarningType.SNIPER);

            StartCoroutine(ShootAfterRotation());
        }
    }


    /**
        Determines the name of the physical tile the player is currently on, adds to list and returns.
    */
    public List<string> GetWarningObjects()
    {
        Dictionary<(int, int), string> mapping = warningManager.GetLogicalToPhysicalTileMapping();
        return new List<string> { mapping[(playerControl.currentRingIndex, playerControl.currentTileIndex)] }; ;
    }


    /**
    This will tilt the sniper cube to face the player, and call Shoot() on the sniper;
    */
    private void AimAtPlayer()
    {
        Vector3 playerDirection = playerShootPosition - transform.position;
        float turretRotationStep = turretRotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, playerDirection, turretRotationStep, 0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        // Once we are aiming at the correct location, the Fire() method should run.
        if (newDirection == prevRotation)
        {
            aiming = false;
            readyToShoot = true;  // this allows our coroutine to start executing.
        }

        prevRotation = newDirection;
    }


    /**
    This ensures that the shot won't be fired until the turret has rotated to face where the player
    was when they missed a beat.  
    We don't track continuously because the sniper shouldn't know where you are if you can stay on beat.
    */
    IEnumerator ShootAfterRotation()
    {
        yield return new WaitUntil(() => readyToShoot);
        sniper.Shoot();
        readyToShoot = false;
    }
}
