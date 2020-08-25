using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Detect/OverlapCircleDecision")]
public class OverlapCircleDecision : Detect
{
    public override bool StartDetectHaviour()
    {
        return OverlapCircle();
    }

    private bool OverlapCircle()
    {
        Collider2D col = Physics2D.OverlapCircle(ai.transform.position, ai.detectDistance, ai.playerLayer);

        if (!col)
        {

        }
        else if (col.CompareTag("Player"))
        {
            ai.SetChaseTarget(col.transform);
            return true;
        }
        return false;
    }
}