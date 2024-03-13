using UnityEngine;

public class PhaseController : MonoBehaviour
{
    public static PhaseController Instance;
    public int phase = 0;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Phase Controller");
        }

        Instance = this;
    }
}
