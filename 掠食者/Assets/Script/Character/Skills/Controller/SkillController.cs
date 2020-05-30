using System;
using System.Collections;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public Transform skillCenterPoint;  // 施放技能基準點
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
                character.CurrentHealth -= (int)skill.cost;
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
        Vector3 skillPoint = skillCenterPoint.position != null ? skillCenterPoint.position : character.transform.position;
        Vector3 position = skillPoint + character.transform.right * skill.centerPositionOffset;
        var skillObj = SkillPools.Instance.SpawnSkillFromPool(character, skill, position, character.transform.rotation);
        if (skillObj != null)
        {
            character.operationController.StartUseSkillAnim(StartCastSkill(skill, skillObj), StartUseSkill(skillObj), castTime, skill.duration);
        }

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
