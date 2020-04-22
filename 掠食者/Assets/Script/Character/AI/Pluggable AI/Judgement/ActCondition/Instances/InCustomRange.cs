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
        if (ai.chaseTarget != null)
        {
            // 若在自定距離內(含)
            if ((ai.chaseTarget.transform.position - ai.transform.position).sqrMagnitude <= customRange * customRange)
            {
                return true;
            }
        }
        return false;
    }
}
