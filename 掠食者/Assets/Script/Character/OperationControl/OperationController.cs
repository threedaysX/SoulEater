using UnityEngine;

public class AnimationKeyWordDictionary
{
    public const string moveSpeed = "Speed";
    public const string jump = "IsJumping";
    public const string attack = "IsAttacking";
    public const string attackCount = "AttackCount";
    public const string evade = "Evade";
    public const string useSkill = "UseSkill";
}

public class OerationController : MonoBehaviour
{
    private Character character;
    private Animator anim;

    [Header("操作判定")]
    public bool canMove = true;
    public bool canJump = true;
    public bool canEvade = true;
    public bool canAttack = true;
    public bool canSkill = true;

    [Header("操作狀態判定")]
    public bool IsJumping = false;
    public bool IsAttacking = false;
    public bool IsEvading = false;
    public bool IsGrounding = false;

    [Header("攻擊細節")]
    public float attackFinishedTime;    // 每個攻擊動畫撥放完預期的時間
    public float nextAttackResetTime;
    public float attackResetDuration = 1f;    // 在前一攻擊完畢後N秒內，可以接續攻擊，若沒有接續攻擊，則會重置攻擊次數
    public int cycleAttackCount = 2; // 每個攻擊次數的循環 (完成一連串攻擊動作的總需求次數)
    public float firstAttackTime;       // 第一次攻擊的時間
    public int attackCount = 0;         // 當前累積的攻擊次數
    private int attackCountWasted = 0;  // 當前累積浪費掉的攻擊次數
    public int lastLimitAttackCount;  // 上一秒的攻擊最大次數上限
    public int maxLimitAttackCount;   // 攻擊最大次數上限

    private void Start()
    {
        anim = GetComponent<Animator>();
        character = GetComponent<Character>();
        nextAttackResetTime = 0;
        attackCount = 0;
    }

    private void Update()
    {
        if (anim != null)
        {
            if (IsJumping)
            {
                IsJumping = false;
                anim.SetBool(AnimationKeyWordDictionary.jump, true);
            }
        }
    }

    private void LateUpdate()
    {
        if (anim != null)
        {
            CheckAttack();
        }

        CheckResetAttack();
    }

    public void StartMoveAnim(float horizontalMove)
    {
        anim.SetFloat(AnimationKeyWordDictionary.moveSpeed, Mathf.Abs(horizontalMove));
    }

    public void StartJumpAnim()
    {
        IsJumping = true;
    }

    public void StartAttackAnim()
    {
        IsAttacking = true;
        int attackAnimNumber = (attackCount % cycleAttackCount == 0) ? cycleAttackCount : (attackCount % cycleAttackCount);
        anim.SetBool(AnimationKeyWordDictionary.attack, true);
        anim.SetInteger(AnimationKeyWordDictionary.attackCount, attackAnimNumber);
    }
    private void CheckAttack()
    {
        if (IsAttacking)
        {
            float attackAnimTime = AnimationBase.Instance.GetCurrentAnimationLength(anim);
            attackFinishedTime = Time.time + attackAnimTime;
            nextAttackResetTime = attackFinishedTime + attackResetDuration;
            IsAttacking = false;
        }
        else
        {
            if (attackFinishedTime != 0 && Time.time >= attackFinishedTime)
            {
                attackFinishedTime = 0;
                anim.SetBool(AnimationKeyWordDictionary.attack, false);
            }
        }
    }

    private void CheckResetAttack()
    {
        // 確認玩家在攻擊間隔內是否有再次攻擊，否則攻擊次數歸0
        if (Time.time >= nextAttackResetTime && attackCount > 0)
        {
            attackCount = 0;
            lastLimitAttackCount = 0;
            maxLimitAttackCount = 0;
            firstAttackTime = 0;
            attackCountWasted = 0;
            if (anim != null)
            {
                anim.SetInteger(AnimationKeyWordDictionary.attackCount, attackCount);
            }
        }
    }

    public bool StartAttack(AttackType attackType = AttackType.Attack, ElementType elementType = ElementType.None)
    {
        float normalizeTime = Time.time;
        bool attackSuccess = false;
        if (character.operationController.canAttack)
        {
            // 重置第一次攻擊時間
            if (firstAttackTime == 0)
            {
                firstAttackTime = normalizeTime;
            }

            // 重置攻擊時間間隔
            float attackSpeed = character.data.attackSpeed.Value;
            ResetAttackDuration();

            // 當前已經過秒數，所可以打到的攻擊次數上限
            float timePassed = normalizeTime - firstAttackTime;
            maxLimitAttackCount = (int)Mathf.Floor(timePassed <= 1 ? attackSpeed : Mathf.Ceil(timePassed) * attackSpeed);
            lastLimitAttackCount = (int)(Mathf.Ceil(timePassed - 1) * attackSpeed);

            // Attack，在最大攻擊上限次數內才可以開始攻擊
            if (attackCount + attackCountWasted < lastLimitAttackCount)
            {
                attackCountWasted = lastLimitAttackCount - attackCount; // 填滿所有未進行攻擊的次數 (補滿浪費掉的攻擊次數)
            }
            if (attackCount + attackCountWasted < maxLimitAttackCount)
            {
                attackCount++;  // 累積攻擊次數+1
                StartAttackAnim();
                character.operationSoundController.PlaySound(character.operationSoundController.attackSound);
                attackSuccess = character.combat.Attack(attackType, elementType);
                if (attackSuccess)
                {
                    character.operationSoundController.PlaySound(character.operationSoundController.hitSound);
                }
            }
        }

        return attackSuccess;
    }

    /// <summary>
    /// 攻擊重置的時間間隔，會根據攻擊速度調整
    /// </summary>
    private void ResetAttackDuration()
    {
        attackResetDuration = (1 / character.data.attackSpeed.Value) * 2;
    }
}