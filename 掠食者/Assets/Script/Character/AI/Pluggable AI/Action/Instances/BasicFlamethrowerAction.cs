using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Action/Flamethrower")]
public class BasicFlamethrowerAction : Action
{
    public Skill flamethrowerObject;
    public override bool StartActHaviour()
    {
        return Flame();
    }

    private bool Flame()
    {
        return ai.skillController.Trigger(flamethrowerObject);
    }
}
