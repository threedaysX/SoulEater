using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/JudgeCondition/LessThanCustomHealth")]
public class LessThanCustomHealth : JudgeCondition
{
    [Range(0, 100)]
    public float healthPercentageToCheck;
    public override bool CheckActConditionHaviour()
    {
        return CheckIfHealthy();
    }

    private bool CheckIfHealthy()
    {
        //healthPercentage
        if ((ai.currentHealth / ai.data.maxHealth.Value) * 100 < healthPercentageToCheck)
        {
            return true;
        }
        return false;
    }
}
