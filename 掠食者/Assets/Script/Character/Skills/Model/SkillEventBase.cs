using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 各類技能的事件。
/// 包含對自身的、對敵人...等等的行為模式(Buff、擊退、暈眩...事件)。
/// </summary>
public abstract class SkillEventBase : MonoBehaviour, ISkillGenerator, ISkillUse, ISkillCaster
{
    private const string skillAnimTrigger = "Trigger";
    public bool autoRenderCollider = true;
    public bool canTriggerSelf = false;
    protected Skill currentSkill;
    protected Character sourceCaster;
    public Character target;

    // 立即效果
    public UnityEvent immediatelyAffect;
    // 命中效果
    public UnityEvent hitAffect;

    protected Animator anim;
    protected AudioSource sound;
    public AudioClip castSound;
    public AudioClip inUsingSound;
    public ParticleSystem inUsingParticle;

    private void Awake()
    {
        if (autoRenderCollider)
        {
            this.gameObject.AddComponent<PolygonCollider2D>().isTrigger = true;
        }

        this.gameObject.layer = LayerMask.NameToLayer("Skill");

        // 附加技能效果
        AddAffectEvent();
    }

    public void GenerateSkill(Character caster, Skill skill)
    {
        sourceCaster = caster;
        currentSkill = skill;

        anim = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();

        // 禁用技能Hitbox與貼圖 (避免生成技能時直接播放動畫與觸發效果)
        SetSkillEnable(false);
    }

    /// <summary>
    /// 技能生成。
    /// </summary>
    /// <param name="caster">施放技能的人</param>
    public void UseSkill()
    {
        // 啟用技能
        SetSkillEnable(true);

        // 觸發立即性效果
        InvokeAffect(immediatelyAffect);

        if (sound != null)
        {
            sound.PlayOneShot(inUsingSound);
        }
        if (anim != null)
        {
            AnimationBase.Instance.PlayAnimationLoop(anim, skillAnimTrigger, currentSkill.duration, false, false);
        }
        if (inUsingParticle != null)
        {
            inUsingParticle.Play(true);
        }
    }

    public virtual void CastSkill()
    {
        PlayCastSound();
    }

    /// <summary>
    /// 使用技能前，會撥放預設施放音效
    /// </summary>
    public virtual void PlayCastSound()
    {
        if (castSound == null)
        {
            sourceCaster.operationSoundController.PlaySound(sourceCaster.operationSoundController.castSound);
        }
        else
        {
            sourceCaster.operationSoundController.PlaySound(this.castSound);
        }
    }

    /// <summary>
    /// 技能開始施放，可將Enabled為True
    /// </summary>
    private void SetSkillEnable(bool enable)
    {
        // 技能貼圖
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.enabled = enable;
        }

        // 技能碰撞
        Collider2D col = this.GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = enable;
        }
    }

    protected IEnumerator SetActiveAfterSkillDone(float duration)
    {
        yield return new WaitForSeconds(duration);
        SetActiveAfterSkillDone(false);
    }

    protected void SetActiveAfterSkillDone(bool active)
    {
        this.gameObject.SetActive(active);
    }

    protected void InvokeAffect(UnityEvent affectEvent)
    {
        if (affectEvent == null)
            return;
        affectEvent.Invoke();
    }

    protected virtual void DamageTarget(float damageDirectionX = 0)
    {
        float damage = DamageController.Instance.GetSkillDamage(sourceCaster, target, currentSkill);
        target.TakeDamage((int)damage, damageDirectionX);
        sourceCaster.DamageDealtSteal(damage, false);
    }

    protected abstract void AddAffectEvent();

    #region 技能通用效果
    protected string lockDirectionBuffName = "方向鎖定";
    public virtual void LockDirectionTillEnd()
    {
        void affect() { sourceCaster.freeDirection.Lock(); }
        void remove() { sourceCaster.freeDirection.UnLock(); }
        sourceCaster.buffController.AddBuffEvent(lockDirectionBuffName, affect, remove, currentSkill.duration);
    }
    #endregion
}