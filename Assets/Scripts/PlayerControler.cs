using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public int numberOfSlices = 8;
    public float radius = 50f;
    private Vector3[] sliceCenters;
    private int currentSliceIndex = 0;
    public float heightAboveSlices = 1.0f; // Adjust this value as needed
    public float topRatio = 0.25f;
    public float middleRatio = 0.50f;
    public float bottomRatio = 0.75f;
    private enum SliceSection { Top, Middle, Bottom }
    private SliceSection currentSection = SliceSection.Middle;

    // Movement Updates
    private float lastMoveTime;
    private float movementCooldown = 1f;
    public bool inputted { get; set; }
    
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
        // Initialize the slice centers array
        // lastMoveTime = Time.time;
        sliceCenters = new Vector3[numberOfSlices];
        float sliceAngle = 360f / numberOfSlices;
        for (int i = 0; i < numberOfSlices; i++)
        {
            sliceCenters[i] = GetSliceCenterPoint(radius, i * sliceAngle, sliceAngle) + Vector3.up * heightAboveSlices;
        }

        // Move player to the center of the first slice
        transform.position = sliceCenters[0];
    }

    public void AllowMove()
    {
        // if (Time.time - lastMoveTime >= movementCooldown)
        // {
            
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveToNextSlice();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveToPreviousSlice();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveOneLayerUp();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveOneLayerDown();
            }
        // }
    }
    void Update()
    {
        // Check for player input

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
        switch (currentSection)
        {
            case SliceSection.Bottom:
                currentSection = SliceSection.Middle;
                break;
            case SliceSection.Middle:
                currentSection = SliceSection.Top;
                break;
            case SliceSection.Top:
                break;
        }
        MoveToCurrentSlice();
    }

    void MoveOneLayerDown()
    {
        switch (currentSection)
        {
            case SliceSection.Bottom:
                break;
            case SliceSection.Middle:
                currentSection = SliceSection.Bottom;
                break;
            case SliceSection.Top:
                currentSection = SliceSection.Middle;
                break;
        }
        MoveToCurrentSlice();
    }

    void MoveToCurrentSlice()
    {
        Vector3 sliceCenter = sliceCenters[currentSliceIndex];
        float distanceMultiplier = 1.0f;
        
        switch (currentSection)
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
        }

        // Calculate new position
        Vector3 newPosition = sliceCenter * distanceMultiplier;

        // Keep the y-coordinate (height) constant
        newPosition.y = heightAboveSlices;

        transform.position = newPosition;
        inputted = true;
    }
}