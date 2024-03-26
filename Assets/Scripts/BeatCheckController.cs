using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeatCheckController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlayerControl player;
    [SerializeField] private SniperAttack sniper;
    private bool playerVulnerable;
    private Attack attack;
    private CameraControl cameraControl;

    // Get access to the PlayController instance, and set it. 
    void Start()
    {
        player = FindObjectOfType<PlayerControl>();
        sniper = FindObjectOfType<SniperAttack>();
        attack = FindObjectOfType<Attack>();
        cameraControl = FindObjectOfType<CameraControl>();
        playerVulnerable = true;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
            return;
        
        if (MusicEventHandler.beatCheck)
        {
            player.OnMove(context);
            InputIndicator.Instance.type = InputIndicator.SpriteType.ON_BEAT_INPUTTED;
            
        }
        else
        {
            cameraControl.TriggerShake();
            attack.UpdateCombo(Attack.ComboChange.DECREASE);
            player.inputted = true;
            InputIndicator.Instance.type = InputIndicator.SpriteType.OFF_BEAT_INPUTTED;
            sniper.TriggerAttack();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
            return;
        
        if (MusicEventHandler.beatCheck)
        {
            InputIndicator.Instance.type = (InputIndicator.SpriteType.ON_BEAT_INPUTTED);
            player.OnAttack(context);
        }
        else
        {
            player.inputted = true;
            attack.UpdateCombo(Attack.ComboChange.DECREASE);
            InputIndicator.Instance.type = (InputIndicator.SpriteType.OFF_BEAT_INPUTTED);
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
