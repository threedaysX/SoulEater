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
        if (ai.distanceDetect.chaseTarget != null)
        {
            // 若在攻擊距離外
            if (ai.distanceDetect.AIToTargetDistance >= ai.data.attackRange.Value)
            {
                return true;
            }
        }
        return false;
    }
}
