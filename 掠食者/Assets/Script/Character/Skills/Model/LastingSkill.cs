using UnityEngine;

/// <summary>
/// 持續性技能
/// </summary>
public abstract class LastingSkill : SkillEventBase
{
    protected float nextDamageTime = 0;

    /// <summary>
    /// [針對各個技能]
    /// 當技能碰觸到物件時 (造成傷害、異常...)
    /// 該技能的目標為何？
    /// </summary>
    public virtual void OnTriggerStay2D(Collider2D target)
    {
        if (sourceCaster == null || target == null)
            return;
        this.target = target.GetComponent<Character>();
        if (this.target == null || this.target.GetImmuneState())
            return;
    }
}
