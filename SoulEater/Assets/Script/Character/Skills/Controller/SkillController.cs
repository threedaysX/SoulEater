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

    public bool Trigger(Skill skill, bool ignoreCoolDown)
    {
        // 技能為空
        if (skill.prefab == null)
        {
            return false;
        }

        // 技能冷卻中
        if (skill.cooling && !ignoreCoolDown)
        {
            return false;
        }

        // 消耗
        switch (skill.costType)
        {
            case CostType.Health:
                if (character.CurrentHealth < skill.cost.Value)
                {
                    return false;
                }
                character.CurrentHealth -= (int)skill.cost.Value;
                break;
            case CostType.Mana:
                if (character.CurrentMana < skill.cost.Value)
                {
                    return false;
                }
                character.CurrentMana -= skill.cost.Value;
                break;
        }

        // 詠唱，固定詠唱時間+ 結束後施放技能，持續N秒。
        float totalCastTime = GetCastTime(skill);
        Vector3 skillCenterPos = skillCenterPoint.position != null ? skillCenterPoint.position : character.transform.position;
        Vector3 skillPos = skillCenterPos + character.transform.right * skill.centerPositionOffset;
        GameObject skillObj = SkillPools.Instance.SpawnSkillFromPool(character, skill, skillPos, character.transform.rotation);
        if (skillObj != null)
        {
            character.operationController.StartUseSkillAnim(StartCastSkill(skill, totalCastTime, skillObj), StartUseSkill(skill, skillObj), totalCastTime, skill.duration);
        }

        this.lastSkill = skill;
        return true;
    }

    private Action StartCastSkill(Skill skill, float castTime, GameObject skillObj)
    {
        if (castTime <= 0)
            return null;

        // 技能施放冷卻計算
        if (skill.coolDownType == SkillCoolDownType.CoolInCast)
            StartCoroutine(GetIntoCoolDown(skill));

        ISkillCaster skillCaster = skillObj.GetComponent<ISkillCaster>();
        return delegate { skillCaster.CastSkill(); };
    }

    private Action StartUseSkill(Skill skill, GameObject skillObj)
    {
        // 技能施放冷卻計算
        if (skill.coolDownType == SkillCoolDownType.CoolAfterUse)
            StartCoroutine(GetIntoCoolDown(skill));

        ISkillUse skilluse = skillObj.GetComponent<ISkillUse>();
        return delegate { skilluse.UseSkill(); };
    }

    protected float GetCastTime(Skill skill)
    {
        return skill.fixedCastTime.Value + skill.castTime.Value * (1 - character.data.reduceCastTime.Value / 100);
    }

    protected IEnumerator GetIntoCoolDown(Skill skill)
    {
        skill.cooling = true;
        skill.trueCoolDown = skill.coolDown.Value * (1 - character.data.reduceSkillCoolDown.Value / 100);
        skill.coolDownTimer = skill.trueCoolDown;
        while (skill.coolDownTimer > 0)
        {
            var frameTime = Time.deltaTime;
            skill.coolDownTimer -= frameTime;
            yield return new WaitForSeconds(frameTime);
        }

        skill.coolDownTimer = 0;
        skill.cooling = false;
    }

    public void ResetAllSkillCoolDown()
    {
        if (character.skillFields == null)
            return;
        foreach (var skill in character.skillFields)
        {
            skill.cooling = false;
            skill.coolDownTimer = 0;
        }
    }
}
