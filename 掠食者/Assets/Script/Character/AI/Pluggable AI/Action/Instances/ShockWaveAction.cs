using UnityEngine;

[CreateAssetMenu(menuName = "Character/AI/Action/ShockWave")]
public class ShockWaveAction : Action
{
    public Skill shockWaveActionObject;
    public override bool StartActHaviour()
    {
        return ShockWave();
    }

    private bool ShockWave()
    {
        return ai.skillController.Trigger(shockWaveActionObject);
    }
}
