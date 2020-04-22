using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/JudgeCondition/InAttackRange")]
public class InAttackRange : JudgeCondition
{
    public override bool CheckActConditionHaviour()
    {
        return CheckCloseDistance();
    }

    private bool CheckCloseDistance()
    {
        if (ai.chaseTarget != null)
        {
            // 若在攻擊距離內(含)
            if ((ai.chaseTarget.transform.position - ai.transform.position).sqrMagnitude <=  ai.data.attackRange.Value * ai.data.attackRange.Value)
            {
                return true;
            }
        }
        return false;
    }
}
