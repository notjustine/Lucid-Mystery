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

    public void TriggerPhase2()
    {
        animator.SetTrigger("Phase2");
    }
    
    // Update is called once per frame
    void Update()
    {
    }
}
