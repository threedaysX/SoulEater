using UnityEngine;
using System.Collections;


public class BackStep : DisposableSkill
{
    protected override void AddAffectEvent()
    {
        immediatelyAffect.AddListener(MoveBack);
    }

    /// <summary>
    /// 0.1秒內退移到後方4m處。	
    /// </summary>
    private void MoveBack()
    {
        sourceCaster.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        StartCoroutine(MoveToPosition(sourceCaster.transform
            , sourceCaster.transform.position + sourceCaster.transform.right * -4f
            , 0.1f));
    }
    public IEnumerator MoveToPosition(Transform transform, Vector3 destination, float timeToMove)
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
}