using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 各類技能的事件。
/// 包含對自身的、對敵人...等等的行為模式(Buff、擊退、暈眩...事件)。
/// </summary>
public abstract class SkillEventBase : MonoBehaviour, ISkillGenerator
{
    private const string skillAnimTrigger = "Trigger";
    protected bool canDamageSelf = false;
    protected Skill currentSkill;
    protected Character sourceCaster;
    protected Character target;

    // 立即效果
    public UnityEvent immediatelyAffect;
    // 命中效果
    public UnityEvent hitAffect;

    public Animator anim;
    public AudioSource sound;
    public ParticleSystem particle;

    private void Awake()
    {
        this.gameObject.AddComponent<PolygonCollider2D>().isTrigger = true;
        this.gameObject.layer = LayerMask.NameToLayer("Skill");

        // 附加技能效果
        AddAffectEvent();
    }

    /// <summary>
    /// 技能生成。
    /// </summary>
    /// <param name="caster">施放技能的人</param>
    public void GenerateSkill(Character caster, Skill skill)
    {
        sourceCaster = caster;
        currentSkill = skill;
        anim = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
        particle = GetComponent<ParticleSystem>();

        // 觸發立即性效果
        InvokeAffect(immediatelyAffect);

        if (sound != null)
        {
            sound.PlayOneShot(sound.clip);
        }
        if (anim != null)
        {
            AnimationBase.Instance.PlayAnimationLoop(anim, skillAnimTrigger, currentSkill.duration, false, false);
        }
        if (particle != null)
        {
            particle.Play();
        }
    }

    protected void InvokeAffect(UnityEvent affectEvent)
    {
        if (affectEvent == null)
            return;
        affectEvent.Invoke();
    }

    protected virtual void DamageTarget()
    {
        float damage = DamageController.Instance.GetSkillDamage(sourceCaster, target, currentSkill);
        target.TakeDamage((int)damage);
        sourceCaster.DamageDealtSteal(damage, false);
    }

    protected abstract void AddAffectEvent();

    #region 技能通用效果
    public string lockDirectionBuffName = "方向鎖定";
    public virtual void LockDirectionTillEnd()
    {
        void affect() { sourceCaster.freeDirection.Lock(); }
        void remove() { sourceCaster.freeDirection.UnLock(); }
        sourceCaster.buffController.AddBuffEvent(lockDirectionBuffName, affect, remove, currentSkill.duration);
    }
    #endregion
}