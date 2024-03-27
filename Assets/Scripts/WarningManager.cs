using System.Collections.Generic;
using UnityEngine;


public class WarningManager : MonoBehaviour
{
    public enum WarningType
    {
        SNIPER,
        SLAM,
        STEAM,
        SPIRAL,
        HAZARD,
        INFO
    }

    private Dictionary<(int, int), string> logicalToPhysicalTileMapping;
    private List<string> sniperWarnings;
    private List<string> allWarnings;
    private List<string> infoTiles;

    // for blink effect
    [SerializeField] Color tileColor;
    [SerializeField] Color turretColor;
    [SerializeField] Color warningColor;
    [SerializeField] Color infoColor;
    private const float TURRET_BLINK_SPEED = 0.3f;
    private const float TILE_BLINK_SPEED = 0.6f;
    private GameObject tempObject;
    private MeshRenderer tempRenderer;
    private MaterialPropertyBlock propBlock;
    // end blink effect stuff


    public static WarningManager Instance { get; private set; }

    /**
         FOR REFERENCE:  THIS ACTUALLY WORKS TO CHANGE COLOR OF A SHADER

        testSphere = GameObject.Find("test-to-delete");
        testRenderer = testSphere.GetComponent<MeshRenderer>();
        MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
        testRenderer.GetPropertyBlock(propBlock);
        propBlock.SetColor("_BaseColor", blinkEnd);  // double check that the name in the inspector is actually "Base Color"
        testRenderer.SetPropertyBlock(propBlock);
    */


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Warning Manager");
        }
        Instance = this;
    }


    void Start()
    {
        sniperWarnings = new List<string>();
        allWarnings = new List<string>();
        infoTiles = new List<string>();
        InitLogicToPhysMapping();
    }


    void Update()
    {
        // Every name found in allWarnings should be blinking.
        foreach (string name in allWarnings)
        {
            tempObject = GameObject.Find(name);
            tempRenderer = tempObject.GetComponent<MeshRenderer>();
            propBlock = new MaterialPropertyBlock();
            tempRenderer.GetPropertyBlock(propBlock);

            Color color = tempObject.CompareTag("Turret") ? turretColor : tileColor;
            float speed = tempObject.CompareTag("Turret") ? TURRET_BLINK_SPEED : TILE_BLINK_SPEED;
            propBlock.SetColor("_BaseColor", Color.Lerp(color, warningColor, Mathf.PingPong(Time.time, speed)));
            tempRenderer.SetPropertyBlock(propBlock);
        }

        foreach (string name in infoTiles)
        {
            tempObject = GameObject.Find(name);
            tempRenderer = tempObject.GetComponent<MeshRenderer>();
            propBlock = new MaterialPropertyBlock();
            tempRenderer.GetPropertyBlock(propBlock);
            propBlock.SetColor("_BaseColor", Color.Lerp(tileColor, infoColor, Mathf.PingPong(Time.time, TILE_BLINK_SPEED)));
            tempRenderer.SetPropertyBlock(propBlock);
        }
    }


    public List<string> GetSniperWarnings()
    {
        return sniperWarnings;
    }


    /**
        Given the names of tiles to update, changes them to warning or toggles them back to original accordingly.

        NOTE:  this puts the responsibility on various attacks to know which tiles they should warn about (using IWarningGenerator interface)
    */
    public List<string> ToggleWarning(List<string> objects, bool warningActive, WarningType type)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            GameObject objectToChange = GameObject.Find(objects[i]);  // TO-DO: add error handling/null checks maybe
            MeshRenderer renderer = objectToChange.GetComponent<MeshRenderer>();
            if (warningActive)
            {
                TrackWarning(objects[i], type);  // So that Update function can make it blink
            }
            else
            {
                if (type != WarningType.INFO)
                {
                    allWarnings.Remove(objects[i]);
                    HandleRemoveSniper(type, objects[i]);
                    HandleToggleMaterial(objects[i], renderer, objectToChange.CompareTag("Turret"));
                }
                else
                {
                    // Tutorial
                    infoTiles.Remove(objects[i]);
                    HandleToggleMaterial(objects[i], renderer, false);
                }
            }
        }
        return objects;
    }


    /**
        If the type of warning is sniper, un-track the sniper warning for that tile.
    */
    private void HandleRemoveSniper(WarningType type, string tilename)
    {
        if (type == WarningType.SNIPER)
        {
            sniperWarnings.Remove(tilename);
        }
    }


    /**
        Keep this tile as the warning color if there is still another outstanding warning.
    */
    private void HandleToggleMaterial(string name, MeshRenderer renderer, bool isTurret)
    {
        if (!allWarnings.Contains(name))
        {
            propBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propBlock);
            propBlock.SetColor("_BaseColor", isTurret ? turretColor : tileColor);
            renderer.SetPropertyBlock(propBlock);
        }
    }


    /**
        Keep track of different warnings that are active, so they can be undone.
    */
    private void TrackWarning(string tileName, WarningType type)
    {
        switch (type)
        {
            case WarningType.SNIPER:
                sniperWarnings.Add(tileName);
                allWarnings.Add(tileName);
                break;
            case WarningType.INFO:
                infoTiles.Add(tileName);
                break;
            default:
                // Steam + Slam + Hazard + Spiral
                allWarnings.Add(tileName);
                break;
        }
    }


    /**
        Getter for the mapping so that the attack classes can fetch it for indexing into it.
    */
    public Dictionary<(int, int), string> GetLogicalToPhysicalTileMapping()
    {
        return logicalToPhysicalTileMapping;
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
            {(0, 0), "R1_18"},
            {(0, 1), "R1_17"},
            {(0, 2), "R1_16"},
            {(0, 3), "R1_15"},
            {(0, 4), "R1_14"},
            {(0, 5), "R1_13"},
            {(0, 6), "R1_12"},
            {(0, 7), "R1_11"},
            {(0, 8), "R1_10"},
            {(0, 9), "R1_09"},
            {(0, 10), "R1_08"},
            {(0, 11), "R1_07"},
            {(0, 12), "R1_06"},
            {(0, 13), "R1_05"},
            {(0, 14), "R1_04"},
            {(0, 15), "R1_03"},
            {(0, 16), "R1_02"},
            {(0, 17), "R1_01"},
            {(0, 18), "R1_24"},
            {(0, 19), "R1_23"},
            {(0, 20), "R1_22"},
            {(0, 21), "R1_21"},
            {(0, 22), "R1_20"},
            {(0, 23), "R1_19"},

            {(1, 0), "R2_18"},
            {(1, 1), "R2_17"},
            {(1, 2), "R2_16"},
            {(1, 3), "R2_15"},
            {(1, 4), "R2_14"},
            {(1, 5), "R2_13"},
            {(1, 6), "R2_12"},
            {(1, 7), "R2_11"},
            {(1, 8), "R2_10"},
            {(1, 9), "R2_09"},
            {(1, 10), "R2_08"},
            {(1, 11), "R2_07"},
            {(1, 12), "R2_06"},
            {(1, 13), "R2_05"},
            {(1, 14), "R2_04"},
            {(1, 15), "R2_03"},
            {(1, 16), "R2_02"},
            {(1, 17), "R2_01"},
            {(1, 18), "R2_24"},
            {(1, 19), "R2_23"},
            {(1, 20), "R2_22"},
            {(1, 21), "R2_21"},
            {(1, 22), "R2_20"},
            {(1, 23), "R2_19"},

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
