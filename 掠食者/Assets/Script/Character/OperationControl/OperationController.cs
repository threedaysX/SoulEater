using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AnimationKeyWordDictionary
{
    public const string idle = "Idle";
    public const string moveSpeed = "Speed";
    public const string jump = "IsJumping";
    public const string startJump = "StartJump";
    public const string fallJump = "FallJump";
    public const string endJump = "EndJump";
    public const string preAttack = "IsPreAttacking";
    public const string attack = "IsAttacking";
    public const string attackCount = "AttackCount";
    public const string evade = "IsEvading";
    public const string useSkill = "IsSkillUsing";
    public const string castSkill = "IsSkillCasting";
    public const string knockStun = "IsKnockStun";
}

public class Operation
{
    public string name;        // 執行當前行動的來源
    public OperationStateType stateType;    
    public IEnumerator action;              // 當前用來執行此動作的方法
    public float delay;     // 當前動作結束後的時間間隔延遲 (延遲多久進行下一個行動)
    public float nextOperationTime;     // 下一個行動的時間點
    public bool finished;
    public bool paused;
    public bool running;
    public bool stop;
    private bool forceActToEnd;  // 當前動作一定會做完，不會被其他動作中斷(依舊會被外力影響中斷)

    public Operation(string name, IEnumerator action = null, float delay = 0)
    {
        this.name = name;
        this.action = action;
        this.delay = delay;
        stateType = OperationStateType.None;
    }

    public IEnumerator CallOperation()
    {
        if (!stop)
            Start();

        IEnumerator e = action;
        while (running || forceActToEnd)
        {
            if (paused)
                yield return null;
            else
            {
                if (e != null && e.MoveNext())
                {
                    yield return e.Current;
                }
                else
                {
                    running = false;
                    forceActToEnd = false;
                    break;
                }
            }
        }
        finished = true;
    }

    private void Start()
    {
        running = true;
        finished = false;
        stop = false;
    }

    public void Stop(float delayOffset = -999)
    {
        stop = true;
        running = false;
        if (delayOffset != -999)
        {
            delay = delayOffset;
        }
    }

    public void Pause()
    {
        paused = true;
    }
    public void Unpause()
    {
        paused = false;
    }

    public void ForceActToEnd()
    {
        forceActToEnd = true;
    }

    public void SetAction(IEnumerator action)
    {
        this.action = action;
    }
    public void SetDelay(float delay)
    {
        this.delay = delay;
        this.nextOperationTime = Time.time + this.delay;
    }

    /// <summary>
    /// 設置動作狀態 (以對應下一個動作)
    /// </summary>
    public void SetOperationState(OperationStateType stateType)
    {
        this.stateType = stateType;
    }

    public bool CheckOperationSame(string newOperationName)
    {
        if (newOperationName == name)
        {
            return true;
        }
        return false;
    }

    public bool CheckOperationSame(Operation newOperation)
    {
        if (newOperation == null)
            return false;
        if (newOperation.name == name)
        {
            return true;
        }
        return false;
    }

    public bool CheckOperationState(string newOperationName)
    {
        bool canStoreNextOperation = false;
        bool isSameOperation = CheckOperationSame(newOperationName);

        switch (stateType)
        {
            case OperationStateType.None:
                break;
            case OperationStateType.Continuous:
                if (isSameOperation)
                {
                    canStoreNextOperation = true;
                }
                break;
            case OperationStateType.Interrupt:
                if (!isSameOperation)
                {
                    canStoreNextOperation = true;
                    Stop(0);
                }
                break;
            case OperationStateType.Trigger:
                canStoreNextOperation = true;
                if (!isSameOperation)
                {
                    Stop(0);
                }
                break;
            case OperationStateType.Combo:
                canStoreNextOperation = true;
                if (isSameOperation)
                {
                    Stop();
                }
                else
                {
                    Stop(0);
                }
                break;
        }

        return canStoreNextOperation;
    }
}

