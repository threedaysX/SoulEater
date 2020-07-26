using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCameraControl : Singleton<CinemachineCameraControl>
{
    private List<ZoomInSetting> zoomInSettings;
    private float nextZoomInStartTime = 0;

    private void Start()
    {
        zoomInSettings = new List<ZoomInSetting>();
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

    public void SetOnlyCurrentCamreaActive()
    {
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        CinemachineClearShot currentVcamParent = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineClearShot>();
        if (!currentVcamParent.IsLiveChild(GetCurrentActiveCamera())) Debug.Log("ch");//GetCurrentActiveCamera().gameObject.SetActive(false);
        else Debug.Log("e");// GetCurrentActiveCamera().gameObject.SetActive(true);
    }

    #region Zoom Camera
    private void CheckToZoomCamera()
    {
        if (zoomInSettings.Count > 0)
        {
            if (!zoomInSettings[0].isTrigger && Time.time >= nextZoomInStartTime)
            {
                zoomInSettings[0].isTrigger = true;
                StartCoroutine(ZoomInCamera(zoomInSettings[0].finalZoomSize, zoomInSettings[0].duration));
                nextZoomInStartTime = Time.time + zoomInSettings[0].duration + zoomInSettings[0].afterDelay;
                zoomInSettings.Remove(zoomInSettings[0]);
            }
        }
    }

    public void ZoomInCameraActions(params ZoomInSetting[] zoomInSettings)
    {
        this.zoomInSettings.AddRange(zoomInSettings);
    }

    public IEnumerator ZoomInCamera(float finalZoomSize, float duration)
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
    #endregion
}

public class ZoomInSetting
{
    public float finalZoomSize;
    public float duration;
    public float afterDelay;
    public bool isTrigger;
}