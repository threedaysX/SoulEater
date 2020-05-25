using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Action/EvadeArea")]
public class EvadeAreaAction : Action
{
    public Skill evadeAreaObject;
    public override bool StartActHaviour()
    {
        return EvadeArea();
    }

    private bool EvadeArea()
    {
        return ai.skillController.Trigger(evadeAreaObject);
    }
}
