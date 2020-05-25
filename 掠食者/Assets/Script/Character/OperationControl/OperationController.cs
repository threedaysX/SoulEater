using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationKeyWordDictionary
{
    public const string moveSpeed = "Speed";
    public const string jump = "IsJumping";
    public const string preAttack = "IsPreAttacking";
    public const string attack = "IsAttacking";
    public const string attackCount = "AttackCount";
    public const string evade = "Evade";
    public const string useSkill = "IsSkillUsing";
    public const string castSkill = "IsSkillCasting";
    public const string run = "Run";
    public const string idle = "Idle";
}

public class Operation
{
    public IEnumerator action;          // 當前行動的方法
    public bool canInterruptOperation;
    public float delay;     // 當前行動結束後的延遲時間 (延遲進行下一個行動)
    public bool finished;
    public bool paused;
    public bool running;

    public Operation(IEnumerator action, float delay)
    {
        this.action = action;
        this.delay = delay;
    }

    public IEnumerator CallOperation()
    {
        Start();
        IEnumerator e = action;
        while (running)
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
                }
            }
        }
        finished = true;
    }

    private void Start()
    {
        running = true;
        finished = false;
    }

    public void Stop()
    {
        running = false;
    }

    public void Pause()
    {
        paused = true;
    }
    public void Unpause()
    {
        paused = false;
    }
}

public class OperationController : MonoBehaviour
{
    private Character character;
    private Animator anim;

    [Header("操作狀態判定")]
    public bool isSkillUsing = false;
    public bool isSkillCasting = false;
    public bool isJumping = false;
    public bool isAttacking = false;
    public bool isEvading = false;
    public bool isPreAttacking = false;
    public bool isRunning = false;
    public bool isIdle = false;

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
    public float attackDelayDuration = 1f;    // 每一輪攻擊後的延遲
    private float attackFinishedTime;    // 每個攻擊動畫撥放完預期的時間
    private float nextAttackResetTime;
    private int _attackAnimNumber = 0;

    private List<Operation> operationStack;   // 行動列，將按下的動作儲存
    private float nextOperationTime;
    private bool canTriggerNextOperation = true;    // 用來判斷是否可接續其他動作
    private bool canInterruptOperation = false;     // 用來判斷此動作可不可以被其他動作打斷

    /// <summary>
    /// 第N次攻擊所播放的動畫
    /// </summary>
    private int AttackAnimNumber 
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
        anim = GetComponent<Animator>();
        character = GetComponent<Character>();
        operationStack = new List<Operation>();
        nextAttackResetTime = 0;
        attackComboCount = 0;
        groundLayer = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        CheckOperation();

