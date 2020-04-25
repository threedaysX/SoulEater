using StatsModel;
using UnityEngine;

public class DamageController : Singleton<DamageController>
{
    public bool IsCritical { get; set; }
    public float GetFinalDamageWithAttack(Character source, Character target, AttackType attackType, bool isSkill)
    {
        IsCritical = false;

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
        return GetFloatDamage(finalLowerDamage, finalUpperDamage) * GetCritical(source.data.critical.Value, source.data.criticalDamage.Value, source.data.status.lucky, target.data.status.lucky, isSkill); 
    }

    public float GetAttackDamage(Character source, Character target, AttackType attackType, ElementType attackElement)
    {
        if (target == null)
            return 0;
        float targetBaseDefense = target.data.defense.BaseValue;
        float targetAddDefense = target.data.defense.Value - targetBaseDefense;
        float targetElementMagnification = target.GetResistance(attackElement).Value;

        // 待修: 其他倍率 (碎片加成 => 多種倍率相乘....)
        float otherMagnification = 1;

        // 實際造成傷害 = (最終Atk * (基礎防禦/100)% * 穿甲倍率 - (加成防禦 + 穿甲值)) * 其他倍率 * 抗性表
        float resultDamage = (GetFinalDamageWithAttack(source, target, attackType, false) * (1 - targetBaseDefense / 100) * (1 - source.data.penetrationMagnification.Value / 100) - (targetAddDefense - source.data.penetrationValue.Value)) * otherMagnification * (targetElementMagnification / 100);
        return Mathf.Round(resultDamage);
    }

    public float GetSkillDamage(Character source, Character target, Skill currentSkill)
    {
        if (target == null)
            return 0;
        float targetBaseDefense = target.data.defense.BaseValue;
        float targetAddDefense = target.data.defense.Value - targetBaseDefense;
        float targetElementMagnification = target.GetResistance(currentSkill.elementType).Value;

        // 待修: 其他倍率 (碎片加成 => 多種倍率相乘....)
        float otherMagnification = 1;

        // 實際技能傷害 = (最終MAtk or Atk  * 技能倍率 * (基礎防禦/100)% * 穿甲倍率 - (加成防禦 + 穿甲值)) * 其他倍率 * 抗性表
        float resultDamage = (GetFinalDamageWithAttack(source, target, currentSkill.skillType, true) * (currentSkill.damageMagnification / 100) * (1 - targetBaseDefense / 100) * (1 - source.data.penetrationMagnification.Value / 100) - (targetAddDefense - source.data.penetrationValue.Value)) * otherMagnification * (targetElementMagnification / 100);

        return Mathf.Round(resultDamage);
    }

    private float GetFloatDamage(float upperLimitDamage, float lowerLimitDamage)
    {
        return Random.Range(lowerLimitDamage, upperLimitDamage);

    }

    private float GetCritical(float critical, float criticalDamage, Stats sourceLucky, Stats targetLucky, bool isSkill)
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
            IsCritical = true;
            criticalDamageTimes = criticalDamage / 100; // Percentage (%) change to Times
        }

        return criticalDamageTimes;
    }
}
