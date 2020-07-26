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
        if (ai.distanceDetect.chaseTarget != null)
        {
            // 若在自定距離外(含)
            if (ai.distanceDetect.AIToTargetDistance >= customRange)
            {
                return true;
            }
        }
        return false;
    }
}
