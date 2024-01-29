using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputIndicator : MonoBehaviour
{
    
    public static InputIndicator Instance { get; private set; }
    private bool clicked = false;
    // public Transform transform { get; private set; }
    public Material material;
    public Color color;
    // public Text hitText;
    // private int counter = 0;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Input Indicator");
        }

        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (!clicked)
        clicked = Input.GetButtonDown("Jump");
        material.color = color;
    }
}
