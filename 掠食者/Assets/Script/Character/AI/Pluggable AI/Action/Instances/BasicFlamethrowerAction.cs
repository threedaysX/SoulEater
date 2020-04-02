using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Action/Flamethrower")]
public class BasicFlamethrowerAction : Action
{
    public Skill flamethrowerObject;
    public override void StartActHaviour()
    {
        Flame();
    }

    private void Flame()
    {
        ai.skillController.Trigger(flamethrowerObject);
    }
}
