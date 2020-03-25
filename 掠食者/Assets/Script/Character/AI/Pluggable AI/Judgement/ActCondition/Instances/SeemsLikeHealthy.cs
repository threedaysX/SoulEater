using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/JudgeCondition/SeemsLikeHealthy")]
public class SeemsLikeHealthy : JudgeCondition
{
    public override bool CheckActConditionHaviour()
    {
        return CheckIfHealthy();
    }

    private bool CheckIfHealthy()
    {
        float healthPercentage = (ai.currentHealth / ai.data.maxHealth.Value) * 100;
        // 若血量大於75%
        if (healthPercentage >= 75)
        {
            return true;
        }
        return false;
    }
}
