using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/JudgeCondition/PlayerCloseAttack")]
public class PlayerCloseAttack : JudgeCondition
{
    public override bool CheckActConditionHaviour()
    {
        return true;  //暫定
    }
}
