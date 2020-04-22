using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 各類技能的事件。
/// 包含對自身的、對敵人...等等的行為模式(Buff、擊退、暈眩...事件)。
/// </summary>
public abstract class SkillEventBase : MonoBehaviour
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
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        AnimationBase.Instance.PlayAnimation(anim, skillAnimTrigger, currentSkill.duration);
        AddAffectEvent();

        InvokeAffect(immediatelyAffect);
    }

    /// <summary>
    /// 技能生成，相對於施法者位置。
    /// </summary>
    /// <param name="caster">施放技能的人</param>
    /// <param name="skill">哪個技能</param>
    public void InstantiateSkill(Character caster, Skill skill)
    {       
        var skillObj = Instantiate(skill.prefab, caster.transform.position + caster.transform.right * skill.range, caster.transform.rotation);
        skillObj.GetComponent<SkillEventBase>().Init(caster, skill);
    }

    /// <summary>
    /// 技能生成，可指定位置。
    /// </summary>
    /// <param name="caster">施放技能的人</param>
    /// <param name="skill">哪個技能</param>
    /// <param name="position">技能生成位置</param>
    /// <param name="rotation">技能生成角度</param>
    public void InstantiateSkill(Character caster, Skill skill, Vector3 position, Quaternion rotation)
    {
        var skillObj = Instantiate(skill.prefab, position, rotation);
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

    protected abstract void AddAffectEvent();
}