public class Operations : Dictionary<int, Operation>
{
    public byte currentIndex;   // 當前執行的動作Index
    public byte lastIndex;      // 最後的動作Index

    public Operations()
    {
        currentIndex = 0;
        lastIndex = 0;
    }

    public Operation GetCurrentOperation()
    {
        if (currentIndex == byte.MaxValue)
            currentIndex = byte.MinValue;
        return this[currentIndex];
    }

    public Operation GetLastOperation()
    {
        return this[lastIndex - 1];
    }

    public void AddOperation(Operation operation)
    {
        if (lastIndex == byte.MaxValue)
            lastIndex = byte.MinValue;

        this.Add(lastIndex++, operation);
    }

    public void RemoveCurrentOperation()
    {
        Remove(currentIndex);
        currentIndex++;
    }

    public void RemoveLastOperation()
    {
        Remove(lastIndex - 1);
        lastIndex--;
    }
}

public enum OperationStateType
{
    /// <summary>
    /// 用來判斷此動作【可不可以被預存】(同樣動作or不同動作- 不預存)
    /// </summary>
    None,

    /// <summary>
    /// 用來判斷此動作【可不可以被接續】(同樣動作- 預存不打斷)
    /// </summary>
    Continuous,

    /// <summary>
    /// 用來判斷此動作【可不可以被打斷】(不同動作- 預存打斷)
    /// </summary>
    Interrupt,

    /// <summary>
    /// 用來判斷此動作【可不可以被觸發】(同樣動作- 預存不打斷 or 不同動作- 預存打斷)
    /// </summary>
    Trigger,

    /// <summary>
    /// 用來判斷此動作【可不可以被連段】(同樣動作or不同動作- 預存打斷)
    /// </summary>
    Combo
}

/// <summary>
/// 每個動作會有獨立的延遲
/// </summary>
public class OperationDelayDictionary : Dictionary<string, Operation>
{
    public void AddOperation(string name, Operation operation)
    {
        if (this.ContainsKey(name))
        {
            this[name] = operation;
        }
        else
        {
            this.Add(name, operation);
        }
    }
}

public class OperationController : MonoBehaviour
{
    private Character character;
    private Animator anim;
    private Rigidbody2D rb;

    [Header("操作狀態判定")]
    public bool isIdle = false;
    public bool isMoving = false;
    public bool isSkillUsing = false;
    public bool isSkillCasting = false;
    public bool isJumping = false;
    public bool startJump = false;
    public bool fallJump = false;
    public bool endJump = false;
    public bool isAttacking = false;
    public bool isEvading = false;
    public bool isPreAttacking = false;
    public bool isKnockStun = false;

    [Header("碰撞判定")]
    public bool isGrounding = false;
    public bool groundTouch = false;
    public float collisionRadius = 0.3f;
    public Vector2 bottomOffset = new Vector2(0, -1);
    private LayerMask groundLayer;

    [Header("攻擊細節")]
    public int cycleAttackCount = 2;    // 每一輪攻擊次數的循環 (完成一連串攻擊動作的總需求次數)
    public int attackComboCount = 0;         // 當前累積的攻擊次數
    public float attackResetDuration = 1f;    // 在前一攻擊完畢後N秒內，可以接續攻擊，若沒有接續攻擊，則會重置連擊次數
    public float attackDelayDuration = 1f;    // 每一次攻擊後的延遲
    public float attackCycleDuration = 1f;    // 每一輪攻擊後的延遲  
    private float attackFinishedTime;    // 每個攻擊動畫撥放完預期的時間
    private float nextAttackResetTime;
    private int _attackAnimNumber = 0;

    [Header("閃避細節")]
    public float evadeCoolDownDuration;

    private Operations operations;   // 行動列，將按下的動作儲存
    private OperationDelayDictionary delayOperationDict;      // 記錄任何有延遲時間的動作 (延遲時間將以動作個別計算)

