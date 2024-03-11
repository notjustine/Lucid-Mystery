using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/** 
    WHEN EDITING THIS CLASS you should:
    1. add a new entry in the StatName enum
    2. add a new entry to the 'Communication list', if the relevant class isn't already listed
    3. update setUpDifficultyMap to reflect the thing you added in step 1.
    4. update setValuesForDifficulty to reflect the thing you added in step 1/2.

    Class description:
    This class is responsible for being a container of various statistics for each of the 3 difficulty levels, allowing an initial fetch 
    of the statistics values, as well as pushing out updated values to the relevant classes when their values change.

    NOTE: I considered the alternative where communication only comes from the other classes inward, but this would mean that those 
    classes are re-fetching the values on every frame, and this is terrible for performance, when users won't change difficulty often.
*/
public class DifficultyManager : MonoBehaviour
{

    public enum Difficulty
    {
        EASY,
        MEDIUM,
        HARD
    }

    // This enum should be a comprehensive list of all stats we want to manage with our difficulty.
    public enum StatName
    {
        PLAYER_DAMAGE,
        SNIPER_DAMAGE,
        STEAM_DAMAGE,
        SLAM_DAMAGE,
        SNIPER_BULLET_SPEED,
        SPRAY_BULLET_SPEED
    }
    
    // Communication list: Below is a list of classes that the DifficultyManager will push out updates to, if the player changes the difficulty
    SteamAttack steam;
    Attack playerAttack;
    // End of list 

    [SerializeField] Difficulty currDifficulty;
    // difficultyMap maps a tuple representing a combination of StatName and Difficulty (ie: (StatName.PLAYER_DAMAGE, Difficulty.HARD) to a float for that particular difficulty setting.
    private Dictionary<(StatName, Difficulty), float> difficultyMap;
    private bool hasChanged;  // TEMPORARY, while we allow adjustment on the fly with keyboard input.


    void Start()
    {   
        currDifficulty = Difficulty.MEDIUM;
        difficultyMap = new Dictionary<(StatName, Difficulty), float>();  
        SetUpDifficultyMap();
        hasChanged = false;

        // list of objects that the manager will have to notify about changes:
        steam = FindObjectOfType<SteamAttack>();
        playerAttack = FindObjectOfType<Attack>();
    }


    void Update()
    {   
        // TEMPORARY UNTIL WE ADD SOME UI CHANGES WHERE THEY CAN SELECT A DIFFICULTY
        if (Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("setting easy");
            SetDifficulty(Difficulty.EASY);
        } else if (Input.GetKeyDown(KeyCode.M)) {
            SetDifficulty(Difficulty.MEDIUM);
            Debug.Log("setting medium");
        } else if (Input.GetKeyDown(KeyCode.H)) {
            SetDifficulty(Difficulty.HARD);
            Debug.Log("setting hard");
        }
        
        if (hasChanged)
        {
            SetValuesForDifficulty();
            hasChanged = false;
        }
    }

    /** 
        Setter that can be called via some main menu with buttons. 
    */
    public void SetDifficulty(Difficulty difficulty) {
        currDifficulty = difficulty;
        hasChanged = true;
    }


    /** 
        Initializes the map.  This is where we can play around with the actual difficulty values.
    */
    private void SetUpDifficultyMap() {
        // Damages of things
        difficultyMap[(StatName.PLAYER_DAMAGE, Difficulty.EASY)] = 70f;
        difficultyMap[(StatName.PLAYER_DAMAGE, Difficulty.MEDIUM)] = 50f;
        difficultyMap[(StatName.PLAYER_DAMAGE, Difficulty.HARD)] = 35f;

        difficultyMap[(StatName.SNIPER_DAMAGE, Difficulty.EASY)] = 3f;
        difficultyMap[(StatName.SNIPER_DAMAGE, Difficulty.MEDIUM)] = 5f;
        difficultyMap[(StatName.SNIPER_DAMAGE, Difficulty.HARD)] = 7f;

        difficultyMap[(StatName.STEAM_DAMAGE, Difficulty.EASY)] = 5f;
        difficultyMap[(StatName.STEAM_DAMAGE, Difficulty.MEDIUM)] = 10f;
        difficultyMap[(StatName.STEAM_DAMAGE, Difficulty.HARD)] = 15f;

        difficultyMap[(StatName.SLAM_DAMAGE, Difficulty.EASY)] = 3f;
        difficultyMap[(StatName.SLAM_DAMAGE, Difficulty.MEDIUM)] = 5f;
        difficultyMap[(StatName.SLAM_DAMAGE, Difficulty.HARD)] = 7f;

        // Speeds of things
        difficultyMap[(StatName.SNIPER_BULLET_SPEED, Difficulty.EASY)] = 50f;
        difficultyMap[(StatName.SNIPER_BULLET_SPEED, Difficulty.MEDIUM)] = 70f;
        difficultyMap[(StatName.SNIPER_BULLET_SPEED, Difficulty.HARD)] = 140f;
    }


    /** 
        Accesses the difficultyMap to get the current stat value for a given stat and difficulty combo.  

        NOTE:  CAN be used on the fly, as with the SlamAttack, Sniper stats.
    */
    public float GetValue(StatName stat)
    {
        return difficultyMap[(stat, currDifficulty)];
    }


    /**
        We can update the values for controller that remain in the scene for

        NOTE:  Not included are:  sniper bullet stats, slam attacks, because these spawn on the fly with updated values.
    */
    private void SetValuesForDifficulty()
    {
        playerAttack.SetPlayerDamage(difficultyMap[(StatName.PLAYER_DAMAGE, currDifficulty)]);
        steam.SetSteamDamage(difficultyMap[(StatName.STEAM_DAMAGE, currDifficulty)]);
    }
}
