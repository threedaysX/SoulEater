using UnityEngine;
using StatsModifierModel;
using System.Collections;

public class Flamethrower : LastingSkill
{
    protected override void AddAffectEvent()
    {
        immediatelyAffect.AddListener(LockDirectionTillEnd);
        immediatelyAffect.AddListener(KnockBackSelf);
        immediatelyAffect.AddListener(BuffFireResistance);
        hitAffect.AddListener(KnockBackEnemy);
        hitAffect.AddListener(DebuffFireResistance);
    }

    public string buffName = "烈焰鎧甲";
    /// <summary>
    /// 增加自身+20%火抗(-20%傷害)，持續5秒
    /// </summary>
    private void BuffFireResistance()
    {
        var fireStat = sourceCaster.data.resistance.fire;
        void affect() { fireStat.AddModifier(new StatModifier(-20, StatModType.FlatAdd, buffName)); }
        void remove() { fireStat.RemoveModifier(new StatModifier(-20, StatModType.FlatAdd, buffName)); }
        sourceCaster.buffController.AddBuffEvent(buffName, affect, remove, 5f);
    }

    public string debuffName = "烈焰崩毀";
    /// <summary>
    /// 減少目標20%火抗(+20%傷害)，持續5秒
    /// </summary>
    private void DebuffFireResistance()
    {
        var fireStat = target.data.resistance.fire;
        void affect() { fireStat.AddModifier(new StatModifier(20, StatModType.FlatAdd, debuffName)); }
        void remove() { fireStat.RemoveModifier(new StatModifier(20, StatModType.FlatAdd, debuffName)); }
        target.buffController.AddBuffEvent(debuffName, affect, remove, 5f);
    }

    /// <summary>
    /// 擊退效果
    /// </summary>
    private void KnockBackSelf()
    {
        StartCoroutine(KnockBackCoroutine(sourceCaster, -0.6f * sourceCaster.data.moveSpeed.Value, currentSkill.duration));
    }
    private void KnockBackEnemy()
    {
        StartCoroutine(KnockBackCoroutine(target, 0.15f * target.data.moveSpeed.Value, currentSkill.duration));
    }
    private IEnumerator KnockBackCoroutine(Character target, float knockbackforce, float duration)
    {
        if (sourceCaster == null || target == null)
            yield break;

        Vector3 directionForce = sourceCaster.transform.right * knockbackforce;
        float timeleft = duration;
        while (timeleft > 0)
        {
            if (target == null)
                yield break;
            if (timeleft > Time.deltaTime)
                target.transform.position += (directionForce * Time.deltaTime / duration);
            else
                target.transform.position += (directionForce * timeleft / duration);

            timeleft -= Time.deltaTime;
            yield return null;
        }
    }
}
