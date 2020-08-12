using StatsModel;
using UnityEngine;

public class DamageController : Singleton<DamageController>
{
    public float GetFinalDamageWithAttack(Character source, Character target, AttackType attackType, bool isSkill, out bool isCritical)
    {
        // 浮動值
        // 最終Atk or Matk = (該最終值 +- 武器碎片等級*(基礎力量or智力/5)*(基礎體質/8)*((靈巧/4)+基礎靈巧/2)) * 爆擊倍率
        float baseFloatDamage = 1;
        switch (attackType)
        {
            case AttackType.Attack:
                baseFloatDamage = source.data.status.strength.BaseValue;
                break;
            case AttackType.Magic:
                baseFloatDamage = source.data.status.intelligence.BaseValue;
                break;
        }
        float weaponFloat = 1 * (baseFloatDamage / 5) * (source.data.status.vitality.BaseValue / 8) * (source.data.status.dexterity.Value / 4 + source.data.status.dexterity.BaseValue);

        float basicDamage = source.GetAttackData(attackType).Value;
        float finalUpperDamage = (basicDamage + weaponFloat);
        float finalLowerDamage = (basicDamage - weaponFloat);
        float criticalMultiplier = GetCritical(source.data.critical.Value, source.data.criticalDamage.Value, source.data.status.lucky, target.data.status.lucky, isSkill, out isCritical);
        return GetFloatDamage(finalLowerDamage, finalUpperDamage) * criticalMultiplier; 
    }

    public float GetAttackDamage(Character source, Character target, AttackType attackType, ElementType attackElement, out bool isCritical)
    {
        if (target == null)
        {
            isCritical = false;
            return 0;
        }
        float targetBaseDefense = target.data.defense.BaseValue;
        float targetAddDefense = target.data.defense.Value - targetBaseDefense;
        float targetElementMagnification = target.GetResistance(attackElement).Value;

        // 待修: 其他倍率 (碎片加成 => 多種倍率相乘....)
        float otherMagnification = 1;

        // 實際造成傷害 = ((最終Atk - (加成防禦 + 穿甲值)) * (基礎防禦/100)% * 穿甲倍率) * 其他倍率 * 抗性表
        float resultDamage = ((GetFinalDamageWithAttack(source, target, attackType, false, out isCritical) - (targetAddDefense - source.data.penetrationValue.Value)) * (1 - targetBaseDefense / 100) * (1 - source.data.penetrationMagnification.Value / 100)) * otherMagnification * (targetElementMagnification / 100);
        return Mathf.Round(resultDamage);
    }

    public float GetSkillDamage(Character source, Character target, Skill currentSkill, out bool isCritical)
    {
        if (target == null)
        {
            isCritical = false;
            return 0;
        }
        float targetBaseDefense = target.data.defense.BaseValue;
        float targetAddDefense = target.data.defense.Value - targetBaseDefense;
        float targetElementMagnification = target.GetResistance(currentSkill.elementType).Value;

        // 待修: 其他倍率 (碎片加成 => 多種倍率相乘....)
        float otherMagnification = 1;

        // 實際技能傷害 = ((最終MAtk or Atk - (加成防禦 + 穿甲值))  * 技能倍率 * (基礎防禦/100)% * 穿甲倍率) * 其他倍率 * 抗性表
        float resultDamage = ((GetFinalDamageWithAttack(source, target, currentSkill.skillType, true, out isCritical) - (targetAddDefense - source.data.penetrationValue.Value)) * (currentSkill.damageMagnification.Value / 100) * (1 - targetBaseDefense / 100) * (1 - source.data.penetrationMagnification.Value / 100)) * otherMagnification * (targetElementMagnification / 100);

        return Mathf.Round(resultDamage);
    }

    private float GetFloatDamage(float upperLimitDamage, float lowerLimitDamage)
    {
        return Random.Range(lowerLimitDamage, upperLimitDamage);
    }

    private float GetCritical(float critical, float criticalDamage, Stats sourceLucky, Stats targetLucky, bool isSkill, out bool isCritical)
    {
        float criticalDamageTimes = 1;
        float randomDice = Random.Range(0, 100);
        float finalCriticalChance = critical - (2 * targetLucky.BaseValue - sourceLucky.BaseValue) - targetLucky.Value;

        // 魔法、治癒...可以爆擊，但機率為1/2。
        if (isSkill)
        {
            finalCriticalChance /= 2;
        }
        if (randomDice <= finalCriticalChance)
        {
            isCritical = true;
            criticalDamageTimes = criticalDamage / 100; // Percentage (%) change to Times
        }
        else
        {
            isCritical = false;
        }

        return criticalDamageTimes;
    }
}
