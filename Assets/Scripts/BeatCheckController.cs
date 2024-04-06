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
        
        player.OnMove(context);
        if (MusicEventHandler.beatCheck)
        {
            InputIndicator.Instance.type = InputIndicator.SpriteType.ON_BEAT_INPUTTED;
        }
        else
        {
            cameraControl.TriggerShake();
            player.inputted = true;
            InputIndicator.Instance.type = InputIndicator.SpriteType.OFF_BEAT_INPUTTED;
            sniper.TriggerAttack();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
            return;
        
        player.OnAttack(context);
        if (MusicEventHandler.beatCheck)
        {
            InputIndicator.Instance.type = (InputIndicator.SpriteType.ON_BEAT_INPUTTED);
        }
        else
        {
            player.inputted = true;
            InputIndicator.Instance.type = (InputIndicator.SpriteType.OFF_BEAT_INPUTTED);
            sniper.TriggerAttack();
        }
    }


    public void SetVulnerable(bool status)
    {
        playerVulnerable = status;
    }

    public bool GetVulnerable()
    {
        return playerVulnerable;
    }
}
