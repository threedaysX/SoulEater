using StatsModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OperationSoundController))]
[RequireComponent(typeof(WeaponController))]
[RequireComponent(typeof(OperationController))]
[RequireComponent(typeof(SkillController))]
[RequireComponent(typeof(BuffController))]
[RequireComponent(typeof(CombatController))]
[RequireComponent(typeof(IconController))]
[RequireComponent(typeof(KnockStunSystem))]
[RequireComponent(typeof(DieController))]
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

    #region 狀態判定
    [Header("操作狀態判定")]
    public bool isLockAction = false;   // 用來判斷是否完全無法行動
    public BasicOperation move;
    public BasicOperation jump;
    public BasicOperation evade;
    public BasicOperation attack;
    public BasicOperation useSkill;
    public BasicOperation freeDirection;  // 判斷是否被鎖定面對方向

    [Header("特殊狀態判定")]
    public GameObject lastAttackMeTarget;   // 上一個攻擊我的目標是?
    public bool isKnockStun = false;   // 判斷是否被擊暈
    [SerializeField] private bool isImmune = false;  // 用來判斷角色是否無敵(不會被命中)
    #endregion

    #region 角色資料
    [Header("詳細參數")]
    public CharacterData data;
    [Header("技能欄")]
    public List<Skill> skillFields;
    private Dictionary<string, int> skillDictionary;

    [Header("彈出文字訊息中心點")]
    public Transform popupTextCenter;

    [Header("受傷數字顏色")]
    public Color normalDamagedColor = new Color32(255, 255, 255, 255);
    public Color criticalDamagedColor = new Color32(255, 0, 0, 255);
    #endregion

    #region 控制器
    [HideInInspector] public OperationSoundController operationSoundController; // 操作聲音控制
    [HideInInspector] public WeaponController weaponController; // 武器控制
    [HideInInspector] public OperationController operationController;    // 操作控制
    [HideInInspector] public SkillController skillController;   // 技能控制
    [HideInInspector] public BuffController buffController; // 狀態控制
    [HideInInspector] public CombatController combatController; // 戰鬥控制
    [HideInInspector] public KnockStunSystem knockBackSystem; // 擊退控制
    [HideInInspector] public DieController dieController; // 死亡控制
    [HideInInspector] public IconController iconController; // 提示圖示控制
    [HideInInspector] public Animator anim;
    #endregion

    private void Awake()
    {
        operationSoundController = GetComponent<OperationSoundController>();
        weaponController = GetComponent<WeaponController>();
        operationController = GetComponent<OperationController>();
        skillController = GetComponent<SkillController>();
        buffController = GetComponent<BuffController>();
        combatController = GetComponent<CombatController>();
        knockBackSystem = GetComponent<KnockStunSystem>();
        dieController = GetComponent<DieController>();
        iconController = GetComponent<IconController>();
        anim = GetComponent<Animator>();

        ReBorn();
    }

    public virtual void LateUpdate()
    {
        CheckOperationAvoidLockAllDays();
    }

    #region Damage & TakeDamage & Knock & Die
    /// <summary>
    /// 受到傷害
    /// </summary>
    public virtual bool TakeDamage(DamageData data)
    {
        lastAttackMeTarget = data.damageSource;
        if (isImmune)
        {
            return false;
        }
        if (data.damage <= 0)
        {
            data.damage = 0;
            DamagePopup(data.damage, data.isCritical);
            return true;
        }

        if (data.timesOfPerDamage <= 0 || data.duration <= 0)
        {
            CurrentHealth -= data.damage;
            cumulativeDamageTake += data.damage;
            if (CurrentHealth <= 0)
            {
                Die();
            }
            if (this != null)
            {
                StartCoroutine(TakeDamageColorChanged(0.1f));
            }
            DamagePopup(data.damage, data.isCritical);
            KnockBackCheck(data.damageDirectionX, data.weaponKnockBackForce);
        }
        else
        {
            // 流血...等持續性傷害
            StartCoroutine(TakeDamagePerSecondInDuration(data));
        }
        return true;
    }

    /// <summary>
    /// 自身受到超過KB值的傷害，會被擊退
    /// </summary>
    private void KnockBackCheck(float damageDirectionX, float knockBackForce)
    {
        float knockbackDamage = data.knockBackDamage.Value;
        if (cumulativeDamageTake >= knockbackDamage && damageDirectionX != 0)
        {
            knockBackSystem.KnockStun(this, damageDirectionX, knockBackForce);
            cumulativeDamageTake -= knockbackDamage;
        }
    }

    private IEnumerator TakeDamagePerSecondInDuration(DamageData data)
    {
        while(data.duration >= 0)
        {
            if (data.damageImmediate)
            {
                // Would not trigger critical. (Ex: Blood, Ignite, Poison...)
                DamageData newData = new DamageData(data)
                {
                    duration = 0,
                    isCritical = false,
                    timesOfPerDamage = 0
                };
                TakeDamage(newData);
            }
            yield return new WaitForSeconds(data.timesOfPerDamage);
            data.duration -= data.timesOfPerDamage;
            data.damageImmediate = true;
        }
        yield break;
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

    public void DamagePopup(int damage, bool isCritical)
    {
        if (isCritical)
        {
            ObjectPools.Instance.PopText("CriticalDamageText", isCritical, damage, criticalDamagedColor, popupTextCenter.position);
        }
        else
        {
            ObjectPools.Instance.PopText("DamageText", isCritical, damage, normalDamagedColor, popupTextCenter.position);
        }
    }

    /// <summary>
    /// Pop english string message Only!
    /// </summary>
    public void PopTextMessage(string message, Color color)
    {
        ObjectPools.Instance.PopText("PopText", message, color, popupTextCenter.position);
    }

    public IEnumerator TakeDamageColorChanged(float duration)
    {
        var sprite = GetComponent<SpriteRenderer>();
        Color originColor = new Color32(255, 255, 255, 255);
        sprite.color = new Color32(255, 100, 100, 230);
        yield return new WaitForSeconds(duration);
        sprite.color = originColor;
    }

    public virtual void Die()
    {
        dieController.StartDie();
    }
    #endregion

    #region Skill
    public virtual bool UseSkill(Skill skill, bool ignoreCoolDown = false, bool ignoreCanDo = false)
    {
        if (skill == null || (!useSkill.canDo && !ignoreCanDo))
            return false;
        return skillController.Trigger(skill, ignoreCoolDown);
    }

    public virtual void LearnSkill(Skill skill)
    {
        skillFields.Add(skill);
        skillDictionary.Add(skill.skillName, skillFields.Count - 1);
    }

    public virtual void RemoveSkill(Skill skill)
    {
        skillFields.RemoveAll(x => x.skillName == skill.skillName);
        ResetSkillDictionaryIndex();
    }

    public Skill GetSkillByName(string skillName)
    {
        return skillFields[skillDictionary[skillName]];
    }
    #endregion
    
    #region Attack
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
    #endregion

    #region Operation
    /// <summary>
    /// 一次調整所有行動 (用在擊暈等重大影響的異常or特定動作，表示在此影響結束前，不得進行其他動作)
    /// </summary>
    /// <param name="islock">若True，代表鎖定所有行動，反之則恢復正常行動</param>
    public void LockOperation(LockType lockType, bool islock, float nextUnLockTime = 0)
    {
        if (islock)
        {
            move.Lock(lockType, nextUnLockTime);
            jump.Lock(lockType, nextUnLockTime);
            evade.Lock(lockType, nextUnLockTime);
            attack.Lock(lockType, nextUnLockTime);
            useSkill.Lock(lockType, nextUnLockTime);
            freeDirection.Lock(lockType, nextUnLockTime);
            isLockAction = true;
        }
        else
        {
            move.UnLock(lockType);
            jump.UnLock(lockType);
            evade.UnLock(lockType);
            attack.UnLock(lockType);
            useSkill.UnLock(lockType);
            freeDirection.UnLock(lockType);
            isLockAction = false;
        }
    }

    protected void CheckOperationAvoidLockAllDays()
    {
        
    }
    #endregion

    #region Immune
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
    #endregion

    #region DataInitialize
    public virtual void ReBorn()
    {
        ResetBaseData();
        ResetStatsDirtyData();
        ResetSkillDictionaryIndex();
        InitBasicOperation();
    }

    public void ResetBaseData()
    {
        DataInitializer dataInitializer = new DataInitializer(data.status);
        data.maxHealth.BaseValue = dataInitializer.GetMaxHealth();
        data.maxMana.BaseValue = dataInitializer.GetMaxMana();
        data.attack.BaseValue = dataInitializer.GetAttack();
        data.magicAttack.BaseValue = dataInitializer.GetMagicAttack();
        data.defense.BaseValue = dataInitializer.GetDefense();
        data.critical.BaseValue = dataInitializer.GetCritical();
        data.criticalDamage.BaseValue = dataInitializer.GetCriticalDamage();
        data.knockBackDamage.BaseValue = dataInitializer.GetKnockbackDamage();
        data.manaStealOfPoint.BaseValue = dataInitializer.GetManaStealOfPoint();
        data.manaStealOfDamage.BaseValue = dataInitializer.GetManaStealOfDamage();
        data.jumpForce.BaseValue = dataInitializer.GetJumpForce();
        data.moveSpeed.BaseValue = dataInitializer.GetMoveSpeed();
        data.attackDelay.BaseValue = dataInitializer.GetAttackDelay();
        data.reduceSkillCoolDown.BaseValue = dataInitializer.GetSkillCoolDownReduce();
        data.reduceCastTime.BaseValue = dataInitializer.GetCastTimeReduce();
        data.evadeCoolDown.BaseValue = dataInitializer.GetEvadeCoolDownDuration();
        data.recoverFromKnockStunTime.BaseValue = dataInitializer.GetRecoverFromKnockStunTime();

        CurrentHealth = data.maxHealth.Value;
        CurrentMana = data.maxMana.Value;
    }

    public void ResetStatsDirtyData()
    {
        #region NormalData
        data.maxHealth.ResetDirtyStats();
        data.maxMana.ResetDirtyStats();
        data.attack.ResetDirtyStats();
        data.attackDelay.ResetDirtyStats();
        data.attackLifeSteal.ResetDirtyStats();
        data.attackRange.ResetDirtyStats();
        data.critical.ResetDirtyStats();
        data.criticalDamage.ResetDirtyStats();
        data.defense.ResetDirtyStats();
        data.evadeCoolDown.ResetDirtyStats();
        data.jumpForce.ResetDirtyStats();
        data.knockBackDamage.ResetDirtyStats();
        data.magicAttack.ResetDirtyStats();
        data.manaStealOfDamage.ResetDirtyStats();
        data.manaStealOfPoint.ResetDirtyStats();
        data.maxHealth.ResetDirtyStats();
        data.maxMana.ResetDirtyStats();
        data.moveSpeed.ResetDirtyStats();
        data.penetrationMagnification.ResetDirtyStats();
        data.penetrationValue.ResetDirtyStats();
        data.recoverFromKnockStunTime.ResetDirtyStats();
        data.reduceCastTime.ResetDirtyStats();
        data.reduceSkillCoolDown.ResetDirtyStats();
        #endregion
        #region Resistance
        data.resistance.air.ResetDirtyStats();
        data.resistance.dark.ResetDirtyStats();
        data.resistance.earth.ResetDirtyStats();
        data.resistance.fire.ResetDirtyStats();
        data.resistance.light.ResetDirtyStats();
        data.resistance.thunder.ResetDirtyStats();
        data.resistance.thunder.ResetDirtyStats();
        data.resistance.water.ResetDirtyStats();
        #endregion
        #region Status
        data.skillLifeSteal.ResetDirtyStats();
        data.status.agility.ResetDirtyStats();
        data.status.dexterity.ResetDirtyStats();
        data.status.intelligence.ResetDirtyStats();
        data.status.lucky.ResetDirtyStats();
        data.status.strength.ResetDirtyStats();
        data.status.vitality.ResetDirtyStats();
        #endregion
        #region Skill
        foreach (Skill skill in skillFields)
        {
            skill.castTime.ResetDirtyStats();
            skill.coolDown.ResetDirtyStats();
            skill.cost.ResetDirtyStats();
            skill.damageMagnification.ResetDirtyStats();
            skill.fixedCastTime.ResetDirtyStats();
            skill.range.ResetDirtyStats();
        }
        #endregion
    }

    public void ResetSkillDictionaryIndex()
    {
        if (skillDictionary == null)
        {
            skillDictionary = new Dictionary<string, int>();
        }
        else
        {
            skillDictionary.Clear();
        }

        int index = 0;
        foreach (Skill skillField in skillFields)
        {
            skillDictionary.Add(skillField.skillName, index++);
        }
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

/// <param name="damageSource">傷害來源</param>
/// <param name="damage">單次傷害</param>
/// <param name="isCritical">是否爆擊</param>
/// <param name="damageDirectionX">傷害來源方向(大於0為右側，小於為左)</param>
/// <param name="timesOfPerDamage">造成單次傷害所需時間</param>
/// <param name="duration">持續時間</param>
/// <param name="damageImmediate">是否立即造成傷害</param>
public struct DamageData
{
    public GameObject damageSource;
    public ElementType element;
    public int damage;
    public bool isCritical;
    public float damageDirectionX;
    public float weaponKnockBackForce;
    public float timesOfPerDamage;
    public float duration;
    public bool damageImmediate;

    public DamageData(DamageData data)
    {
        this.damageSource = data.damageSource;
        this.element = data.element;
        this.damage = data.damage;
        this.isCritical = data.isCritical;
        this.damageDirectionX = data.damageDirectionX;
        this.weaponKnockBackForce = data.weaponKnockBackForce;
        this.timesOfPerDamage = data.timesOfPerDamage;
        this.duration = data.duration;
        this.damageImmediate = data.damageImmediate;
    }

    public DamageData(GameObject damageSource, ElementType element, int damage, bool isCritical, float damageDirectionX = 0, float weaponKnockBackForce = 0, float timesOfPerDamage = 0, float duration = 0, bool damageImmediate = true)
    {
        this.damageSource = damageSource;
        this.element = element;
        this.damage = damage;
        this.isCritical = isCritical;
        this.damageDirectionX = damageDirectionX;
        this.weaponKnockBackForce = weaponKnockBackForce;
        this.timesOfPerDamage = timesOfPerDamage;
        this.duration = duration;
        this.damageImmediate = damageImmediate;
    }
}
