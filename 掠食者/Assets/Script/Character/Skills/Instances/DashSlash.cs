using System.Collections;
using UnityEngine;

public class DashSlash : DisposableSkill
{
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
    }

    protected override void AddAffectEvent()
    {
        immediatelyAffect.AddListener(GetInDarkScreen);
        immediatelyAffect.AddListener(GetImmune);
        immediatelyAffect.AddListener(MoveFoward);
        immediatelyAffect.AddListener(delegate { StartCoroutine(HitDetect()); });
    }

    public override void OnTriggerEnter2D(Collider2D targetCol)
    {
        base.OnTriggerEnter2D(targetCol);

        if (!targetCol.CompareTag(sourceCaster.tag))
        {
            isHit = true;
            Character target = targetCol.GetComponent<Character>();
            StartCoroutine(DamageTargetCoroutine(target));
        }
    }

    // 實際造成傷害的方式
    private IEnumerator DamageTargetCoroutine(Character target)
    {
        StartCoroutine(LockEnemyAction(target, 1.8f));
        yield return new WaitForSeconds(1f);

        // 第1段攻擊
        DamageTarget(target);
        yield return new WaitForSeconds(0.1f);

        // 第2段攻擊
        DamageTarget(target);
        yield return new WaitForSeconds(0.1f);

        // 第3段攻擊
        DamageTarget(target);
        yield return new WaitForSeconds(0.1f);

        // 第4段攻擊
        DamageTarget(target);
        yield return new WaitForSeconds(0.1f);

        // 第5段攻擊
        DamageTarget(target);
        yield return new WaitForSeconds(0.1f);

        // 第6段攻擊
        DamageTarget(target);
        yield return new WaitForSeconds(0.1f);

        // 第7段攻擊
        DamageTarget(target);
        yield return new WaitForSeconds(0.2f);
    }

    protected void DamageTarget(Character target)
    {
        if (target == null)
            return;
        slashHitEffect.transform.position = target.transform.position;
        slashHitEffect.Play(true);
        soundControl.PlaySound(slashHitSound);
        base.DamageTarget(transform.right.x);
        CameraShakeWhenHit();
    }

    // 使用技能後，立即鎖定敵人動作
    private IEnumerator LockEnemyAction(Character target, float duration)
    {
        target.SetOperation(LockType.SkillAction, false);
        yield return new WaitForSeconds(duration);
        target.SetOperation(LockType.SkillAction, true);
    }

    // 使用技能後，立即進入無敵狀態
    private void GetImmune()
    {
        sourceCaster.GetIntoImmune(1f);
    }

    // 使用技能後，立即進入短暫黑畫面與畫面特寫
    private void GetInDarkScreen()
    {
        StartCoroutine(GetInDarkScreen(0.3f));
        ZoomInSetting zoomInSetting = new ZoomInSetting { finalZoomSize = 3.2f, duration = 0.2f, afterDelay = 0.8f };
        ZoomInSetting resetCameraSetting = new ZoomInSetting { finalZoomSize = 5f, duration = 0.5f, afterDelay = 0f };
        CinemachineCameraControl.Instance.ZoomInCameraActions(zoomInSetting, resetCameraSetting);
    }

    private IEnumerator GetInDarkScreen(float duration)
    {
        Animator anim = GameObject.FindGameObjectWithTag("Transition").GetComponent<Animator>();
        if (anim == null)
            yield break;

        anim.SetTrigger("TransitionPlaying");
        yield return new WaitForSeconds(duration);
        anim.ResetTrigger("TransitionPlaying");
    }

    /// <summary>
    /// 0.1秒內瞬移到前方6m處。	
    /// </summary>
    private void MoveFoward()
    {
        StartCoroutine(MoveToPosition(sourceCaster.transform
            , sourceCaster.transform.position + sourceCaster.transform.right * 6f
            , 0.1f));
    }
    public IEnumerator MoveToPosition(Transform transform, Vector3 destination, float timeToMove)
    {
        var originPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            // 讓技能物件跟隨玩家，並偵測是否命中
            this.transform.position = sourceCaster.transform.position + sourceCaster.transform.right * 0.8f;
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(originPos, destination, t);
            yield return null;
        }
    }

    private IEnumerator HitDetect()
    {
        yield return new WaitForSeconds(currentSkill.castTime + 0.2f);
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
        StartCoroutine(CameraShake.Instance.StartShakeCamera(1.6f, 0.4f, 0.1f, true));
    }
}