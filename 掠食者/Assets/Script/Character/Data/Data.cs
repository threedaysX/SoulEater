using UnityEngine;
using StatsModel;

[CreateAssetMenu(menuName = "Character/Data/Data")]
public class Data : ScriptableObject
{
    [Header("基本參數")]
    public Stats maxHealth; 
    public Stats mana;
    public Stats attack;
    public Stats magicAttack;
    public Stats defense;
    public Stats critical;
    public Stats criticalDamage;
    public Stats knockBackDamage;
    public Stats manaRecovering; 
    public Stats manaRecoveringOfDamage;
    public Resistance resistance;
    public Status status;

    [Header("功能參數")]
    public Stats jumpForce;
    public Stats moveSpeed;
    public Stats attackSpeed;
    public Stats attackRange;
    public Stats reduceSkillCoolDown;
    public Stats reduceChantTime;
    public Stats reduceEvadeCoolDown;
}
