using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Action/DashAttack")]
public class DashAttackAction : Action
{
    public Skill dashAttackObject;
    public override bool StartActHaviour()
    {
        return DashAttack();
    }

    private bool DashAttack()
    {
        return ai.skillController.Trigger(dashAttackObject);
    }
}
