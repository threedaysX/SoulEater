using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/JudgeCondition/CheckHitsInTime")]
public class CheckHitsInTime : JudgeCondition
{
    [Header("在幾秒內有受傷")]
    public float customTime;
    public override bool CheckActConditionHaviour()
    {
        if (ai == null)
            return false;

        ai.combatController.hasHitInTime = customTime;
        return ai.combatController.hasHit;
    }
}