using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHighlight : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private GameObject overlaySprite;
    public void OnSelect(BaseEventData eventData)
    {
        overlaySprite.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        overlaySprite.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        overlaySprite = transform.Find("Overlay").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
