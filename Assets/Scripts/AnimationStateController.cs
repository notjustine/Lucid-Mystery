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
    public void TriggerAwaken()             // call in Attack.cs
    {
        animator.SetTrigger("Awaken");
    }

    public void TriggerPhase2()             // call in BossHealth.cs
    {
        animator.SetTrigger("Phase2");
    }
    
    public void TriggerFlinch()             // call in Attack.cs
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

    public void TriggerAttack1()            // call in PlayerControl.cs
    {
        animator.SetTrigger("Attack1");
    }

    // Update is called once per frame
    void Update()
    {
    }
}