using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public int numberOfSlices = 8;
    public float radius = 50f;
    private Vector3[] sliceCenters;
    private int currentSliceIndex = 0;
    public float heightAboveSlices = 1.0f; // Adjust this value as needed
    //public float topRatio = 0.25f;
    //public float middleRatio = 0.50f;
    //public float bottomRatio = 0.75f;

    public float topRatio = 0.40f;
    public float midTopRatio = 0.60f;
    public float midBotRatio = 0.80f;
    public float botRatio = 1f;
    //private enum SliceSection { Top, Middle, Bottom }
    private enum SliceSection { Top, MidTop, MidBot, Bot}
    //private SliceSection currentSection = SliceSection.Middle;
    private SliceSection currentSection = SliceSection.Bot;
    private Animator animator;
    private Transform player;
    public Transform cameraTransform; 
    public EventReference attackSound;
    // Movement Updates
    public bool inputted { get;  set; }
    
    Vector3 GetSliceCenterPoint(float radius, float angle, float sliceAngle)
    {
        // Calculate the bisector angle for the slice
        float bisectorAngle = angle + sliceAngle / 2;
        // Convert the angle to radians
        bisectorAngle *= Mathf.Deg2Rad;
        // Calculate the center point
        return new Vector3(radius * Mathf.Cos(bisectorAngle), 0, radius * Mathf.Sin(bisectorAngle));
    }

    void Start()
    {
        animator = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Animator>();
        
        // animator = GetComponent<Animator>();
        player = GetComponent<Transform>();
        // Initialize the slice centers array
        sliceCenters = new Vector3[numberOfSlices];
        float sliceAngle = 360f / numberOfSlices;
        for (int i = 0; i < numberOfSlices; i++)
        {
            sliceCenters[i] = GetSliceCenterPoint(radius, i * sliceAngle, sliceAngle) + Vector3.up * heightAboveSlices;
        }

        // Move player to the center of the first slice
        transform.position = sliceCenters[0];
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!MusicEventHandler.beatCheck)
            return;

        if (context.phase == InputActionPhase.Started) {
            if (inputted)
                return;
            // animator.SetTrigger("Jump");
            return;
        }
        
        Vector2 move = context.ReadValue<Vector2>();
        bool xDominantAxis = (Mathf.Abs(move.x) > Mathf.Abs(move.y));
        // animator.SetTrigger("Jump");
        if (xDominantAxis)
        {
            if (move.x > 0)
                MoveToNextSlice();
            else if (move.x < 0)
                MoveToPreviousSlice();
        }
        else
        {
            if (move.y > 0)
                MoveOneLayerUp();
            else if (move.y < 0)
                MoveOneLayerDown();
        }
        
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!MusicEventHandler.beatCheck)
            return;
        
        if (context.phase != InputActionPhase.Started || inputted)
            return;
        
        // Debug.Log("ATTACK");
        inputted = true;
        animator.SetTrigger("Attack");
        AudioManager.instance.PlayOneShot(attackSound, player.position);
    }
    
    
    void Update()
    {
        // Check for player input
        player.rotation = cameraTransform.rotation;
    }

    void MoveToNextSlice()
    {
        currentSliceIndex = (currentSliceIndex + 1) % numberOfSlices;
        MoveToCurrentSlice();
    }

    void MoveToPreviousSlice()
    {
        if (currentSliceIndex == 0)
        {
            currentSliceIndex = numberOfSlices - 1;
        }
        else
        {
            currentSliceIndex--;
        }
        MoveToCurrentSlice();
    }
    void MoveOneLayerUp()
    {
        /*switch (currentSection)
        {
            case SliceSection.Bottom:
                currentSection = SliceSection.Middle;
                break;
            case SliceSection.Middle:
                currentSection = SliceSection.Top;
                break;
            case SliceSection.Top:
                break;
        }*/

        switch (currentSection)
        {
            case SliceSection.Bot:
                currentSection = SliceSection.MidBot;
                break;
            case SliceSection.MidBot:
                currentSection = SliceSection.MidTop;
                break;
            case SliceSection.MidTop:
                currentSection = SliceSection.Top;
                break;
            case SliceSection.Top:
                break;
        }
        MoveToCurrentSlice();
    }

    void MoveOneLayerDown()
    {
        /*switch (currentSection)
        {
            case SliceSection.Bottom:
                break;
            case SliceSection.Middle:
                currentSection = SliceSection.Bottom;
                break;
            case SliceSection.Top:
                currentSection = SliceSection.Middle;
                break;
        }*/

        switch (currentSection)
        {
            case SliceSection.Bot:
                break;
            case SliceSection.MidBot:
                currentSection = SliceSection.Bot;
                break;
            case SliceSection.MidTop:
                currentSection = SliceSection.MidBot;
                break;
            case SliceSection.Top:
                currentSection = SliceSection.MidTop;
                break;
        }
        MoveToCurrentSlice();
    }

    void MoveToCurrentSlice()
    {
        Vector3 sliceCenter = sliceCenters[currentSliceIndex];
        float distanceMultiplier = 1.0f;

        /*switch (currentSection)
        {
            case SliceSection.Top:
                distanceMultiplier = topRatio;
                break;
            case SliceSection.Middle:
                distanceMultiplier = middleRatio;
                break;
            case SliceSection.Bottom:
                distanceMultiplier = bottomRatio;
                break;
        }*/

        switch (currentSection)
        {
            case SliceSection.Top:
                distanceMultiplier = topRatio;
                break;
            case SliceSection.MidTop:
                distanceMultiplier = midTopRatio;
                break;
            case SliceSection.MidBot:
                distanceMultiplier = midBotRatio;
                break;
            case SliceSection.Bot:
                distanceMultiplier = botRatio;
                break;
        }

        // Calculate new position
        Vector3 newPosition = sliceCenter * distanceMultiplier;

        // Keep the y-coordinate (height) constant
        newPosition.y = heightAboveSlices;

        
        transform.position = newPosition;
        
        inputted = true;
    }
}