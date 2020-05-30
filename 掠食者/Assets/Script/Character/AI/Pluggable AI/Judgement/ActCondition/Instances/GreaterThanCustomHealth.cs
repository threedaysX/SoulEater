using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/JudgeCondition/GreaterThanCustomHealth")]
public class GreaterThanCustomHealth : JudgeCondition
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
        if ((ai.CurrentHealth / ai.data.maxHealth.Value) * 100 > healthPercentageToCheck)
        {
            return true;
        }
        return false;
    }
}
