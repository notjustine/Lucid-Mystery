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

    // This should be a comprehensive list of all stats we want to manage with our difficulty.
    public enum StatName
    {
        STEAM_DAMAGE,
        PLAYER_DAMAGE,
        SPRAY_BULLET_SPEED
    }
    
    // Communication list: Below is a list of classes that the DifficultyManager will push out updates to, if the player changes the difficulty
    SteamAttack steam;
    // End of list 


    [SerializeField] Difficulty currDifficulty;
    // difficultyMap maps a tuple representing a combination of StatName and Difficulty (ie: Difficulty.HARD) to a float for that particular difficulty setting.
    private Dictionary<(StatName, Difficulty), float> difficultyMap;
    private bool hasChanged;  // TEMPORARY, while we allow adjustment on the fly with keyboard input.


    void Start()
    {   
        difficultyMap = new Dictionary<(StatName, Difficulty), float>();  
        setUpDifficultyMap();
        currDifficulty = Difficulty.MEDIUM;
        hasChanged = false;

        // list of object the manager might have to notify:
        steam = FindObjectOfType<SteamAttack>();
    }

    // Update is called once per frame
    void Update()
    {   
        // TEMPORARY UNTIL WE ADD SOME UI CHANGES WHERE THEY CAN SELECT A DIFFICULTY
        if (Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("setting easy");
            setDifficulty(Difficulty.EASY);
        } else if (Input.GetKeyDown(KeyCode.M)) {
            setDifficulty(Difficulty.MEDIUM);
        } else if (Input.GetKeyDown(KeyCode.H)) {
            setDifficulty(Difficulty.HARD);
        }
        

        if (hasChanged)
        {
            setValuesForDifficulty();
            hasChanged = false;
        }
    }

    public void setDifficulty(Difficulty difficulty) {
        currDifficulty = difficulty;
        hasChanged = true;
    }

    // This is the function that needs to be updated whenever a new stat has been added that should be modifiable by our difficulty.
    private void setUpDifficultyMap() {
        difficultyMap[(StatName.STEAM_DAMAGE, Difficulty.EASY)] = 5f;
        difficultyMap[(StatName.STEAM_DAMAGE, Difficulty.MEDIUM)] = 10f;
        difficultyMap[(StatName.STEAM_DAMAGE, Difficulty.HARD)] = 15f;




    }

    public float getValue(StatName stat)
    {
        return difficultyMap[(stat, currDifficulty)];
    }

    private void setValuesForDifficulty()
    {
        steam.setSteamDamage(difficultyMap[(StatName.STEAM_DAMAGE, currDifficulty)]);
    }
}
