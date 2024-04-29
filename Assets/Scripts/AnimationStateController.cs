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
    {                                       // clip length: 1.292
        //animator.SetTrigger("Awaken");
        Invoke("ClearFlinch", 1.0f);
    }

    public void TriggerPhase2()             // call in BossHealth.cs
    {                                       // clip length: 10.625
        animator.SetTrigger("Phase2");
        Invoke("ClearFlinch", 10.4f);
    }
    
    public void TriggerSlam()               // call in SlamAttack.cs
    {                                       // clip length: 3.458
        animator.SetTrigger("Slam");
        Invoke("ClearFlinch", 3.2f);
        Invoke("ClearTransition", 3.2f);
    }

    public void TriggerFlinch()             // call in BossHealth.cs
    {
        animator.SetTrigger("Hit");
        
        int randomNumber = Random.Range(1, 3);
        animator.SetTrigger("Flinch" + randomNumber);
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

    public void TriggerAttack()             // call in PlayerControl.cs
    {
        int randomNumber = Random.Range(1, 3);
        animator.SetTrigger("Attack" + randomNumber);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void ClearFlinch()
    {
        animator.ResetTrigger("Flinch1");
        animator.ResetTrigger("Flinch2");
    }

    private void ClearTransition()
    {
        animator.ResetTrigger("Phase2");
    }
}
