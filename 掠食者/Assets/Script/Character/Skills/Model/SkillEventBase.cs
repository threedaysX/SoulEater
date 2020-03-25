using UnityEngine;

/// <summary>
/// 各類技能的事件。
/// 包含對自身的、對敵人...等等的行為模式(Buff、擊退、暈眩...事件)。
/// </summary>
public abstract class SkillEventBase : ScriptableObject
{
    protected Character character;
    protected Collider2D[] hits;
}