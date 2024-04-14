using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputIndicator : MonoBehaviour
{

    public enum SpriteType
    {
        ON_BEAT_INPUTTED,
        OFF_BEAT_INPUTTED
    }
    
    public static InputIndicator Instance { get; private set; }
    private GameObject onBeatImage;
    private GameObject offBeatImage;
    private Animator animator;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Input Indicator");
        }
    
        Instance = this;
        var images = gameObject.GetComponentsInChildren<Image>(true);
        onBeatImage = images[1].gameObject;
        offBeatImage = images[2].gameObject;
        animator =  GetComponentInChildren<Animator>();
    }

    void Update()
    {
    }
    
    public void PlayAnimation(string animationName, int layer, float normalizedTime)
    {
        animator.Play(animationName, layer, normalizedTime);
    }
    
    public void SetBeatInput(SpriteType type)
    {
        StartCoroutine(BeatInput(type));
    }

    private IEnumerator BeatInput(SpriteType type)
    {
        if (type == SpriteType.ON_BEAT_INPUTTED)
        {
            onBeatImage.SetActive(true);
            offBeatImage.SetActive(false);
        }
        else
        {
            onBeatImage.SetActive(false);
            offBeatImage.SetActive(true);
        }
        yield return new WaitForSeconds(0.3f);
        onBeatImage.SetActive(false);
        offBeatImage.SetActive(false);
    }
}
