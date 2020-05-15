using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/JudgeCondition/ChanceToAct")]
public class ChanceToAct : JudgeCondition
{
    [Range(0, 100)]
    public float chanceToAct;
    public override bool CheckActConditionHaviour()
    {
        return CheckChance();
    }

    private bool CheckChance()
    {
        if(Random.Range(0, 100) >= chanceToAct)
        {
            return true;
        }

        return false;
    }
}
