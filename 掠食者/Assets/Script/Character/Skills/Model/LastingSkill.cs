using UnityEngine;

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
        this.target = target.GetComponent<Character>();

        #region 傷害階段
        if (currentSkill.skillType != AttackType.Effect)
        {
            if (canDamageSelf)
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
                        nextDamageTime = Time.time + currentSkill.timesOfPerDamage;
                    }
                }
            }
        }
        #endregion

        #region 效果影響階段
        if (!target.CompareTag(sourceCaster.tag))
        {
            InvokeAffect(hitAffect);
        }
        #endregion
    }
}
