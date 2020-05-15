using StatsModifierModel;
using UnityEngine;

public class ShockWave : DisposableSkill
{
    protected override void AddAffectEvent()
    {
        hitAffect.AddListener(DebuffSlowDown);
        hitAffect.AddListener(DebuffTired);
    }

    public string slow = "緩速";
    /// <summary>
    /// 對命中的敵人造成-50%移動速度，持續1秒
    /// </summary>
    private void DebuffSlowDown()
    {
        var speedstat = target.data.moveSpeed;
        void affect() { speedstat.AddModifier(new StatModifier(-0.5f, StatModType.Magnification, slow)); }
        void remove() { speedstat.RemoveModifier(new StatModifier(0.5f, StatModType.Magnification, slow)); }
        target.buffController.AddBuffEvent(slow, affect, remove, 1f);
    }

    public string tired = "疲累";
    /// <summary>
    /// 被命中的敵人無法使用跳躍，持續0.6秒
    /// </summary>
    private void DebuffTired()
    {
        void affect() { target.jump.Lock(); }
        void remove() { target.jump.UnLock(); }
        target.buffController.AddBuffEvent(tired, affect, remove, 0.6f);
    }

    public override void OnTriggerEnter2D(Collider2D target)
    {
        base.OnTriggerEnter2D(target);

        if (!target.CompareTag(sourceCaster.tag))
        {
            DamageTarget();
            InvokeAffect(hitAffect);
        }
    }
}
