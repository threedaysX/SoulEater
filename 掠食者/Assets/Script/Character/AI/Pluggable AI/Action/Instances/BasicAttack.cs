using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Action/BasicAttack")]
public class BasicAttack : AiAction
{
    public override bool StartActHaviour()
    {
        return Attack();
    }

    private bool Attack()
    {
        if (ai.StartAttack(AttackType.Attack, ai.data.attackElement))
        {
            return true;
        }
        return false;
    }
}
