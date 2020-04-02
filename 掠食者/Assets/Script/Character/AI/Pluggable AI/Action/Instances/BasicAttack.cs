using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Action/BasicAttack")]
public class BasicAttack : Action
{
    public override void StartActHaviour()
    {
        Attack();
    }

    private void Attack()
    {
        if (ai.combat.StartAttack())
        {
            Debug.Log(ai.characterName + "要攻擊你摟><");
        }
    }
}
