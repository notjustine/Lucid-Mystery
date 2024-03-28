using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealingManager : MonoBehaviour
{
    private Dictionary<(int, int), string> logicalToPhysicalTileMapping;
    private BossHealth bossHealth;
    private List<string> healingTiles;
    private List<string> tileOptions;

    // for blink effect
    [SerializeField] Color tileColor;
    [SerializeField] Color healingStart;
    [SerializeField] Color healingEnd;
    private const float TILE_BLINK_SPEED = 0.6f;
    private GameObject tempObject;
    private MeshRenderer tempRenderer;
    private MaterialPropertyBlock propBlock;
    // end blink effect stuff

    private float time;
    private WarningManager warningManager;
    private List<string> warningTiles;
    public static HealingManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Healing Manager");
        }
        Instance = this;
    }


    void Start()
    {
        time = 7f;
        bossHealth = FindObjectOfType<BossHealth>();
        warningManager = WarningManager.Instance;
        InitTileOptions();
        healingTiles = new List<string>();
        InitLogicToPhysMapping();
    }


    void Update()
    {
        // Determine if healing should be active at all
        if (DifficultyManager.phase == 2)
        {
            time += Time.deltaTime;
            if (time > 8f)
            {
                StartCoroutine(Heal());  // Healing last 6 seconds, so there will be 2 seconds between sets of healing.
                time = 0f;
            }
        }
        warningTiles = warningManager.GetWarnings();

        // Every name found in healingTiles should be blinking (unless it's also under warning).
        foreach (string name in healingTiles)
        {
            if (!warningTiles.Contains(name))
            {
                tempObject = GameObject.Find(name);
                tempRenderer = tempObject.GetComponent<MeshRenderer>();
                propBlock = new MaterialPropertyBlock();
                tempRenderer.GetPropertyBlock(propBlock);
                propBlock.SetColor("_BaseColor", Color.Lerp(healingStart, healingEnd, Mathf.PingPong(Time.time, TILE_BLINK_SPEED)));
                tempRenderer.SetPropertyBlock(propBlock);
            }
        }
    }


    /** 
        Player can use this to determine if they are current on a healing tile.
    */
    public bool IsHealing(int ringIndex, int tileIndex)
    {
        if (logicalToPhysicalTileMapping.ContainsKey((ringIndex, tileIndex)))
        {
            return healingTiles.Contains(logicalToPhysicalTileMapping[(ringIndex, tileIndex)]);
        }
        return false;
    }


    /**
        Changes tiles to healing or toggles them back to original accordingly.
    */
    void ToggleHealing(bool startHealing)
    {
        if (startHealing)
        {
            List<string> tiles = ChooseHealingTiles();
            for (int i = 0; i < 2; i++)
            {
                healingTiles.Add(tiles[i]); // So that Update function can make it blink
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                // Pop front of the healingTiles list in both cases
                GameObject tileToChange = GameObject.Find(healingTiles[0]);  // TO-DO: add error handling/null checks maybe
                MeshRenderer renderer = tileToChange.GetComponent<MeshRenderer>();

                string toRemove = healingTiles[0];
                healingTiles.Remove(toRemove);
                HandleToggleMaterial(toRemove, renderer);
            }
        }
    }


    /**
        Keep this tile as the healing color if there is still another outstanding warning.
    */
    private void HandleToggleMaterial(string name, MeshRenderer renderer)
    {
        if (!healingTiles.Contains(name))
        {
            propBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propBlock);
            propBlock.SetColor("_BaseColor", tileColor);
            renderer.SetPropertyBlock(propBlock);
        }
    }


    private List<string> ChooseHealingTiles()
    {
        // Get two random numbers that are different between 0-47
        System.Random random = new System.Random();
        int first = random.Next(48);
        int second = random.Next(48);
        while (first == second)
        {
            second = random.Next(48);
        }
        return new List<string> { tileOptions[first], tileOptions[second] };
    }


    /**
    This ensures that the shot won't be fired until the turret has rotated to face where the player
    was when they missed a beat.  
    We don't track continuously because the sniper shouldn't know where you are if you can stay on beat.
    */
    IEnumerator Heal()
    {
        // Switch healing on
        ToggleHealing(true);
        yield return new WaitForSeconds(6f);
        ToggleHealing(false);
    }


    private void InitTileOptions()
    {
        tileOptions = new List<string>
        {
            "R3_18", "R3_17", "R3_16", "R3_15", "R3_14", "R3_13", "R3_12", "R3_11",
            "R3_10", "R3_09", "R3_08", "R3_07", "R3_06", "R3_05", "R3_04", "R3_03",
            "R3_02", "R3_01", "R3_24", "R3_23", "R3_22", "R3_21", "R3_20", "R3_19",
            "R4_18", "R4_17", "R4_16", "R4_15", "R4_14", "R4_13", "R4_12", "R4_11",
            "R4_10", "R4_09", "R4_08", "R4_07", "R4_06", "R4_05", "R4_04", "R4_03",
            "R4_02", "R4_01", "R4_24", "R4_23", "R4_22", "R4_21", "R4_20", "R4_19",
        };
    }


    /**
        Initializes a mapping between logical locations used by PlayerControl to physical mapping in our actual scene.
        This will drastically improve performance of determining which tiles to highlight with a warning in various situations, 
        as opposed to on-the-fly calculation by iterating through tiles and calculating which tile the player is overlapping with.
    */
    private void InitLogicToPhysMapping()
    {
        logicalToPhysicalTileMapping = new Dictionary<(int, int), string>
        {
            {(2, 0), "R3_18"},
            {(2, 1), "R3_17"},
            {(2, 2), "R3_16"},
            {(2, 3), "R3_15"},
            {(2, 4), "R3_14"},
            {(2, 5), "R3_13"},
            {(2, 6), "R3_12"},
            {(2, 7), "R3_11"},
            {(2, 8), "R3_10"},
            {(2, 9), "R3_09"},
            {(2, 10), "R3_08"},
            {(2, 11), "R3_07"},
            {(2, 12), "R3_06"},
            {(2, 13), "R3_05"},
            {(2, 14), "R3_04"},
            {(2, 15), "R3_03"},
            {(2, 16), "R3_02"},
            {(2, 17), "R3_01"},
            {(2, 18), "R3_24"},
            {(2, 19), "R3_23"},
            {(2, 20), "R3_22"},
            {(2, 21), "R3_21"},
            {(2, 22), "R3_20"},
            {(2, 23), "R3_19"},

            {(3, 0), "R4_18"},
            {(3, 1), "R4_17"},
            {(3, 2), "R4_16"},
            {(3, 3), "R4_15"},
            {(3, 4), "R4_14"},
            {(3, 5), "R4_13"},
            {(3, 6), "R4_12"},
            {(3, 7), "R4_11"},
            {(3, 8), "R4_10"},
            {(3, 9), "R4_09"},
            {(3, 10), "R4_08"},
            {(3, 11), "R4_07"},
            {(3, 12), "R4_06"},
            {(3, 13), "R4_05"},
            {(3, 14), "R4_04"},
            {(3, 15), "R4_03"},
            {(3, 16), "R4_02"},
            {(3, 17), "R4_01"},
            {(3, 18), "R4_24"},
            {(3, 19), "R4_23"},
            {(3, 20), "R4_22"},
            {(3, 21), "R4_21"},
            {(3, 22), "R4_20"},
            {(3, 23), "R4_19"},
        };
    }
}
