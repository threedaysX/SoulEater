using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Action/SelfHeal")]
public class SelfHealAction : Action
{
    public Skill selfHealObject;
    public override bool StartActHaviour()
    {
        return SelfHeal();
    }

    private bool SelfHeal()
    {
        return ai.skillController.Trigger(selfHealObject);
    }
}