    /// <summary>
    /// 第N次攻擊所播放的動畫
    /// </summary>
    public int AttackAnimNumber 
    {
        get
        {            
            return _attackAnimNumber;
        }
        set
        {
            _attackAnimNumber = value;
            if (_attackAnimNumber > cycleAttackCount)
                _attackAnimNumber = 1;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        character = GetComponent<Character>();
        cycleAttackCount = GetComponent<Character>().data.cycleAttackCount;
        operations = new Operations();
        delayOperationDict = new OperationDelayDictionary();
        nextAttackResetTime = 0;
        attackComboCount = 0;
        groundLayer = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        StartOperation();

        if (anim != null)
        {
            anim.SetBool(AnimationKeyWordDictionary.jump, isJumping);
            anim.SetBool(AnimationKeyWordDictionary.startJump, startJump);
            anim.SetBool(AnimationKeyWordDictionary.fallJump, fallJump);
            anim.SetBool(AnimationKeyWordDictionary.endJump, endJump);

            anim.SetBool(AnimationKeyWordDictionary.evade, isEvading);

            anim.SetBool(AnimationKeyWordDictionary.useSkill, isSkillUsing);
            anim.SetBool(AnimationKeyWordDictionary.castSkill, isSkillCasting);

            anim.SetBool(AnimationKeyWordDictionary.preAttack, isPreAttacking);
            anim.SetBool(AnimationKeyWordDictionary.attack, isAttacking);
            anim.SetInteger(AnimationKeyWordDictionary.attackCount, AttackAnimNumber);

            anim.SetBool(AnimationKeyWordDictionary.knockStun, isKnockStun);
        }
    }

    private void LateUpdate()
    {
        CheckStun();
        CheckIdle();
        CheckGrounded();
        CheckResetAttack();
    }

    #region StartAnim
    public void StartMoveAnim(float horizontalMove)
    {
        if (anim == null)
            return;
        anim.SetFloat(AnimationKeyWordDictionary.moveSpeed, Mathf.Abs(horizontalMove));
    }

    public void StartJumpAnim(Action jumpMethod, bool forceActToEnd = false)
    {
        string operationName = MethodBase.GetCurrentMethod().Name;
        // 檢查動作列狀態
        if (CheckOperation(operationName))
        {
            Operation jump = new Operation(operationName);
            jump.SetAction(StartTrueJump(jump.SetOperationState, jumpMethod));
            jump.SetDelay(0);
            AddOperation(jump, forceActToEnd);
        }
    }

    public void StartEvadeAnim(Action evadeMethod, bool forceActToEnd = false)
    {
        string operationName = MethodBase.GetCurrentMethod().Name;
        // 檢查動作列狀態
        if (CheckOperation(operationName))
        {
            Operation evade = new Operation(operationName);
            evade.SetAction(StartTrueEvade(evade.SetOperationState, evade.SetDelay, evadeMethod));
            AddOperation(evade, forceActToEnd);
        }
    }

    /// <param name="skillUseDurtaion">技能施放持續的時間(與使用技能的動作動畫時間不同)</param>
    public void StartUseSkillAnim(Action skillCastMethod, Action skillUseMethod, float castTime, float skillUseDurtaion, bool forceActToEnd = false)
    {
        if (skillUseMethod == null)
            return;

        string operationName = MethodBase.GetCurrentMethod().Name;
        // 檢查動作列狀態
        if (CheckOperation(operationName))
        {
            Operation skillUse = new Operation(operationName);
            skillUse.SetAction(StartTrueSkillUse(skillCastMethod, skillUseMethod, castTime, skillUseDurtaion));
            AddOperation(skillUse, forceActToEnd);
        }
    }

    public bool StartAttackAnim(Func<bool> attackMethod, bool forceActToEnd = false)
    {
        bool attackSuccess = false;
        string operationName = MethodBase.GetCurrentMethod().Name;
        // 檢查動作列狀態
        if (CheckOperation(operationName))
        {
            Operation attack = new Operation(operationName);
            attack.SetAction(StartTrueAttack(attack.SetOperationState, attack.SetDelay, attackMethod));
            AddOperation(attack, forceActToEnd);
            attackSuccess = true;
        }

        return attackSuccess;
    }
    #endregion

    #region Check
    private void CheckIdle()
    {
        isIdle = anim.GetCurrentAnimatorStateInfo(0).IsName(AnimationKeyWordDictionary.idle);
    }

    private void CheckStun()
    {
        if (character.isKnockStun && !isKnockStun)
        {
            isKnockStun = true;
            InterruptAnimOperation();
            character.LockOperation(LockType.Stun, true);
        }
        if (!character.isKnockStun && isKnockStun)
        {
            isKnockStun = false;
            character.LockOperation(LockType.Stun, false);
        }
    }

    private void CheckGrounded()
    {
        character.operationController.isGrounding = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);

        if (isGrounding && !groundTouch)
        {
            // Play groundTouch particle
            groundTouch = true;
        }

        if (!isGrounding && groundTouch)
        {
            groundTouch = false;
        }
    }

