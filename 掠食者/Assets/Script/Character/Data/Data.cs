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
    public Stats manaRecovering; 
    public Stats manaRecoveringOfDamage;
    public Stats penetrationValue;  // 穿甲值
    public Stats penetrationMagnification;   // 穿甲倍率
    public Resistance resistance;
    public Status status;

    [Header("功能參數")]
    public Stats jumpForce = new Stats(1);
    public Stats moveSpeed = new Stats(1);
    public Stats attackSpeed = new Stats(1);
    public Stats attackRange = new Stats(1);
    public Stats reduceSkillCoolDown;
    public Stats reduceCastTime;
    public Stats reduceEvadeCoolDown;
}
