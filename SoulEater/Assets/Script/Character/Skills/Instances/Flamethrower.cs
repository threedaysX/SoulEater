using UnityEngine;
using StatsModifierModel;
using System.Collections;

public class Flamethrower : LastingSkill
{
    public AudioClip renderingSound;
    public GameObject hintBackground;
    public ParticleSystem hintBeam;
    public string hintLineRenderAnimName;

    public override void OnTriggerStay2D(Collider2D target)
    {
        base.OnTriggerStay2D(target);

        #region 傷害階段
        if (canTriggerSelf)
        {
            DamageTarget();
        }
        else
        {
            if (!target.CompareTag(sourceCaster.tag))
            {
                if (Time.time >= nextDamageTime)
                {
                    DamageTarget();
                    InvokeHitAffect();
                    nextDamageTime = Time.time + currentSkill.timesOfPerDamage;
                }
            }
        }
        #endregion
    }

    protected override void AddAffectEvent()
    {
        immediatelyAffect.AddListener(LockDirectionTillEnd);
        immediatelyAffect.AddListener(KnockBackSelf);
        immediatelyAffect.AddListener(BuffFireResistance);
        immediatelyAffect.AddListener(CameraShakeWhenTrigger);
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
        var mod = new StatModifier(-20, StatModType.FlatAdd, buffName);
        void affect() { fireStat.AddModifier(mod); }
        void remove() { fireStat.RemoveModifier(mod); }
        sourceCaster.buffController.AddBuff(buffName, affect, remove, 5f);
    }

    public string debuffName = "烈焰崩毀";
    /// <summary>
    /// 減少目標20%火抗(+20%傷害)，持續5秒
    /// </summary>
    private void DebuffFireResistance()
    {
        var fireStat = target.data.resistance.fire;
        var mod = new StatModifier(20, StatModType.FlatAdd, debuffName);
        void affect() { fireStat.AddModifier(mod); }
        void remove() { fireStat.RemoveModifier(mod); }
        target.buffController.AddBuff(debuffName, affect, remove, 5f);
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

    public override void CastSkill()
    {
        base.CastSkill();
        RenderHint();
    }

    public override void UseSkill()
    {
        // Hint Stop.
        SetHintActive(false);
        base.UseSkill();
    }

    private void CameraShakeWhenTrigger()
    {
        CameraShake.Instance.ShakeCamera(0.8f, 1f, 3f, 0f, true);
    }

    private void RenderHint()
    {
        SetHintActive(true);
        soundControl.PlaySound(renderingSound);
    }

    private void SetHintActive(bool active)
    {
        hintBackground.SetActive(active);
        hintBeam.gameObject.SetActive(active);
        if (active)
            hintBeam.Play(true);
    }
}
