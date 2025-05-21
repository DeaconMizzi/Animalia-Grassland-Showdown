using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShakeController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCam;

    public void ShakeOnce()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        var noise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise != null)
        {
            noise.m_AmplitudeGain = 2f; // rumble strength
            noise.m_FrequencyGain = 1f; // speed of shake
        }

        yield return new WaitForSeconds(2f); // duration

        if (noise != null)
        {
            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = 0f;
        }
    }
}
