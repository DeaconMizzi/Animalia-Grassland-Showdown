using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // The player to follow
    public Vector3 offset;  // Offset from the target
    public float smoothSpeed = 0.125f; // Smooth dampening factor

    void LateUpdate()
    {
        if (target != null)
        {
            // Follow the player's X and Y position with an offset
            Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, offset.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPosition; // Update the camera position
        }
    }
}
