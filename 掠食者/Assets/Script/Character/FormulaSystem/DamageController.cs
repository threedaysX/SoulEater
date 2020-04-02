public class DamageController : Singleton<DamageController>
{
    public float GetFinalDamageWithAttack(Character source, AttackType attackType)
    {
        // 浮動值
        // 最終Atk or Matk = 最終值 +- 武器碎片等級*(基礎力量/10)
        return source.GetAttackData(attackType).Value; 
    }

    public float GetAttackDamage(Character source, Character target, AttackType attackType, ElementType attackElement)
    {
        if (target == null)
            return 0;
        float targetBaseDefense = target.data.defense.BaseValue;
        float targetAddDefense = target.data.defense.Value - targetBaseDefense;
        float targetElementMagnification = target.GetResistance(attackElement).Value;

        float otherMagnification = source.data.criticalDamage.Value;

        // 實際造成傷害 = (最終Atk * (基礎防禦/100)% * 穿甲倍率 - (加成防禦 + 穿甲值)) * 其他倍率 * 抗性表
        return (GetFinalDamageWithAttack(source, attackType) * (1 - targetBaseDefense / 100) * (1 - source.data.penetrationMagnification.Value / 100) - (targetAddDefense - source.data.penetrationValue.Value)) * 1 * (targetElementMagnification / 100);
    }

    public float GetSkillDamage(Character source, Character target, Skill currentSkill)
    {
        if (target == null)
            return 0;
        float targetBaseDefense = target.data.defense.BaseValue;
        float targetAddDefense = target.data.defense.Value - targetBaseDefense;
        float targetElementMagnification = target.GetResistance(currentSkill.elementType).Value;

        float casterAtk = source.GetAttackData(currentSkill.skillType).Value;

        // 待修: 其他倍率 (碎片加成、爆擊倍率 => 多種倍率相乘....)
        float otherMagnification = source.data.criticalDamage.Value;

        // 實際技能傷害 = (最終MAtk or Atk  * 技能倍率 * (基礎防禦/100)% * 穿甲倍率 - (加成防禦 + 穿甲值)) * 其他倍率 * 抗性表
        float resultDamage = (casterAtk * (currentSkill.damageMagnification / 100) * (1 - targetBaseDefense / 100) * (1 - source.data.penetrationMagnification.Value / 100) - (targetAddDefense - source.data.penetrationValue.Value)) * 1 * (targetElementMagnification / 100);

        return resultDamage;
    }
}
