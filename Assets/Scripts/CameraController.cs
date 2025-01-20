using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;       // The player to follow
    public float smoothSpeed = 0.125f; // Smoothness of the camera movement
    public float fixedY = 0f;      // Fixed Y position for the camera
    public float fixedZ = -10f;    // Fixed Z position for the camera

    void LateUpdate()
    {
        if (target != null)
        {
            // Follow the target's X position while keeping Y and Z fixed
            Vector3 desiredPosition = new Vector3(target.position.x, fixedY, fixedZ);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