    private void CheckResetAttack()
    {
        // 當不在攻擊中，且確認玩家在攻擊間隔內是否有再次攻擊，否則攻擊次數歸0
        if (!isAttacking && !isPreAttacking && Time.time >= nextAttackResetTime && attackComboCount > 0)
        {
            AttackAnimNumber = 0;
            attackComboCount = 0;
            if (anim != null)
            {
                anim.SetInteger(AnimationKeyWordDictionary.attackCount, attackComboCount);
            }
        }
    }
    #endregion

    #region IEnumerator Anim Instances
    private IEnumerator StartTrueJump(Action<OperationStateType> setOperationState, Action jumpMethod)
    {
        isJumping = true;
        
        setOperationState(OperationStateType.Combo);        
        jumpMethod.Invoke();

        while (true)
        {
            if (rb.velocity.y > -1 && rb.velocity.y <= 0 && fallJump)
            {
                startJump = false;
                fallJump = false;
                endJump = true;
                yield return new WaitForSeconds(GetFrameTimeOffset(2));
                yield return new WaitForSeconds(AnimationBase.Instance.GetCurrentAnimationLength(anim) - GetFrameTimeOffset(2));
                endJump = false;
                break;
            }
            if (rb.velocity.y > 0 && !startJump)
            {
                startJump = true;
                fallJump = false;
            }
            if (rb.velocity.y <= -1 && !fallJump)
            {
                startJump = false;
                fallJump = true;
            }
            yield return null;
        }

        isJumping = false;
    }

    private IEnumerator StartTrueEvade(Action<OperationStateType> setOperationState, Action<float> setDelay, Action evadeMethod)
    {
        isEvading = true;
        character.GetIntoImmune(0.4f);

        // 重置閃避時間
        ResetEvadeCoolDownDuration();
        setDelay(evadeCoolDownDuration);

        // 鎖定攻擊、移動、方向
        character.attack.Lock(LockType.OperationAction);
        character.move.Lock(LockType.OperationAction);
        character.freeDirection.Lock(LockType.OperationAction);

        setOperationState(OperationStateType.Interrupt);
        yield return new WaitForSeconds(GetFrameTimeOffset(1));   // 等待一幀，使動畫開始撥放，否則會取到上一個動畫的狀態。

        evadeMethod.Invoke();

        float evadeAnimDuration = AnimationBase.Instance.GetCurrentAnimationLength(anim);
        yield return new WaitForSeconds(evadeAnimDuration * 0.7f);    // 等待動畫播放至準備收尾

        character.attack.UnLock(LockType.OperationAction); // 收尾動作的時候才可以開始攻擊

        yield return new WaitForSeconds(evadeAnimDuration * 0.3f);    // 等待動畫播放結束
        isEvading = false;
    }

