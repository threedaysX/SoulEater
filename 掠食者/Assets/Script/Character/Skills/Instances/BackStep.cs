using UnityEngine;
using System.Collections;


public class BackStep : DisposableSkill
{
    protected override void AddAffectEvent()
    {
        immediatelyAffect.AddListener(InterruptAction);
        immediatelyAffect.AddListener(MoveBack);
    }

    /// <summary>
    /// 0.05秒內退移到後方6m處。	
    /// </summary>
    private void MoveBack()
    {
        sourceCaster.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        StartCoroutine(MoveToPosition(sourceCaster.transform
            , sourceCaster.transform.position + sourceCaster.transform.right * -6f
            , 0.05f));
    }
    private IEnumerator MoveToPosition(Transform transform, Vector3 destination, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, destination, t);
            yield return null;
        }
    }

    private void InterruptAction()
    {
        sourceCaster.SetOperation(LockType.OperationAction, true);
        sourceCaster.operationController.InterruptAnimOperation();
    }
}