using System.Collections;
using UnityEngine;

public class DistanceDetect : MonoBehaviour
{
    [SerializeField] private Transform player;

    public float timeToAct;
    public float customDistance;
    private float lastSecDistance;
    public bool hasGetClose;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player != null)
        {
            StartCoroutine(LastSecDistance());
        }
    }

    private IEnumerator LastSecDistance()
    {
        lastSecDistance = (player.position - transform.position).sqrMagnitude;

        yield return new WaitForSeconds(timeToAct);

        if (player == null)
            yield break;

        if (lastSecDistance - (player.position - transform.position).sqrMagnitude >= customDistance * customDistance)
        {
            hasGetClose = true;
        }
        else
        {
            hasGetClose = false;
        }
    }
}