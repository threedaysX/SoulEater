using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/JudgeCondition/OutOfCustomRange")]
public class OutOfCustomRange : JudgeCondition
{
    public float customRange;
    public override bool CheckActConditionHaviour()
    {
        return CheckCustomFarDistance();
    }

    private bool CheckCustomFarDistance()
    {
        if (ai.chaseTarget != null)
        {
            // 若在自定距離外(含)
            if ((ai.chaseTarget.transform.position - ai.transform.position).sqrMagnitude >= customRange * customRange)
            {
                return true;
            }
        }
        return false;
    }
}
