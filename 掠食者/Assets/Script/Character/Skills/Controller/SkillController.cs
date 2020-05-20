using System;
using System.Collections;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public Character character;
    public Skill lastSkill;

    private void Start()
    {
        character = GetComponent<Character>();
        ResetAllSkillCoolDown();
    }

    public bool Trigger(Skill skill)
    {
        // 技能為空
        if (skill.prefab == null)
        {
            return false;
        }

        // 技能冷卻中
        if (skill.cooling)
        {
            return false;
        }

        // 消耗
        switch (skill.costType)
        {
            case CostType.Health:
                if (character.CurrentHealth < skill.cost)
                {
                    return false;
                }
                character.CurrentHealth -= skill.cost;
                break;
            case CostType.Mana:
                if (character.CurrentMana < skill.cost)
                {
                    return false;
                }
                character.CurrentMana -= skill.cost;
                break;
        }

        // 詠唱，結束後施放技能，持續N秒。
        float castTime = skill.castTime * (1 - character.data.reduceCastTime.Value / 100);
        Vector3 position = character.transform.position + character.transform.right * skill.range;
        var skillObj = SkillPools.Instance.SpawnSkillFromPool(character, skill, position, character.transform.rotation);
        character.operationController.StartUseSkillAnim(StartCastSkill(skill, skillObj), StartUseSkill(skillObj), castTime, skill.duration);

        // 技能施放後就直接計算冷卻
        StartCoroutine(GetIntoCoolDown(skill));
        this.lastSkill = skill;
        return true;
    }

    private Action StartCastSkill(Skill skill, GameObject skillObj)
    {
        if (skill.castTime <= 0)
            return null;

        ISkillCaster skillCaster = skillObj.GetComponent<ISkillCaster>();
        return delegate { skillCaster.CastSkill(); };
    }

    private Action StartUseSkill(GameObject skillObj)
    {
        ISkillUse skilluse = skillObj.GetComponent<ISkillUse>();
        return delegate { skilluse.UseSkill(); };
    }

    protected IEnumerator GetIntoCoolDown(Skill skill)
    {
        skill.cooling = true;
        float timer = skill.coolDown * (1 - character.data.reduceSkillCoolDown.Value / 100);
        while (timer > 0)
        {
            var frameTime = Time.deltaTime;
            timer -= frameTime;
            yield return new WaitForSeconds(frameTime);
        }

        skill.cooling = false;
    }

    private void ResetAllSkillCoolDown()
    {
        if (character.skillFields == null)
            return;
        foreach (var field in character.skillFields)
        {
            field.cooling = false;
        }
    }
}
