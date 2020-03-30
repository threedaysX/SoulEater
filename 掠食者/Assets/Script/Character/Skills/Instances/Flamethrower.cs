using UnityEngine;
using StatsModifierModel;
using System.Collections;

public class Flamethrower : SkillEventBase
{
    protected override void AddAffectEvent()
    {
        immediatelyAffect.AddListener(KnockBackSelf);
        immediatelyAffect.AddListener(BuffFireResistance);
        hitAffect.AddListener(KnockBackEnemy);
        hitAffect.AddListener(DebuffFireResistance);
    }

    public string buffName = "烈焰鎧甲";
    /// <summary>
    /// 增加自身+20%火炕，持續5秒
    /// </summary>
    private void BuffFireResistance()
    {
        sourceCaster.buffController.AddModifier(sourceCaster.data.resistance.fire, new StatModifier(20, StatModType.FlatAdd, buffName), 5f);
    }

    public string debuffName = "烈焰崩毀";
    /// <summary>
    /// 減少目標20%火炕，持續5秒
    /// </summary>
    private void DebuffFireResistance()
    {
        target.buffController.AddModifier(target.data.resistance.fire, new StatModifier(-20, StatModType.FlatAdd, debuffName), 5f);
    }

    /// <summary>
    /// 擊退效果
    /// </summary>
    private void KnockBackSelf()
    {
        StartCoroutine(KnockBackCoroutine(sourceCaster, -0.5f * sourceCaster.data.moveSpeed.Value, currentSkill.duration));
    }
    private void KnockBackEnemy()
    {
        StartCoroutine(KnockBackCoroutine(target, -2f * target.data.moveSpeed.Value, currentSkill.duration));
    }
    private IEnumerator KnockBackCoroutine(Character target, float knockbackforce, float duration)
    {
        // knockbackforce => 向後退移，所以力道為負值。
        Vector3 directionForce = target.transform.right * knockbackforce;
        Vector3 direction = target.transform.rotation * directionForce;
        float timeleft = 3f; //currentSkill.duartion;
        while (timeleft > 0)
        {
            if (timeleft > Time.deltaTime)
                target.transform.Translate(direction * Time.deltaTime / duration);
            else
                target.transform.Translate(direction * timeleft / duration);

            timeleft -= Time.deltaTime;
            yield return null;
        }
        sourceCaster.transform.position -= new Vector3(sourceCaster.data.moveSpeed.Value * Time.deltaTime, 0, 0);
    }
}
