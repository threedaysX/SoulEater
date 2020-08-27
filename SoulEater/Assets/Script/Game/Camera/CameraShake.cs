using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    private static bool shaking;

    // Cinemachine Shake
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    public void ShakeCamera(float shakeAmplitude, float shakeFrequency, float duration, float startDelay = 0f,  bool overrideShake = true)
    {
        StartCoroutine(ShakeCameraCoroutine(shakeAmplitude, shakeFrequency, duration, startDelay, overrideShake));
    }

    public IEnumerator ShakeCameraCoroutine(float shakeAmplitude, float shakeFrequency, float duration, float startDelay = 0f, bool overrideShake = false)
    {
        yield return new WaitForSeconds(startDelay);

        // Get current Vcam's noise setting. (Check 6D Shake is added)
        virtualCameraNoise = CinemachineCameraControl.Instance.GetCurrentActiveCamera().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        // If null or in shaking, return.
        if (virtualCameraNoise == null || (shaking && !overrideShake))
            yield break;    

        shaking = true;
        float timeleft = duration;
        while (timeleft > 0)
        {
            // Set Cinemachine Camera Noise parameters
            virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
            virtualCameraNoise.m_FrequencyGain = shakeFrequency;

            timeleft -= Time.deltaTime;
            yield return null;
        }
        virtualCameraNoise.m_AmplitudeGain = 0;
    }
}
