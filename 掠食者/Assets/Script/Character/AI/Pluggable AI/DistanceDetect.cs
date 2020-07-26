using System.Collections;
using UnityEngine;

public class DistanceDetect : Singleton<DistanceDetect>
{
    [SerializeField] private Transform player;
    [SerializeField] public Transform centerPoint;
    [HideInInspector] public Transform chaseTarget;

    public float AIToTargetDistance;    //測這AI和另一個點(Transform)的距離

    public float timeToAct;
    public float customDistance;
    private float lastSecDistance;
    public bool hasGetClose;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        centerPoint = transform.Find("AttackCenterPoint");
    }

    private void Update()
    {
        if (player != null)
            StartCoroutine(LastSecDistance());
    }

    public void CheckDistance()    //測這AI和另一個點(Transform)的距離
    {
        if (centerPoint != null && chaseTarget != null)
            AIToTargetDistance = (centerPoint.position - chaseTarget.position).magnitude;
    }

    private IEnumerator LastSecDistance()
    {
        lastSecDistance = AIToTargetDistance;

        yield return new WaitForSeconds(timeToAct);

        if (player == null)
            yield break;

        if (lastSecDistance - AIToTargetDistance >= customDistance)
            hasGetClose = true;
        else
            hasGetClose = false;
    }

}