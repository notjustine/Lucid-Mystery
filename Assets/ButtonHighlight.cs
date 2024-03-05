using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHighlight : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject overlaySprite;
    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Selected");
        overlaySprite.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        overlaySprite.SetActive(false);
        Debug.Log("Deselected");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
