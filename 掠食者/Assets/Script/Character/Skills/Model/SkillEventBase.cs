using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 各類技能的事件。
/// 包含對自身的、對敵人...等等的行為模式(Buff、擊退、暈眩...事件)。
/// </summary>
public abstract class SkillEventBase : MonoBehaviour
{
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
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        AnimationController.Instance.PlayAnimation(anim, currentSkill.duration);
        AddAffectEvent();

        InvokeAffect(immediatelyAffect);
    }

    public void InstantiateSkill(Character caster, Skill skill)
    {
        // 根據技能定義生成於特定位置。
        var skillObj = Instantiate(skill.prefab, caster.transform.position + caster.transform.right * skill.range, caster.transform.rotation);
        skillObj.GetComponent<SkillEventBase>().Init(caster, skill);
    }

    private void Init(Character caster, Skill skill)
    {
        sourceCaster = caster;
        currentSkill = skill;
    }

    protected void InvokeAffect(UnityEvent affectEvent)
    {
        if (affectEvent == null)
            return;
        affectEvent.Invoke();
    }

    protected virtual void DamageTarget()
    {
        target.TakeDamage(DamageController.Instance.GetSkillDamage(sourceCaster, target, currentSkill));
    }

    protected virtual UnityEvent CreateAffectEvent(UnityAction call)
    {
        UnityEvent affect = new UnityEvent();
        affect.AddListener(call);

        return affect;
    }

    protected abstract void AddAffectEvent();
}