using UnityEngine;

public abstract class SkillAiAction : AiAction
{
    [Header("技能Prefab")]
    public Skill skillActionObject;

    public override bool StartActHaviour()
    {
        return TriggerSkill();
    }

    protected virtual bool TriggerSkill()
    {
        return ai.UseSkill(skillActionObject);
    }
}
