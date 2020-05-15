using StatsModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region 基礎參數
    [Header("基礎參數")]
    public string characterName;
    public bool isHealthDirty = false;
    public bool isManaDirty = false;
    private float lastHealth = 0;   // 儲存上次的血量
    private float lastMana = 0;   // 儲存上次的魔力
    [SerializeField] private float _currentHealth = 0;
    [SerializeField] private float _currentMana = 0;
    [SerializeField] private float cumulativeDamageTake = 0;    // 累積受到的傷害
    [SerializeField] private float cumulativeDamageDealt = 0;   // 累積造成的傷害
    public float CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
            if (_currentHealth > data.maxHealth.Value)
            {
                _currentHealth = data.maxHealth.Value;
            }

            if (lastHealth != _currentHealth && !isHealthDirty)
            {
                lastHealth = _currentHealth;
                isHealthDirty = true;
            }
        }
    }
    public float CurrentMana
    {
        get
        {
            return _currentMana;
        }
        set
        {
            _currentMana = value;
            if (_currentMana > data.maxMana.Value)
            {
                _currentMana = data.maxMana.Value;
            }

            if (lastMana != _currentMana && !isManaDirty)
            {
                lastMana = _currentHealth;
                isManaDirty = true;
            }
        }
    }
    #endregion

    #region 操作狀態判定
    [Header("操作狀態判定")]
    public bool isLockAction = false;   // 用來判斷是否完全無法行動
    public BasicOperation move;
    public BasicOperation jump;
    public BasicOperation evade;
    public BasicOperation attack;
    public BasicOperation useSkill;
    public BasicOperation freeDirection;  // 判斷是否被鎖定面對方向
    
    [Header("特殊狀態判定")]
    [SerializeField] private bool isImmune = false;  // 用來判斷角色是否無敵(不會被命中)
    [SerializeField] private bool _isKnockStun = false;
    /// <summary>
    /// 判斷是否被擊暈
    /// </summary>
    public bool IsKnockStun 
    {
        get
        {
            return _isKnockStun;
        }
        set
        {
            _isKnockStun = value;
            if (_isKnockStun)
            {
                SetOperation(false);
            }
            else
            {
                SetOperation(true);
            }
        }
    }
    #endregion

    #region 角色資料
    [Header("詳細參數")]
    public Data data;
    [Header("技能欄")]
    public List<Skill> skillFields;
    #endregion

    #region 控制器
    [HideInInspector] public OperationSoundController operationSoundController; // 操作聲音控制
    [HideInInspector] public WeaponController weaponController; // 武器控制
    [HideInInspector] public OperationController operationController;    // 操作控制
    [HideInInspector] public SkillController skillController;   // 技能控制
    [HideInInspector] public BuffController buffController; // 狀態控制
    [HideInInspector] public CombatController combatController; // 戰鬥控制
    [HideInInspector] public KnockStunSystem knockBackSystem; // 擊退控制
    [HideInInspector] public Animator anim;
    #endregion

    private void Awake()
    {
        this.gameObject.AddComponent<KnockStunSystem>();
        operationSoundController = GetComponent<OperationSoundController>();
        weaponController = GetComponent<WeaponController>();
        operationController = GetComponent<OperationController>();
        skillController = GetComponent<SkillController>();
        buffController = GetComponent<BuffController>();
        combatController = GetComponent<CombatController>();
        knockBackSystem = GetComponent<KnockStunSystem>();
        anim = GetComponent<Animator>();

        ResetBaseData();
        InitBasicOperation();

        CurrentHealth = data.maxHealth.Value;
        CurrentMana = data.maxMana.Value;
    }

    /// <summary>
    /// 受到傷害
    /// </summary>
    /// <param name="damageDirectionX">傷害來源方向</param>
    /// <param name="damage">單次傷害</param>
    /// <param name="timesOfPerDamage">造成單次傷害所需時間</param>
    /// <param name="duration">持續時間</param>
    /// <param name="damageImmediate">是否立即造成傷害</param>
    public virtual void TakeDamage(int damage, float damageDirectionX = 0, float weaponKnockBackForce = 0, float timesOfPerDamage = 0, float duration = 0, bool damageImmediate = true)
    {
        if (damage < 0 || isImmune)
        {
            return;
        }

        if (timesOfPerDamage <= 0 || duration <= 0)
        {
            CurrentHealth -= damage;
            cumulativeDamageTake += damage;
            ObjectPools.Instance.DamagePopup("DamageText", DamageController.Instance.IsCritical, damage,
                transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(2.2f, 2.6f), 0), damageDirectionX);

            // 自身受到超過KB值的傷害，會被擊退
            float knockbackDamage = data.knockBackDamage.Value;
            if (cumulativeDamageTake >= knockbackDamage && damageDirectionX != 0)
            {
                knockBackSystem.KnockStun(this, damageDirectionX, weaponKnockBackForce);
                cumulativeDamageTake -= knockbackDamage;
            }
        }
        else
        {
            // 流血...等持續性傷害
            StartCoroutine(TakeDamagePerSecondInTimes(damage, timesOfPerDamage, duration, damageImmediate));
        }

        if (CurrentHealth <= 0)
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
                TakeDamage((int)damage);
            }
            yield return new WaitForSeconds(timesOfPerDamage);
            duration -= timesOfPerDamage;
            damageImmediate = true;
        }
        yield break;
    }

    public virtual void Die()
    {
        this.StopAllCoroutines();
        this.gameObject.SetActive(false);
    }

    public virtual bool UseSkill(Skill skill)
    {
        if (skill == null || !useSkill.canDo)
            return false;
        return skillController.Trigger(skill);
    }

    public virtual void LearnSkill(Skill skill)
    {
        skillFields.Add(skill);
    }

    public virtual bool StartAttack(AttackType attackType = AttackType.Attack, ElementType elementType = ElementType.None)
    {
        bool attackSuccess = false;
        if (attack.canDo)
        {
            attackSuccess = operationController.StartAttackAnim(delegate 
            { 
                return combatController.Attack(attackType, elementType); 
            });
        }

        return attackSuccess;
    }

    /// <summary>
    /// 傷害吸血、吸魔
    /// </summary>
    /// <param name="damage">傷害量</param>
    /// <param name="isAttack">【True】為一般攻擊 ||【False】為技能攻擊</param>
    public void DamageDealtSteal(float damage, bool isAttack)
    {
        cumulativeDamageDealt += damage;
        LifeSteal(damage, isAttack);
        ManaSteal(damage);
    }

    /// <summary>
    /// 傷害吸血
    /// </summary>
    protected void LifeSteal(float damage, bool isAttack)
    {
        if (damage <= 0)
            return;

        switch (isAttack) 
        {
            case true:
                float attackLifeSteal = data.attackLifeSteal.Value / 100;
                if (attackLifeSteal > 0)
                {
                    CurrentHealth += Mathf.Ceil(damage * attackLifeSteal);
                }
                break;
            case false:
                float skillLifeSteal = data.skillLifeSteal.Value / 100;
                if (skillLifeSteal > 0)
                {
                    CurrentHealth += Mathf.Ceil(damage * skillLifeSteal);
                }
                break;
        }
    }

    /// <summary>
    /// 傷害回魔
    /// </summary>
    protected void ManaSteal(float damage)
    {
        if (damage <= 0)
            return;

        // 造成N點傷害量，就回復N點魔力
        float manaStealOfDamage = data.manaStealOfDamage.Value;
        if (cumulativeDamageDealt >= manaStealOfDamage)
        {
            CurrentMana += Mathf.Ceil(data.manaStealOfPoint.Value);
            cumulativeDamageDealt -= manaStealOfDamage;
        }
    }

    /// <summary>
    /// 一次調整所有行動 (用在擊暈等重大影響的異常or特定動作，表示在此影響結束前，不得進行其他動作)
    /// </summary>
    /// <param name="canDo">若True，代表恢復正常行動，反之則鎖定所有行動</param>
    public void SetOperation(bool canDo = true)
    {
        if (canDo)
        {
            move.UnLock();
            jump.UnLock();
            evade.UnLock();
            attack.UnLock();
            useSkill.UnLock();
            freeDirection.UnLock();
            isLockAction = false;
        }
        else
        {
            move.Lock();
            jump.Lock();
            evade.Lock();
            attack.Lock();
            useSkill.Lock();
            freeDirection.Lock();
            isLockAction = true;
        }
    }

    /// <summary>
    /// 進入無敵狀態
    /// </summary>
    /// <param name="duration">無敵狀態持續時間</param>
    public void GetIntoImmune(float duration)
    {
        StartCoroutine(GetIntoImmuneCoroutine(duration));
    }

    public void GetIntoImmune(bool can)
    {
        isImmune = can;
    }

    /// <summary>
    /// 是否為無敵狀態
    /// </summary>
    public bool GetImmuneState()
    {
        return isImmune;
    }

    private IEnumerator GetIntoImmuneCoroutine(float duration)
    {
        isImmune = true;
        yield return new WaitForSeconds(duration);
        if (this == null)
        {
            yield break;
        }
        isImmune = false;
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
        this.data.criticalDamage.BaseValue = dataInitializer.GetCriticalDamage();
        this.data.knockBackDamage.BaseValue = dataInitializer.GetKnockbackDamage();
        this.data.manaStealOfPoint.BaseValue = dataInitializer.GetManaStealOfPoint();
        this.data.manaStealOfDamage.BaseValue = dataInitializer.GetManaStealOfDamage();
        this.data.jumpForce.BaseValue = dataInitializer.GetJumpForce();
        this.data.moveSpeed.BaseValue = dataInitializer.GetMoveSpeed();
        this.data.attackDelay.BaseValue = dataInitializer.GetAttackDelay();
        this.data.reduceSkillCoolDown.BaseValue = dataInitializer.GetSkillCoolDownReduce();
        this.data.reduceCastTime.BaseValue = dataInitializer.GetCastTimeReduce();
        this.data.evadeCoolDown.BaseValue = dataInitializer.GetEvadeCoolDownDuration();
        this.data.recoverFromKnockStunTime.BaseValue = dataInitializer.GetRecoverFromKnockStunTime();
    }

    private void InitBasicOperation()
    {
        move.operationType = BasicOperationType.Move;
        move.canDo = true;
        jump.operationType = BasicOperationType.Jump;
        jump.canDo = true;
        evade.operationType = BasicOperationType.Evade;
        evade.canDo = true;
        attack.operationType = BasicOperationType.Attack;
        attack.canDo = true;
        useSkill.operationType = BasicOperationType.UseSkill;
        useSkill.canDo = true;
        freeDirection.operationType = BasicOperationType.LockDirection;
        freeDirection.canDo = true;
    }
    #endregion

    #region GetData
    public float GetSkillCost(CostType costType)
    {
        float resultCost = CurrentMana;
        switch (costType)
        {
            case CostType.Health:
                resultCost = CurrentHealth;
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