    private IEnumerator StartTrueAttack(Action<OperationStateType> setOperationState, Action<float> setDelay, Func<bool> attackMethod)
    {
        /// 攻擊前搖
        isPreAttacking = true;

        // 鎖定移動
        character.move.Lock(LockType.OperationAction);

        // 重置攻擊時間間隔
        AttackAnimNumber++;
        ResetAttackDelayDuration();
        setDelay(attackDelayDuration);

        // 若是最後一段的攻擊動作，就無法預存下一個攻擊動作。
        if (!CheckIsFinalAttack())
            setOperationState(OperationStateType.Trigger);
        else
            setOperationState(OperationStateType.Interrupt);
        yield return new WaitForSeconds(GetFrameTimeOffset(1));   // 等待一幀，使動畫開始撥放，否則會取到上一個動畫的狀態。

        // 攻速過快，動畫時間縮短
        float preAttackAnimDuration = AnimationBase.Instance.GetCurrentAnimationLength(anim);
        if (attackCycleDuration < preAttackAnimDuration)
            preAttackAnimDuration = attackCycleDuration;
        yield return new WaitForSeconds(preAttackAnimDuration);    // 等待動畫播放結束
        
        /// 攻擊中
        isPreAttacking = false;
        isAttacking = true;

        // 鎖定跳躍與閃避、方向
        character.jump.Lock(LockType.OperationAction);
        character.evade.Lock(LockType.OperationAction);
        character.freeDirection.Lock(LockType.OperationAction);

        if (!CheckIsFinalAttack())
        {
            setOperationState(OperationStateType.Continuous);
        }
        else
        {
            setOperationState(OperationStateType.None);
        }
        yield return new WaitForSeconds(GetFrameTimeOffset(2));

        // 累積攻擊次數+1
        attackComboCount++;

        // 攻擊觸發
        character.operationSoundController.PlaySound(character.operationSoundController.attackSound);
        if (attackMethod.Invoke())
        {
            character.operationSoundController.PlaySound(character.operationSoundController.hitSound);
        }

        // 攻速過快，動畫時間縮短
        float attackAnimDuration = AnimationBase.Instance.GetCurrentAnimationLength(anim);
        if (attackCycleDuration < attackAnimDuration)
            attackAnimDuration = attackCycleDuration;
        
        // 儲存下次攻擊重置時間
        attackFinishedTime = Time.time + attackAnimDuration;
        nextAttackResetTime = attackFinishedTime + attackResetDuration;

        float comboDuration = GetFrameTimeOffset(8);
        yield return new WaitForSeconds(attackAnimDuration - comboDuration);

        // 攻擊收尾，可連段
        setOperationState(OperationStateType.Combo);           

        yield return new WaitForSeconds(comboDuration);

        isAttacking = false;
    }

    private IEnumerator StartTrueSkillUse(Action skillCastMethod, Action skillUseMethod, float castTime, float skillUseDurtaion)
    {
        // 鎖定行動
        character.LockOperation(LockType.OperationAction, true);

        if (skillCastMethod != null)
        {
            isSkillCasting = true;
            skillCastMethod.Invoke();
            yield return new WaitForSeconds(GetFrameTimeOffset(1));

            // 計時(詠唱中)
            float timer = 0;
            while (timer < castTime)
            {
                timer += GetFrameTimeOffset(1);
                // Render Casting GUI
                yield return new WaitForSeconds(GetFrameTimeOffset(1));
            }
        
            isSkillCasting = false;
        }

        // 詠唱結束，施放技能
        skillUseMethod.Invoke();

        isSkillUsing = true;
        yield return new WaitForSeconds(GetFrameTimeOffset(1));
        if (skillUseDurtaion > 0)
        {
            // 計時(持續施放中)
            float timer = 0;
            while (timer < skillUseDurtaion)
            {
                timer += GetFrameTimeOffset(1);
                // Render Casting GUI
                yield return new WaitForSeconds(GetFrameTimeOffset(1));
            }
        }
        isSkillUsing = false;
    }
    #endregion

