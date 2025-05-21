using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float scrollSpeed = 0.5f;  // Speed of the background scroll
    private Transform cameraTransform; // Reference to the camera
    private Vector3 initialPosition;   // Initial position of the background

    void Start()
    {
        // Automatically find the camera
        cameraTransform = Camera.main.transform;

        if (cameraTransform == null)
        {
            Debug.LogError("Main Camera not found! Make sure your camera has the 'MainCamera' tag.");
            return;
        }

        // Store the initial position of the background
        initialPosition = transform.position;
    }

    void LateUpdate()
    {
        if (cameraTransform != null)
        {
            

            // Adjust the background's position
            transform.position = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z);
        }
    }
}
