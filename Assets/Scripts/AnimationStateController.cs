using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        // Debug.Log(animator);
        
    }

    // BOSS ANIMATIONS
    public void TriggerAwaken()             // call in BossHealth.cs
    {
        animator.ResetTrigger("Flinch");
        animator.SetTrigger("Awaken");
    }

    public void TriggerPhase2()             // call in BossHealth.cs
    {
        animator.SetTrigger("Phase2");
    }
    
    public void TriggerSlam()               // call in SlamAttack.cs
    {
        animator.SetTrigger("Slam");
    }

    public void TriggerFlinch()             // call in BossHealth.cs
    {
        animator.SetTrigger("Flinch");
    }

    public void TriggerDeath()              // call in BossHealth.cs
    {
        animator.SetTrigger("Death");
    }

    // PLAYER ANIMATIONS
    public void TriggerStumble()            // call in PlayerStatus.cs
    {
        animator.SetTrigger("Stumble");
    }

    public void TriggerAttack()            // call in PlayerControl.cs
    {
        int randomNumber = Random.Range(1, 3);
        animator.SetTrigger("Attack" + randomNumber);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
