using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/JudgeCondition/TooFar")]
public class TooFar : JudgeCondition
{
    public override bool CheckActConditionHaviour()
    {
        return CheckFarDistance();
    }

    private bool CheckFarDistance()
    {
        if (ai.chaseTarget != null)
        {
            float xDistance = Mathf.Abs(ai.chaseTarget.position.x - ai.transform.position.x);
            // 若在攻擊距離外
            if (xDistance > ai.data.attackRange.Value)
            {
                return true;
            }
        }
        return false;
    }
}
