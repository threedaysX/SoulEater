using UnityEngine;
using StatsModel;

[CreateAssetMenu(menuName = "Character/Data/Data")]
public class Data : ScriptableObject
{
    [Header("基本參數")]
    public Stats maxHealth;
    public Stats maxMana;
    public Stats attack;
    public Stats magicAttack;
    public Stats defense;
    public Stats critical;
    public Stats criticalDamage;
    public Stats knockBackDamage;
    public Stats penetrationValue;  // 穿甲值
    public Stats penetrationMagnification;   // 穿甲倍率
    public Resistance resistance;
    public Status status;

    [Header("功能參數")]
    public Stats jumpForce = new Stats(1);
    public Stats moveSpeed = new Stats(1);
    public Stats attackDelay = new Stats(1);
    public Stats attackRange = new Stats(1);
    public Stats reduceSkillCoolDown;
    public Stats reduceCastTime;
    public Stats evadeCoolDown;
    public Stats recoverFromKnockStunTime;
    public Stats attackLifeSteal;
    public Stats skillLifeSteal;
    public Stats manaStealOfPoint; 
    public Stats manaStealOfDamage;

    [Header("武器")]
    public int cycleAttackCount;
    public float weaponKnockBackForce;  // 武器擊退力道
    public Transform hitEffectPrefab;    // 武器擊中效果
    public Transform attackHitBoxPrefab;
    public WeaponSoundSet weaponSoundSet;
    public WeaponType weaponType;
    public ElementType attackElement;
}
