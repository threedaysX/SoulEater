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
    public abstract void OnTriggerEnter2D(Collider2D target);
}