public class DamageController : Singleton<DamageController>
{
    public float GetPhysicalDamage()
    {
        return 0;
    }

    public float GetSkillDamage(Character source, Character target, Skill currentSkill)
    {
        float targetBaseDefense = target.data.defense.BaseValue;
        float targetAddDefense = target.data.defense.Value - targetBaseDefense;
        float targetElementMagnification = target.GetResistance(currentSkill.elementType).Value;
        float casterAtk = source.GetAttack(currentSkill.skillType).Value;

        // 待修: 其他倍率 (碎片加成、爆擊倍率 => 多種倍率相乘....)
        float otherMagnification = source.data.criticalDamage.Value;

        // 實際技能傷害 = (最終MAtk or Atk  * 技能倍率 * (基礎防禦/100)% * 穿甲倍率 - (加成防禦 + 穿甲值)) * 其他倍率 * 抗性表
        float resultDamage = (casterAtk * currentSkill.damageMagnification * (1 - targetBaseDefense / 100) * (1 - source.data.penetrationMagnification.Value / 100) - (targetAddDefense - source.data.penetrationValue.Value)) * (otherMagnification / 100) * (targetElementMagnification / 100);

        return resultDamage;
    }
}
