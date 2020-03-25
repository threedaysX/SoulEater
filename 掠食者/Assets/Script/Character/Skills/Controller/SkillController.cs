using UnityEngine;
using UnityEngine.Events;

public class SkillController : MonoBehaviour
{
    protected Skill skill;

    public void Trigger(Character character, Skill usedSkill)
    {
        // 哪個技能
        skill = usedSkill;

        // 沒魔力了
        if (character.currentMana < skill.cost)
        {
            Debug.Log("沒有魔力啦！");
            return;
        }
        // 技能冷卻中
        //if (skill.coolDown...)
        //{
        //    return;
        //}


        // 補冷卻、詠唱加進Event? 分開?
        // 施放後，把技能的Event在此時加進去
        character.currentMana -= skill.cost;
        if (skill.animator != null)
            skill.animator.Play(skill.skillName);
        skill.InvokeEffect(character);
    }

    /// <summary>
    /// 施放技能時，不能操作。
    /// </summary>
    public void CastingSkill()
    {

    }

    public void GetIntoCoolDown()
    {

    }

    public void AddEffect(UnityAction call)
    {
        skill.effect.AddListener(call);
    }
}
