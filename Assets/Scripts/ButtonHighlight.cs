using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHighlight : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject overlaySprite;

    public void OnDisable()
    {
        overlaySprite.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (overlaySprite != null)
            overlaySprite.SetActive(true);
        AudioManager.instance.PlayOneShot(SoundRef.Instance.menuMove, new Vector3());
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (overlaySprite != null)
            overlaySprite.SetActive(false);
        // AudioManager.instance.PlayOneShot(SoundRef.Instance.menuMove, new Vector3());
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        if (overlaySprite != null)
            overlaySprite.SetActive(true);
        // AudioManager.instance.PlayOneShot(SoundRef.Instance.menuSelect, new Vector3());
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (overlaySprite != null)
            overlaySprite.SetActive(false);
        AudioManager.instance.PlayOneShot(SoundRef.Instance.menuMove, new Vector3());
    }

    // Start is called before the first frame update
    void Awake()
    {
        overlaySprite = transform.Find("Overlay").gameObject;
    }

}
