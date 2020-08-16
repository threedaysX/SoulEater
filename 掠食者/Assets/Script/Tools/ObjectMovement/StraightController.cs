using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class StraightController : MonoBehaviour
{
    public void MoveToPoint(Transform targetPos, ObjectMoverSetting setting)
    {
        StartCoroutine(MoveCoroutine(transform, targetPos.position, setting));
    }

    private IEnumerator MoveCoroutine(Transform transform, Vector3 destination, ObjectMoverSetting setting)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / setting.timeToMove;
            transform.position = Vector3.Lerp(currentPos, destination, t);
            yield return null;
        }

        setting.afterMoveEvent.Invoke();
    }
}

public struct ObjectMoverSetting
{
    public float timeToMove;
    public UnityEvent afterMoveEvent;
}
