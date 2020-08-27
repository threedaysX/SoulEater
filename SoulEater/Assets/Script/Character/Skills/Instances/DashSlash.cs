using System;
using System.Collections;
using UnityEngine;

public class DashSlash : DisposableSkill
{
    public ParticleSystem chargeTrailEffect; // 衝刺特效
    public ParticleSystem drawnSwordEffect; // 拔劍特效
    public ParticleSystem slashHitEffect;   // 斬擊特效
    public AudioClip slashHitSound; // 斬擊音效
    private bool isHit;

    public override void CastSkill()
    {
        base.CastSkill();

        sourceCaster.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        drawnSwordEffect.transform.position = sourceCaster.transform.position + sourceCaster.transform.right * 0.5f;
        drawnSwordEffect.Play(true);
        GetImmune();
    }

    protected override void AddAffectEvent()
    {
        immediatelyAffect.AddListener(GetInDarkScreenAndZoomIn);
        immediatelyAffect.AddListener(MoveFoward);
        immediatelyAffect.AddListener(delegate { StartCoroutine(HitDetect()); });
        //immediatelyAffect.AddListener(AddBuffs);
    }

    public override void OnTriggerEnter2D(Collider2D targetCol)
    {
        base.OnTriggerEnter2D(targetCol);

        if (!targetCol.CompareTag(sourceCaster.tag))
        {
            isHit = true;
            Character target = targetCol.GetComponent<Character>();
            this.target = target;
            BindEnemyAction(target, 1.8f);
            DamageTarget(1f);
        }
    }

    protected override bool Damage(float damageDirectionX = 0)
    {
        TriggerHitEffect(target);
        CameraShakeWhenHit();
        return base.Damage(transform.right.x);
    }

    private void TriggerHitEffect(Character target)
    {
        if (target == null)
            return;
        slashHitEffect.transform.position = target.transform.position;
        slashHitEffect.Play(true);
        soundControl.PlaySound(slashHitSound);
    }

    //private void AddBuffs()
    //{
    //    // 【自身每扣100血，會再額外受到10點傷害】
    //    void Affect() { sourceCaster.TakeDamage(new DamageData(ElementType.None, 10)); }
    //    // 【效果移除時，自身恢復100hp】
    //    void EndAffect() { sourceCaster.CurrentHealth += 100f; }
    //    sourceCaster.damageStoreController.AddDamageStoreData(TestFor100Hp, dscType, 0);
    //    // 效果持續5秒，持續期間 【自身每扣100血，會再額外受到10點傷害】
    //    sourceCaster.buffController.AddBuff(TestFor100Hp, Affect, EndAffect, 5f, delegate { return TriggerPer100Health(); });
    //}

    //// 儲存傷害用
    //public const string TestFor100Hp = "TestFor100Hp";
    //public DamageStoreType dscType = DamageStoreType.Take;
    //private bool TriggerPer100Health()
    //{
    //    var dsc = sourceCaster.damageStoreController;
    //    int cumulativeDamageTake = dsc.GetDamageData(TestFor100Hp, dscType);
    //    if (cumulativeDamageTake >= 100)
    //    {
    //        dsc.ModifyDamageData(TestFor100Hp, dscType, 0);
    //        return true;
    //    }
    //    return false;
    //}

    // 使用技能後，立即鎖定敵人動作
    private void BindEnemyAction(Character target, float duration)
    {
        Debuff.Instance.Bind(target, duration);
    }

    // 詠唱技能時，就立即進入無敵狀態
    private void GetImmune()
    {
        sourceCaster.GetIntoImmune(1f);
    }

    // 使用技能後，立即進入模糊畫面與畫面特寫
    private void GetInDarkScreenAndZoomIn()
    {
        //StartCoroutine(FadeScreen.Instance.Fade(1f, 1f));
        ZoomInSetting zoomInSetting = new ZoomInSetting { finalZoomSize = 5.5f, duration = 0.1f, startDelay = 0f };
        ZoomInSetting resetCameraSetting = new ZoomInSetting { finalZoomSize = 6f, duration = 0.5f, startDelay = 0.6f };
        CinemachineCameraControl.Instance.ZoomInCamera(zoomInSetting, resetCameraSetting);

        //// Old Blur for Post-Processing.
        //ImageEffectController.Instance.StartRadialBlur(DG.Tweening.Ease.Linear,
        //        new[] {
        //            new RadialBlurSetting { strength = 2f, dist = 1f, duration = 0.3f },
        //            new RadialBlurSetting { strength = 0f, dist = 1f, duration = 0.2f }  });
    }

    /// <summary>
    /// 0.1秒內瞬移到前方6m處。	
    /// </summary>
    private void MoveFoward()
    {
        StartCoroutine(MoveToPosition(sourceCaster.transform
            , sourceCaster.transform.position + sourceCaster.transform.right * currentSkill.range.Value
            , 0.1f));
    }
    public IEnumerator MoveToPosition(Transform transform, Vector3 destination, float timeToMove)
    {
        var originPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            // 讓技能跟隨玩家，並偵測是否命中
            this.transform.position = sourceCaster.transform.position + sourceCaster.transform.right * 0.8f;
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(originPos, destination, t);
            yield return null;
        }

        chargeTrailEffect.Play(true);
    }

    private IEnumerator HitDetect()
    {
        yield return new WaitForSeconds(currentSkill.castTime.Value + 0.1f);
        if (isHit)
        {
            StartCoroutine(SetActiveAfterSkillDone(1.9f));
        }
        else
        {
            SetActiveAfterSkillDone(false);
        }
    }

    private void CameraShakeWhenHit()
    {
        CameraShake.Instance.ShakeCamera(1f, 8f, 0.1f, 0f, true);
    }
}