    #region Operation Control
    private void StartOperation()
    {
        if (operations.Count == 0)
            return;

        // 本次動作尚未開始，並等待每個動作的獨立延遲，可開始動作。
        Operation operation = operations.GetCurrentOperation();
        if (!operation.running && !operation.stop)
        {
            StartCoroutine(operation.CallOperation());
            delayOperationDict.AddOperation(operation.name, operation);
        }

        if (operation.finished)
        {
            character.LockOperation(LockType.OperationAction, false);    // 重置角色動作
            InterruptAnimOperation();    // 重置角色動畫
            operations.RemoveCurrentOperation();
            return;
        }
    }

    /// <summary>
    /// 新增代辦動作
    /// </summary>
    private void AddOperation(Operation operation, bool forceActToEnd = false)
    {
        // 敵人的部分，動作會強制執行完(避免AI其他動作中斷)
        if (forceActToEnd || this.gameObject.CompareTag("Enemy"))
        {
            operation.ForceActToEnd();
        }
        operations.AddOperation(operation);
    }

    private bool CheckOperation(string operationName)
    {
        if (!CheckOperationDelay(operationName))
            return false;

        int stackCount = operations.Count;
        if (stackCount == 0)
        {
            return true;
        }

        Operation lastOperation = operations.GetLastOperation();
        // 若為正在執行的動作，則檢查狀態，決定是否要打斷或是等待
        // 若否，則先檢查是否為相同動作，不是則會被其他動作覆蓋
        if (lastOperation.running)
        {
            return lastOperation.CheckOperationState(operationName);
        }
        else
        {
            if (!lastOperation.CheckOperationSame(operationName))
            {
                operations.RemoveLastOperation();
                return true;
            }
        }

        return false;
    }

    private bool CheckOperationDelay(string operationName)
    {
        // 如果沒有動作紀錄，代表不需等待延遲。
        if (!delayOperationDict.ContainsKey(operationName))
            return true;

        Operation previousOperation = delayOperationDict[operationName];
        if (Time.time >= previousOperation.nextOperationTime)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 動作動畫中止
    /// </summary>
    public void InterruptAnimOperation()
    {
        isSkillUsing = false;
        isSkillCasting = false;

        isJumping = false;
        startJump = false;
        fallJump = false;
        endJump = false;

        isEvading = false;

        isPreAttacking = false;
        isAttacking = false;
    }
    #endregion

    #region Others
    /// <summary>
    /// 攻擊重置的時間間隔，會根據攻擊後延遲調整
    /// </summary>
    private void ResetAttackDelayDuration()
    {
        attackCycleDuration = character.data.attackDelay.Value;
        if (AttackAnimNumber == cycleAttackCount)
        {
            attackDelayDuration = attackCycleDuration;
        }
        else
        {
            attackDelayDuration = attackCycleDuration / 5;
        }
        if (attackCycleDuration < 0.02f)
        {
            attackCycleDuration = 0.02f;
        }

        if (attackCycleDuration >= attackResetDuration)
        {
            attackResetDuration = attackCycleDuration;
        }
        else if (attackResetDuration != 1f)
        {
            attackResetDuration = 1;
        }
    }

    private bool CheckIsFinalAttack()
    {
        return AttackAnimNumber == cycleAttackCount;
    }

    /// <summary>
    /// 閃避重置的時間間隔
    /// </summary>
    private void ResetEvadeCoolDownDuration()
    {
        evadeCoolDownDuration = character.data.evadeCoolDown.Value;
    }

    /// <summary>
    /// 目押的時間間隔 (從該動作【動畫撥放結束前】到【動畫播放完】的時間差)
    /// </summary>
    /// <param name="frame">目押的幀數</param>
    private float GetFrameTimeOffset(int frame)
    {
        return Time.deltaTime * frame;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // 地板偵測
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
    }
    #endregion
}