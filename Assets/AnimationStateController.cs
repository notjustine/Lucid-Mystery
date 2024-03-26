using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    public BossHealth bossHealth;
    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        bossHealth = FindObjectOfType<BossHealth>();
        animator = GetComponent<Animator>();
        // Debug.Log(animator);
    }

    // Update is called once per frame
    void Update()
    {

        if (bossHealth.isPhase2)
        {
            animator.SetTrigger("Phase2");

        }
    }
}
