using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/JudgeCondition/OutOfAttackRange")]
public class OutOfAttackRange : JudgeCondition
{
    public override bool CheckActConditionHaviour()
    {
        return CheckFarDistance();
    }

    private bool CheckFarDistance()
    {
        if (ai.chaseTarget != null)
        {
            // 若在攻擊距離外
            if ((ai.chaseTarget.position - ai.transform.position).sqrMagnitude >= ai.data.attackRange.Value * ai.data.attackRange.Value)
            {
                return true;
            }
        }
        return false;
    }
}
