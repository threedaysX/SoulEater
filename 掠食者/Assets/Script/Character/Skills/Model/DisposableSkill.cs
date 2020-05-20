using UnityEngine;

/// <summary>
/// 一次性技能
/// </summary>
public abstract class DisposableSkill : SkillEventBase
{
    /// <summary>
    /// [針對各個技能]
    /// 當技能碰觸到物件時 (造成傷害、異常...)
    /// 該技能的目標為何？
    /// </summary>
    public virtual void OnTriggerEnter2D(Collider2D targetCol)
    {
        if (targetCol == null)
            return;
        // 如果目標是自己，除非技能可以對自己造成效果，否則略過
        if (!canTriggerSelf && targetCol.CompareTag(sourceCaster.tag))
            return;

        this.target = targetCol.GetComponent<Character>();
        if (this.target == null || this.target.GetImmuneState())
            return;
    }
}