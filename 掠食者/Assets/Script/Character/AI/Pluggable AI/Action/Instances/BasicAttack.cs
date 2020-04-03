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
        if (ai.combat.StartAttack())
        {
            Debug.Log(ai.characterName + "要攻擊你摟><");
            return true;
        }
        return false;
    }
}
