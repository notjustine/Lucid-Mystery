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

    public void TriggerStumble()            // call in PlayerStatus.cs
    {
        animator.SetTrigger("Stumble");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