        if (anim != null)
        {            
            anim.SetBool(AnimationKeyWordDictionary.jump, isJumping);
            anim.SetBool(AnimationKeyWordDictionary.useSkill, isSkillUsing);
            anim.SetBool(AnimationKeyWordDictionary.castSkill, isSkillCasting);

            anim.SetBool(AnimationKeyWordDictionary.preAttack, isPreAttacking);
            anim.SetBool(AnimationKeyWordDictionary.attack, isAttacking);
            anim.SetInteger(AnimationKeyWordDictionary.attackCount, AttackAnimNumber);
        }
    }

    private void LateUpdate()
    {
        CheckMove();
        CheckJump();
        CheckAttack();
        CheckEvade();
        CheckGrounded();
        CheckResetAttack();

        if (gameObject.CompareTag("Player"))
        {
            CheckRunning();
            CheckIdle();
        }
    }

    #region StartAnim
    public void StartMoveAnim(float horizontalMove)
    {
        anim.SetFloat(AnimationKeyWordDictionary.moveSpeed, Mathf.Abs(horizontalMove));
    }

    public void StartJumpAnim()
    {
        isJumping = true;
    }

    public void StartUseSkillAnim()
    {
        isSkillUsing = true;
        character.operationSoundController.PlaySound(character.operationSoundController.useSkillSound);
    }

    public void StopUseSkillAnim()
    {
        isSkillUsing = false;
        character.operationSoundController.StopSound();
    }

    public void StartCastSkillAnim(float duration)
    {
        isSkillCasting = true;
        AnimationBase.Instance.PlayAnimationLoop(anim, "Casting", duration);
    }
    #endregion

    #region Check
    private void CheckMove()
    {
        if (isEvading)
        {
            character.canMove = false;
        }

        if ((isAttacking || isPreAttacking) && character.canMove)
        {
            character.canMove = false;
        }

        if ((isSkillUsing || isSkillCasting) && character.canMove)
        {
            character.canMove = false;
        }

        if (!isAttacking && !isPreAttacking && !isSkillUsing && !isSkillCasting && !isEvading && !character.canMove)
        {
            character.canMove = true;
        }
    }

    private void CheckJump()
    {      
        if (isAttacking || isSkillUsing || isSkillCasting && character.canJump)
        {
            character.canJump = false;
        }

        if (!isAttacking && !isSkillUsing && !isSkillCasting && !character.canJump)
        {
            character.canJump = true;
        }
    }

    private void CheckAttack()
    {
        if (isPreAttacking || isAttacking)
        {
            isJumping = false;
            isEvading = false;
        }

        if ((isSkillUsing || isSkillCasting) && character.canAttack)
        {
            character.canAttack = false;
        }

        if (!isSkillUsing && !isSkillCasting && !character.canAttack)
        {
            character.canAttack = true;
        }
    }

    private void CheckEvade()
    {
        if (isAttacking && character.canEvade)
        {
            character.canEvade = false;
        }

        if ((isSkillUsing || isSkillCasting) && character.canEvade)
        {
            character.canEvade = false;
        }

        if (!isAttacking && !isSkillUsing && !isSkillCasting && !character.canEvade)
        {
            character.canEvade = true;
        }
    }
    private void CheckRunning()
    {
        isRunning = anim.GetCurrentAnimatorStateInfo(0).IsName(AnimationKeyWordDictionary.run);
    }

    private void CheckIdle()
    {
        isIdle = anim.GetCurrentAnimatorStateInfo(0).IsName(AnimationKeyWordDictionary.idle);
    }

    private void CheckGrounded()
    {
        character.operationController.isGrounding = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);

        if (isGrounding && !groundTouch)
        {
            // Play groundTouch particle
            groundTouch = true;
            isJumping = false;
        }

        if (!isGrounding && groundTouch)
        {
            groundTouch = false;
        }
    }

    private void CheckOperation()
    {
        if (operationStack.Count > 0)
        {
            StartOperation();
        }
    }

    private void CheckResetAttack()
    {
        // 當不在攻擊中，且確認玩家在攻擊間隔內是否有再次攻擊，否則攻擊次數歸0
        if (!isAttacking && !isPreAttacking && Time.time >= nextAttackResetTime && attackComboCount > 0 && operationStack.Count == 0)
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

    #region Attack
    public bool StartAttack(AttackType attackType = AttackType.Attack, ElementType elementType = ElementType.None)
    {
        bool attackSuccess = false;
        if (character.canAttack)
        {
            // 每段攻擊後延遲判定 (按鍵預存)
            if (CheckCanTriggerNextOperation())
            {
                // 重置攻擊時間間隔
                ResetAttackDelayDuration();
                AddOperation(StartTrueAttack(attackType, elementType), attackDelayDuration);
                attackSuccess = true;
            }
        }

        return attackSuccess;
    }

    private IEnumerator StartTrueAttack(AttackType attackType = AttackType.Attack, ElementType elementType = ElementType.None)
    {
        #region PreAttack
        isPreAttacking = true;
        canTriggerNextOperation = true;
        canInterruptOperation = true;
        AttackAnimNumber++;

        yield return new WaitForEndOfFrame();   // 等待一幀，使動畫開始撥放，否則會取到上一個動畫的狀態。
        yield return new WaitForSeconds(AnimationBase.Instance.GetCurrentAnimationLength(anim));    // 等待動畫播放結束

        canInterruptOperation = false;
        isPreAttacking = false;
        #endregion
        #region Attack
        isAttacking = true;
        yield return new WaitForEndOfFrame();

        // 累積攻擊次數+1
        attackComboCount++;

        character.operationSoundController.PlaySound(character.operationSoundController.attackSound);
        if (character.combatController.Attack(attackType, elementType))
        {
            character.operationSoundController.PlaySound(character.operationSoundController.hitSound);
        }

        float attackAnimDuration = AnimationBase.Instance.GetCurrentAnimationLength(anim);
        float aimStopOffset = GetAimStopOffset(6);
        yield return new WaitForSeconds(attackAnimDuration - aimStopOffset);

        canTriggerNextOperation = false;

        yield return new WaitForSeconds(aimStopOffset);

        attackFinishedTime = Time.time + attackAnimDuration;
        nextAttackResetTime = attackFinishedTime + attackResetDuration;
        isAttacking = false;
        #endregion
    }
    #endregion

    private void StartOperation()
    {
        if (!operationStack[0].running && Time.time >= nextOperationTime)
        {
            StartCoroutine(operationStack[0].CallOperation());
        }

        // 若上一個動作執行完
        if (operationStack[0].finished)
        {
            nextOperationTime = Time.time + operationStack[0].delay;
            operationStack.RemoveAt(0);
        }
    }

    /// <summary>
    /// 新增代辦動作
    /// </summary>
    private void AddOperation(IEnumerator action, float delay)
    {
        Operation operation = new Operation(action, delay);
        operationStack.Add(operation);
    }

    private bool CheckCanTriggerNextOperation()
    {
        if (operationStack.Count == 0)
            canTriggerNextOperation = true;

        if (canTriggerNextOperation)
        {
            canTriggerNextOperation = false;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 動作中止
    /// </summary>
    public void InterruptOperation(Operation operation)
    {
        isSkillUsing = false;
        isSkillCasting = false;
        isJumping = false;
        isAttacking = false;
        isEvading = false;
        isPreAttacking = false;
        operation.Stop();
    }

    /// <summary>
    /// 攻擊重置的時間間隔，會根據攻擊速度調整
    /// </summary>
    private void ResetAttackDelayDuration()
    {
        attackDelayDuration = character.data.attackDelay.Value;

        if (attackDelayDuration >= attackResetDuration)
        {
            attackResetDuration = attackDelayDuration;
        }
        else if (attackResetDuration != 1f)
        {
            attackResetDuration = 1;
        }
    }

    /// <summary>
    /// 目押的時間間隔 (從該動作【動畫撥放結束前】到【動畫播放完】的時間差)
    /// </summary>
    /// <param name="frame">目押的幀數</param>
    private float GetAimStopOffset(int frame)
    {
        return Time.deltaTime * frame;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
    }
}