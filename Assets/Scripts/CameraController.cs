using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // The player to follow
    public Vector3 offset;  // Offset from the target
    public float smoothSpeed = 0.125f; // Smooth dampening factor

    private Vector3 shakeOffset = Vector3.zero;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0f;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, offset.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition + shakeOffset;
        }

        if (shakeDuration > 0)
        {
            shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeOffset = Vector3.zero;
        }
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }

    public void ShakeOnce() // For Timeline or external triggers
    {
        ShakeCamera(5f, 0.1f); // Adjust default values as you like
    }
}
