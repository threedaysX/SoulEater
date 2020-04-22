using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Action/CrackSlam")]
public class CrackSlamAction : Action
{
    public Skill crackSlamActionObject;
    public override bool StartActHaviour()
    {
        return CrackSlam();
    }

    private bool CrackSlam()
    {
        return ai.skillController.Trigger(crackSlamActionObject);
    }
}
