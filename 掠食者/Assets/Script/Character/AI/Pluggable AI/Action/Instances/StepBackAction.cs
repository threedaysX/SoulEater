using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Action/StepBack")]
public class StepBackAction : Action
{
    public Skill stepBackObject;
    public override bool StartActHaviour()
    {
        return StepBack();
    }

    private bool StepBack()
    {
        return ai.skillController.Trigger(stepBackObject);
    }
}
