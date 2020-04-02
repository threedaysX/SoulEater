using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/JudgeCondition/CloseAttackDistance")]
public class CloseAttackDistance : JudgeCondition
{
    public override bool CheckActConditionHaviour()
    {
        return CheckCloseDistance();
    }

    private bool CheckCloseDistance()
    {
        if (ai.chaseTarget != null)
        {
            float xDistance = Mathf.Abs(ai.chaseTarget.position.x - ai.transform.position.x);
            // 若在攻擊距離內
            if (xDistance <= ai.data.attackRange.Value)
            {
                return true;
            }
        }
        return false;
    }
}
