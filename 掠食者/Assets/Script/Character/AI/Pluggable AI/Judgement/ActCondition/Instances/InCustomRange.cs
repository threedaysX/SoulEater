using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/JudgeCondition/InCustomRange")]
public class InCustomRange : JudgeCondition
{
    public float customRange;
    public override bool CheckActConditionHaviour()
    {
        return CheckCustomCloseDistance();
    }

    private bool CheckCustomCloseDistance()
    {
        if (ai.distanceDetect.chaseTarget != null && ai.distanceDetect.centerPoint != null)
        {
            // 若在自定距離內(含)
            if (ai.distanceDetect.AIToTargetDistance <= customRange)
            {
                return true;
            }
        }
        return false;
    }
}
