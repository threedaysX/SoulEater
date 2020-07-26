using UnityEngine;
using System.Collections;
using Cinemachine;

[RequireComponent(typeof(CinemachineConfiner))]
[RequireComponent(typeof(CinemachineImpulseListener))]
public class CameraSide : MonoBehaviour
{
    //private OperationController playerOperationController;
    //private CinemachineComponentBase componentBase;
    //private CinemachineVirtualCamera vcam;
    //private CinemachineConfiner confiner;
    //private CinemachineImpulseListener impulseListener;

    //private void Start()
    //{
    //    if(this.gameObject.GetComponent<CinemachineConfiner>() == null)
    //        this.gameObject.AddComponent<CinemachineConfiner>();
    //    if(this.gameObject.GetComponent<CinemachineImpulseListener>() == null)
    //        this.gameObject.AddComponent<CinemachineImpulseListener>();

    //    confiner = GetComponent<CinemachineConfiner>();
    //    confiner.m_ConfineMode = 0;    //enum, 0代表2D
    //    confiner.m_ConfineScreenEdges = true;

    //    //要怎抓COLLIDER
    //    impulseListener = GetComponent<CinemachineImpulseListener>();
    //    impulseListener.m_Use2DDistance = true;          //把晃動調到2D

    //    playerOperationController = GameObject.FindWithTag("Player").GetComponent<OperationController>();
    //    vcam = GetComponent<CinemachineVirtualCamera>();
    //    componentBase = vcam.GetCinemachineComponent(CinemachineCore.Stage.Body);

    //    (componentBase as CinemachineFramingTransposer).m_XDamping = 0f;
    //}

    //private void Update()
    //{
    //    if (!playerOperationController.isMoving && !playerOperationController.isIdle)
    //    {
    //        if (componentBase is CinemachineFramingTransposer)
    //        {
    //            StartCoroutine(CheckCameraStable());
    //        }
    //    }
    //}

    //private IEnumerator CheckCameraStable()         //讓畫面不要那麼搖(除了IDLE和RUN的時候)
    //{
    //    (componentBase as CinemachineFramingTransposer).m_XDamping = 20f;
    //    yield return new WaitForSeconds(1.5f);
    //    (componentBase as CinemachineFramingTransposer).m_XDamping = 0f;
    //}
}
