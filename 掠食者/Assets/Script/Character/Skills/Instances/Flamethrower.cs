using UnityEngine;
using StatsModifierModel;
using System.Collections;

public class Flamethrower : LastingSkill
{
    public AudioClip renderingSound;
    public GameObject hintBackground;
    public GameObject hintLine;
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
                    InvokeAffect(hitAffect);
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

    public override void CastSkill()
    {
        base.CastSkill();

        RenderHint();
    }

    private void CameraShakeWhenTrigger()
    {
        CameraShake.Instance.ShakeCamera(0.8f, 1f, 3f, true);
    }

    private void RenderHint()
    {
        SetHintActive(true);
        soundControl.PlaySound(renderingSound);
        AnimationBase.Instance.PlayAnimationLoop(hintLine.GetComponent<Animator>(), hintLineRenderAnimName, currentSkill.castTime, delegate { SetHintActive(false); });
    }

    private void SetHintActive(bool active)
    {
        hintBackground.SetActive(active);
        hintLine.SetActive(active);
    }

    #region Render Line Hint Old
    //public GameObject[] hintLines;

    //private IEnumerator RenderHintLine()
    //{
    //    foreach (var line in hintLines)
    //    {
    //        line.SetActive(true);
    //        line.transform.localScale = new Vector3(0, 0.01f, 0);
    //    }

    //    sound.PlayOneShot(renderingSound);
    //    StartCoroutine(StretchHintLine(hintLines[0], new Vector3(10f, hintLines[0].transform.localScale.y), 0.1f));
    //    yield return new WaitForSeconds(0.1f);

    //    sound.PlayOneShot(renderingSound);
    //    StartCoroutine(StretchHintLine(hintLines[1], new Vector3(10f, hintLines[1].transform.localScale.y), 0.1f));
    //    yield return new WaitForSeconds(0.1f);

    //    sound.PlayOneShot(renderingSound);
    //    StartCoroutine(StretchHintLine(hintLines[2], new Vector3(10f, hintLines[2].transform.localScale.y), 0.1f));
    //    yield return new WaitForSeconds(0.2f);

    //    sound.PlayOneShot(renderingSound);
    //    StartCoroutine(StretchHintLine(hintLines[3], new Vector3(10f, hintLines[3].transform.localScale.y), 0.1f));
    //    yield return new WaitForSeconds(0.1f);

    //    yield return new WaitForSeconds(currentSkill.castTime - 0.5f);
    //    foreach (var line in hintLines)
    //    {
    //        line.SetActive(false);
    //    }
    //}

    //private IEnumerator StretchHintLine(GameObject hintLine, Vector3 targetScale, float duration)
    //{
    //    Vector3 originScale = hintLine.transform.localScale;
    //    var t = 0f;
    //    while (t < 1)
    //    {
    //        t += Time.deltaTime / duration;
    //        hintLine.transform.localScale = Vector3.Lerp(originScale, targetScale, t);
    //        yield return null;
    //    }
    //}
    #endregion
}
