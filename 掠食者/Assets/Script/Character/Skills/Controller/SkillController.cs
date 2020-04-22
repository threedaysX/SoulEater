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
        // 技能冷卻中
        if (skill.cooling)
        {
            Debug.Log("冷卻中！！");
            return false;
        }

        // 消耗
        switch (skill.costType)
        {
            case CostType.Health:
                if (character.currentHealth < skill.cost)
                {
                    Debug.Log("沒有生命啦！");
                    return false;
                }
                character.currentHealth -= skill.cost;
                break;
            case CostType.Mana:
                if (character.currentMana < skill.cost)
                {
                    Debug.Log("沒有魔力啦！");
                    return false;
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
        return true;
    }

    /// <summary>
    /// 施放技能，是否可操作的控制切換。
    /// </summary>
    protected void Casting(Character character, float castTime, bool canDo)
    {
        character.operationController.canSkill = canDo;
        character.operationController.canMove = canDo;
        character.operationController.canJump = canDo;
        character.operationController.canEvade = canDo;
        character.operationController.canAttack = canDo;

        float timer = 0;
        while (timer < castTime)
        {
            timer += Time.deltaTime;

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
