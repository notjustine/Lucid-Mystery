using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/** 
    WHEN EDITING THIS CLASS you should:
    1. add a new entry in the StatName enum
    2. add a new entry to the 'Communication list', if the relevant class isn't already listed and the thing remains in the scene permanently
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
    public static int phase = 0;
    [Serializable]
    public enum Difficulty
    {
        EASY = 0,
        MEDIUM = 1,
        HARD = 2
    }

    // This enum should be a comprehensive list of all stats we want to manage with our difficulty.
    public enum StatName
    {
        PLAYER_DAMAGE,
        SNIPER_DAMAGE,
        STEAM_DAMAGE,
        STEAM_TIMING,
        SLAM_DAMAGE,
        SLAM_TIMING,
        SPIRAL_BULLET_DAMAGE,
        SNIPER_BULLET_SPEED,
        SPIRAL_BULLET_SPEED,
        COOLDOWN,
    }

    // Communication list: Below is a list of classes that the DifficultyManager will push out updates to, if the player changes the difficulty
    SteamAttack steam;
    Attack playerAttack;
    BossStates bossStates;
    // End of list 

    [SerializeField] Difficulty currDifficulty;
    // difficultyMap maps a tuple representing a combination of StatName and Difficulty (ie: (StatName.PLAYER_DAMAGE, Difficulty.HARD) to a float for that particular difficulty setting.
    private Dictionary<(StatName, Difficulty), float> difficultyMap;
    public bool hasChanged;

    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;

    public static DifficultyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Difficulty Manager");
        }
        Instance = this;
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("difficulty"))
        {
            currDifficulty = (Difficulty)PlayerPrefs.GetInt("difficulty");
        }
        else
        {
            currDifficulty = Difficulty.MEDIUM;
        }

        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            easyButton = GameObject.Find("Easy").GetComponent<Button>();
            mediumButton = GameObject.Find("Medium").GetComponent<Button>();
            hardButton = GameObject.Find("Hard").GetComponent<Button>();

            easyButton.onClick.AddListener(delegate { SetDifficulty(Difficulty.EASY); });
            mediumButton.onClick.AddListener(delegate { SetDifficulty(Difficulty.MEDIUM); });
            hardButton.onClick.AddListener(delegate { SetDifficulty(Difficulty.HARD); });
        }

        difficultyMap = new Dictionary<(StatName, Difficulty), float>();
        SetUpDifficultyMap();
        hasChanged = false;

        if (SceneManager.GetActiveScene().name == "PatentEnvironment")
        {
            SetValuesForDifficulty();
        }
    }


    public void SetDifficulty(Difficulty difficulty)
    {
        currDifficulty = difficulty;
        PlayerPrefs.SetInt("difficulty", (int)difficulty);
        hasChanged = true;
    }


    /** 
        Initializes the map.  This is where we can play around with the actual difficulty values.
    */
    private void SetUpDifficultyMap()
    {
        // Damages of things
        difficultyMap[(StatName.PLAYER_DAMAGE, Difficulty.EASY)] = 75f;
        difficultyMap[(StatName.PLAYER_DAMAGE, Difficulty.MEDIUM)] = 50f;
        difficultyMap[(StatName.PLAYER_DAMAGE, Difficulty.HARD)] = 35f;

        difficultyMap[(StatName.SNIPER_DAMAGE, Difficulty.EASY)] = 3f;
        difficultyMap[(StatName.SNIPER_DAMAGE, Difficulty.MEDIUM)] = 5f;
        difficultyMap[(StatName.SNIPER_DAMAGE, Difficulty.HARD)] = 7f;

        difficultyMap[(StatName.STEAM_DAMAGE, Difficulty.EASY)] = 3f;
        difficultyMap[(StatName.STEAM_DAMAGE, Difficulty.MEDIUM)] = 10f;
        difficultyMap[(StatName.STEAM_DAMAGE, Difficulty.HARD)] = 15f;

        difficultyMap[(StatName.SPIRAL_BULLET_DAMAGE, Difficulty.EASY)] = 3f;
        difficultyMap[(StatName.SPIRAL_BULLET_DAMAGE, Difficulty.MEDIUM)] = 5f;
        difficultyMap[(StatName.SPIRAL_BULLET_DAMAGE, Difficulty.HARD)] = 7f;

        difficultyMap[(StatName.SLAM_DAMAGE, Difficulty.EASY)] = 3f;
        difficultyMap[(StatName.SLAM_DAMAGE, Difficulty.MEDIUM)] = 5f;
        difficultyMap[(StatName.SLAM_DAMAGE, Difficulty.HARD)] = 7f;

        // Speeds of things
        difficultyMap[(StatName.SNIPER_BULLET_SPEED, Difficulty.EASY)] = 40f;
        difficultyMap[(StatName.SNIPER_BULLET_SPEED, Difficulty.MEDIUM)] = 70f;
        difficultyMap[(StatName.SNIPER_BULLET_SPEED, Difficulty.HARD)] = 140f;

        difficultyMap[(StatName.SPIRAL_BULLET_SPEED, Difficulty.EASY)] = 30f;
        difficultyMap[(StatName.SPIRAL_BULLET_SPEED, Difficulty.MEDIUM)] = 50f;
        difficultyMap[(StatName.SPIRAL_BULLET_SPEED, Difficulty.HARD)] = 75f;

        difficultyMap[(StatName.STEAM_TIMING, Difficulty.EASY)] = 4f;
        difficultyMap[(StatName.STEAM_TIMING, Difficulty.MEDIUM)] = 3f;
        difficultyMap[(StatName.STEAM_TIMING, Difficulty.HARD)] = 2.5f;

        difficultyMap[(StatName.SLAM_TIMING, Difficulty.EASY)] = 4f;
        difficultyMap[(StatName.SLAM_TIMING, Difficulty.MEDIUM)] = 3f;
        difficultyMap[(StatName.SLAM_TIMING, Difficulty.HARD)] = 2.5f;

        difficultyMap[(StatName.COOLDOWN, Difficulty.EASY)] = 7f;
        difficultyMap[(StatName.COOLDOWN, Difficulty.MEDIUM)] = 5f;
        difficultyMap[(StatName.COOLDOWN, Difficulty.HARD)] = 4f;
    }


    /** 
        Accesses the difficultyMap to get the current stat value for a given stat and difficulty combo.  

        NOTE:  CAN be used on the fly, as with the SlamAttack, Sniper, Spiral stats.
    */
    public float GetValue(StatName stat)
    {
        return difficultyMap[(stat, currDifficulty)];
    }


    /**
        We can update the values for controllers that remain in the scene for the full duration (the player, the boss, steam attack)
        Should be called AFTER the main scene finishes loading to set correct values.

        NOTE:  Not included are:  sniper bullet stats, slam attacks, because these spawn on the fly with updated values.
    */
    private void SetValuesForDifficulty()
    {
        // list of objects that the manager will have to notify about changes (if they are in the scene permanently, not spawners like spikes/bullets):
        steam = FindObjectOfType<SteamAttack>();
        playerAttack = FindObjectOfType<Attack>();
        bossStates = FindAnyObjectByType<BossStates>();

        playerAttack.SetPlayerDamage(difficultyMap[(StatName.PLAYER_DAMAGE, currDifficulty)]);
        steam.SetSteamDamage(difficultyMap[(StatName.STEAM_DAMAGE, currDifficulty)]);
        bossStates.SetCooldown(difficultyMap[(StatName.COOLDOWN, currDifficulty)]);
    }
}
