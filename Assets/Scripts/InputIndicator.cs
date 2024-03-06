using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputIndicator : MonoBehaviour
{
    
    public static InputIndicator Instance { get; private set; }
    public Material material;
    //public Color color;
    public MeshRenderer inputMesh;
    public bool active = false;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Input Indicator");
        }

        Instance = this;
    }

    void Update()
    {
        inputMesh.enabled = active;

    }
}
