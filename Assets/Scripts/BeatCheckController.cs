using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeatCheckController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlayerControl player;
    [SerializeField] private SniperAttack sniper;
    private bool playerVulnerable;

    // Get access to the PlayController instance, and set it. 
    void Start()
    {
        player = FindObjectOfType<PlayerControl>();
        sniper = FindObjectOfType<SniperAttack>();
        playerVulnerable = true;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (MusicEventHandler.beatCheck)
        {
            player.OnMove(context);
        }
        else
        {
            sniper.TriggerAttack();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (MusicEventHandler.beatCheck)
        {
            player.OnAttack(context);
        }
        else
        {
            sniper.TriggerAttack();
        }
    }


    public void SetVulnerable(bool status)
    {
        // Debug.Log($"set vulnerable to: {status}");
        playerVulnerable = status;
    }

    public bool GetVulnerable()
    {
        // Debug.Log($"get vulnerable: {playerVulnerable}");
        return playerVulnerable;
    }
}
