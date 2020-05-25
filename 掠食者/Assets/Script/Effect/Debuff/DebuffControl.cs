using UnityEngine;
using UnityEngine.Events;

public class DebuffControl : Singleton<DebuffControl>
{
    public const string igniteName = "Ignite";
    public ParticleSystem igniteEffect;

    public void Ignite(Character target, float duration)
    {
        ApplyDebuff(target, igniteName, delegate { IgniteEffect(target, duration); }, delegate { RemoveIgniteEffect(target); }, duration);
    }

    private void ApplyDebuff(Character target, string affectName, UnityAction applyAffect, UnityAction removeAffect, float duration)
    {
        target.buffController.AddBuffEvent(affectName, applyAffect, removeAffect, duration);
    }

    #region Ignite
    protected virtual void IgniteEffect(Character target, float duration)
    {
        var currentFireResis = target.data.resistance.fire;
        // 火抗小於等於50不會被燃燒。
        if (currentFireResis.Value <= 50)
        {
            return;
        }
        // 進入燃燒狀態，提升火抗至30。
        currentFireResis.ForceToChangeValue(50f);
        IgniteDamage(target, duration);
    }

    protected void IgniteDamage(Character target, float duration)
    {
        // Damage target per sec.
        float damagePerTimes = 1f;
        // Damage Formula.
        float damage = ((target.data.status.strength.BaseValue + 1) / 2) * ((target.data.status.intelligence.BaseValue) / 2) * (target.data.maxHealth.Value * 0.001f);
        // [false] => Ignite would not trigger critical damage and damage target immediately.
        target.TakeDamage((int)damage, false, 0, 0, damagePerTimes, duration, false);
    }

    protected virtual void RemoveIgniteEffect(Character target)
    {
        var currentFireResis = target.data.resistance.fire;
        currentFireResis.CancelForceValue();
    }
    #endregion
}
