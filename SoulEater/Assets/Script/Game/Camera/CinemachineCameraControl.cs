using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCameraControl : Singleton<CinemachineCameraControl>
{
    private List<ZoomInSetting> zoomInSettings;
    private int currentIndex = 0;
    private float nextZoomInStartTime = 0;
    private bool resetZoomTrigger = true;

    private void Start()
    {
        zoomInSettings = new List<ZoomInSetting>();
        resetZoomTrigger = true;
        currentIndex = 0;
    }

    private void Update()
    {
        CheckToZoomCamera();
    }

    public CinemachineVirtualCamera GetCurrentActiveCamera()
    {
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        CinemachineClearShot currentVcamParent = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineClearShot>();
        if (currentVcamParent != null)
        {
            // GetCurrentCamera - Active
            return currentVcamParent.LiveChild as CinemachineVirtualCamera;
        }
        return null;
    }

    #region Zoom Camera
    private void CheckToZoomCamera()
    {
        if (zoomInSettings.Count > 0)
        {
            // Reset First Time Zoom Delay.
            if (resetZoomTrigger)
            {
                nextZoomInStartTime = Time.time + zoomInSettings[currentIndex].startDelay;
                resetZoomTrigger = false;
            }
            else if (!resetZoomTrigger && !zoomInSettings[currentIndex].isTrigger && Time.time >= nextZoomInStartTime)
            {
                zoomInSettings[currentIndex].isTrigger = true;
                StartCoroutine(ZoomInCamera(zoomInSettings[currentIndex].finalZoomSize, zoomInSettings[currentIndex].duration));
                nextZoomInStartTime = Time.time + zoomInSettings[currentIndex].duration + GetZoomSettingNextStartDelay();
                currentIndex++;
            }

            // When finished, Clear zoomInSettings and reset state.
            if (currentIndex > zoomInSettings.Count - 1)
            {
                resetZoomTrigger = true;
                currentIndex = 0;
                zoomInSettings.Clear();
            }
        }
    }

    public void ZoomInCamera(params ZoomInSetting[] zoomInSettings)
    {
        this.zoomInSettings.AddRange(zoomInSettings);
    }

    private IEnumerator ZoomInCamera(float finalZoomSize, float duration)
    {
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        CinemachineClearShot currentVcamParent = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineClearShot>();
        if (currentVcamParent != null)
        {
            CinemachineVirtualCamera currentVcam = currentVcamParent.LiveChild as CinemachineVirtualCamera;
            if (currentVcam != null)
            {
                float originZoomSize = currentVcam.m_Lens.OrthographicSize;
                float timeLeft = duration;
                while (timeLeft > 0)
                {
                    currentVcam.m_Lens.OrthographicSize += (finalZoomSize - originZoomSize) * Time.deltaTime / duration;
                    timeLeft -= Time.deltaTime;
                    yield return null;
                }
                if (currentVcam.m_Lens.OrthographicSize != finalZoomSize)
                {
                    currentVcam.m_Lens.OrthographicSize = finalZoomSize;
                }
            }
        }
    }

    private float GetZoomSettingNextStartDelay()
    {
        if (currentIndex + 1 >= zoomInSettings.Count)
            return 0f;

        return zoomInSettings[currentIndex + 1].startDelay;
    }
    #endregion
}

public class ZoomInSetting
{
    public float finalZoomSize;
    public float duration;
    public float startDelay;
    public bool isTrigger;
}