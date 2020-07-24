using StatsModifierModel;
using UnityEngine;

public class PowerSlam : DisposableSkill
{
    protected override void AddAffectEvent()
    {
        hitAffect.AddListener(DebuffArmorBreak);
    }

    public string armorBreak = "破甲LV1";
    /// <summary>
    /// 破甲Lv1: 對命中的敵人造成-10%基礎防禦，持續4秒。			
    /// </summary>
    private void DebuffArmorBreak()
    {
        var armorstat = target.data.defense;
        void affect() { armorstat.AddModifier(new StatModifier(-10, StatModType.MagnificationAdd, armorBreak)); }
        void remove() { armorstat.RemoveModifier(new StatModifier(10, StatModType.MagnificationAdd, armorBreak)); }
        target.buffController.AddBuff(armorBreak, affect, remove, 4f);
    }

    public override void OnTriggerEnter2D(Collider2D targetCol)
    {
        base.OnTriggerEnter2D(targetCol);

        if (!targetCol.CompareTag(sourceCaster.tag))
        {
            DamageTarget();
            InvokeAffect(hitAffect);
        }
    }
}

     
