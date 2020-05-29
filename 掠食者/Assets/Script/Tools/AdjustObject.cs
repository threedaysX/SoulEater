using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AdjustObject : MonoBehaviour
{
    public float timeToMove;
    public UnityEvent afterMoveEvent;

    public void MoveToPoint(Transform targetPos)
    {
        StartCoroutine(MoveCoroutine(transform, targetPos.position, timeToMove));
    }

    private IEnumerator MoveCoroutine(Transform transform, Vector3 destination, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, destination, t);
            yield return null;
        }

        afterMoveEvent.Invoke();
    }
}
