﻿using System.Collections;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public Character character;
    public Skill lastSkill;

    private void Start()
    {
        character = GetComponent<Character>();
        ResetAllSkillCoolDownOnStart();
    }

    public void Trigger(Skill skill)
    {
        // 技能冷卻中
        if (skill.cooling)
        {
            Debug.Log("冷卻中！！");
            return;
        }

        // 消耗
        switch (skill.costType)
        {
            case CostType.Health:
                if (character.currentHealth < skill.cost)
                {
                    Debug.Log("沒有生命啦！");
                    return;
                }
                character.currentHealth -= skill.cost;
                break;
            case CostType.Mana:
                if (character.currentMana < skill.cost)
                {
                    Debug.Log("沒有魔力啦！");
                    return;
                }
                character.currentMana -= skill.cost;
                break;
        }

        // 詠唱
        if (skill.castTime > 0)
        {
            Casting(character, skill.castTime, false);
        }
        
        if (skill.prefab != null)
        {
            var skillEvent = skill.prefab.GetComponent<SkillEventBase>();
            skillEvent.InstantiateSkill(character, skill);
        }

        StartCoroutine(GetIntoCoolDown(skill));
        this.lastSkill = skill;
    }

    /// <summary>
    /// 施放技能，是否可操作的控制切換。
    /// </summary>
    protected void Casting(Character character, float castTime, bool canDo)
    {
        character.canSkill = canDo;
        character.canMove = canDo;
        character.canJump = canDo;
        character.canEvade = canDo;
        character.canAttack = canDo;

        float timer = 0;
        while (timer < castTime)
        {
            var frameTime = Time.deltaTime;
            timer += frameTime;

            // Render Casting GUI
        }
    }

    protected IEnumerator GetIntoCoolDown(Skill skill)
    {
        skill.cooling = true;
        float timer = skill.coolDown;
        while (timer > 0)
        {
            var frameTime = Time.deltaTime;
            timer -= frameTime;
            yield return new WaitForSeconds(frameTime);
        }

        skill.cooling = false;
    }

    private void ResetAllSkillCoolDownOnStart()
    {
        foreach (var skill in character.skills)
        {
            skill.cooling = false;
        }
    }
}
