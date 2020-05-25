using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Character/Skill")]
public class Skill : ScriptableObject
{
    public Sprite icon;
    public AttackType skillType;
    public ElementType elementType = ElementType.None;
    public string skillName;
    public string description;

    /// <summary>
    /// 技能施放距離(與自身相對距離)
    /// </summary>
    public float range = 1f;

    /// <summary>
    /// 技能消耗
    /// </summary>
    public float cost = 1f;
    public CostType costType = CostType.Mana;

    /// <summary>
    /// 技能倍率(基於人物攻擊值)(%)
    /// </summary>
    public float damageMagnification = 100f;

    /// <summary>
    /// 技能詠唱時間
    /// </summary>
    public float castTime;

    /// <summary>
    /// 技能冷卻
    /// </summary>
    public float coolDown = 1f;
    public bool cooling = false;

    /// <summary>
    /// 技能造成每次傷害所需的時間間隔(Ex: 每[0.2]秒造成100傷害...)
    /// </summary>
    public float timesOfPerDamage = 1f;

    /// <summary>
    /// 技能的持續時間
    /// </summary>
    public float duration = 1f;

    public Transform prefab;
}

