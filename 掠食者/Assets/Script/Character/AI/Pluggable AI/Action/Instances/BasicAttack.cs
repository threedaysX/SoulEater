using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Action/BasicAttack")]
public class BasicAttack : Action
{
    public override bool StartActHaviour()
    {
        return Attack();
    }

    private bool Attack()
    {
        if (ai.operationController.StartAttack(AttackType.Attack, ai.data.attackElement))
        {
            Debug.Log(ai.characterName + "要攻擊你摟><");
            return true;
        }
        return false;
    }
}
