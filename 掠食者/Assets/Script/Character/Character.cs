using StatsModel;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("基礎參數")]
    public string characterName;
    public float currentHealth;
    public float currentMana;

    [Header("操作判定")]
    public bool canMove = true;
    public bool canJump = true;
    public bool canEvade = true;
    public bool canAttack = true;
    public bool canSkill = true;

    [Header("詳細參數")]
    public Data data;
    [Header("技能欄")]
    public Skill[] skillFields;

    [HideInInspector] public OperationSoundController operationSoundController; // 操作聲音控制
    [HideInInspector] public WeaponController weaponController; // 武器控制
    [HideInInspector] public OperationController operationController;    // 操作控制
    [HideInInspector] public SkillController skillController;   // 技能控制
    [HideInInspector] public BuffController buffController; // 狀態控制
    [HideInInspector] public CombatController combatController; // 戰鬥控制
    [HideInInspector] public Animator animator;

    private void Awake()
    {
        operationSoundController = GetComponent<OperationSoundController>();
        weaponController = GetComponent<WeaponController>();
        operationController = GetComponent<OperationController>();
        skillController = GetComponent<SkillController>();
        buffController = GetComponent<BuffController>();
        combatController = GetComponent<CombatController>();
        animator = GetComponent<Animator>();

        ResetBaseData();

        currentHealth = data.maxHealth.Value;
        currentMana = data.maxMana.Value;
    }   

    /// <summary>
    /// 受到傷害
    /// </summary>
    /// <param name="damage">單次傷害</param>
    /// <param name="timesOfPerDamage">造成單次傷害所需時間</param>
    /// <param name="duration">持續時間</param>
    /// <param name="damageImmediate">是否立即造成傷害</param>
    public void TakeDamage(float damage, float timesOfPerDamage = 0, float duration = 0, bool damageImmediate = true)
    {
        if (damage < 0)
            damage = 0;

        if (timesOfPerDamage <= 0 || duration <= 0)
        {
            currentHealth -= damage;
            string criticalLog = "";
            if (DamageController.Instance.IsCritical)
            {
                criticalLog = "爆擊！！  ";
            }
            Debug.Log(criticalLog + characterName + "受到 " + damage + "點傷害");
        }
        else
        {
            StartCoroutine(TakeDamagePerSecondInTimes(damage, timesOfPerDamage, duration, damageImmediate));
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator TakeDamagePerSecondInTimes(float damage, float timesOfPerDamage, float duration, bool damageImmediate)
    {
        while(duration >= 0)
        {
            if (damageImmediate)
            {
                TakeDamage(damage);
            }
            yield return new WaitForSeconds(timesOfPerDamage);
            duration -= timesOfPerDamage;
            damageImmediate = true;
        }
        yield break;
    }

    public void UseSkill(Skill skill)
    {
        if (!canSkill)
            return;

        Debug.Log(this.data.resistance.fire.Value);
        skillController.Trigger(skill);
        Debug.Log(this.data.resistance.fire.Value);
    }

    public virtual void Die()
    {
        this.StopAllCoroutines();
        Destroy(this.gameObject);
    }

    #region DataInitialize
    public void ResetBaseData()
    {
        DataInitializer dataInitializer = new DataInitializer(data.status);
        this.data.maxHealth.BaseValue = dataInitializer.GetMaxHealth();
        this.data.maxMana.BaseValue = dataInitializer.GetMaxMana();
        this.data.attack.BaseValue = dataInitializer.GetAttack();
        this.data.magicAttack.BaseValue = dataInitializer.GetMagicAttack();
        this.data.defense.BaseValue = dataInitializer.GetDefense();
        this.data.critical.BaseValue = dataInitializer.GetCritical();
        this.data.knockBackDamage.BaseValue = dataInitializer.GetKnockbackDamage();
        this.data.manaRecoveringOfDamage.BaseValue = dataInitializer.GetManaRecoveringOfDamage();
        this.data.jumpForce.BaseValue = dataInitializer.GetJumpForce();
        this.data.moveSpeed.BaseValue = dataInitializer.GetMoveSpeed();
        this.data.attackDelay.BaseValue = dataInitializer.GetAttackDelay();
        this.data.reduceSkillCoolDown.BaseValue = dataInitializer.GetSkillCoolDownReduce();
        this.data.reduceCastTime.BaseValue = dataInitializer.GetCastTimeReduce();
        this.data.reduceEvadeCoolDown.BaseValue = dataInitializer.GetEvadeCoolDownReduce();
    }
    #endregion

    #region GetData
    public float GetSkillCost(CostType costType)
    {
        float resultCost = currentMana;
        switch (costType)
        {
            case CostType.Health:
                resultCost = currentHealth;
                break;
            case CostType.Mana:
                break;
        }

        return resultCost;
    }

    public Stats GetAttackData(AttackType attackType)
    {
        Stats resultAttack = data.attack;
        switch (attackType)
        {
            case AttackType.Attack:
                break;
            case AttackType.Magic:
                resultAttack = data.magicAttack;
                break;
        }

        return resultAttack;
    }

    public Stats GetResistance(ElementType elementType = ElementType.None)
    {
        Stats resultResistance = data.resistance.none;
        switch (elementType)
        {
            case ElementType.None:
                break;
            case ElementType.Fire:
                resultResistance = data.resistance.fire;
                break;
            case ElementType.Water:
                resultResistance = data.resistance.water;
                break;
            case ElementType.Earth:
                resultResistance = data.resistance.earth;
                break;
            case ElementType.Air:
                resultResistance = data.resistance.air;
                break;
            case ElementType.Thunder:
                resultResistance = data.resistance.thunder;
                break;
            case ElementType.Light:
                resultResistance = data.resistance.light;
                break;
            case ElementType.Dark:
                resultResistance = data.resistance.dark;
                break;
        }

        return resultResistance;
    }
    #endregion
}
