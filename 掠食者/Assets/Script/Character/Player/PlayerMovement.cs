using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Character character;
    private Rigidbody2D rb;

    [Header("移動")]
    public Vector2 moveDir;
    public float x;
    public float y;
    public float basicMoveSpeed = 2.5f;

    [Header("跳躍")]
    public float basicJumpForce = 6f;
    public float lowJumpMultiplier = 4f;
    public float fallMultiplier = 2f;
    public float skyWalkLimitDuration = 1f;
    private float nextSkyWalkFallenTimes;
    private bool startSkyWalk;

    [Header("閃避")]
    public float basicEvadeSpeed = 6f; 

    private void Start()
    {
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();
        startSkyWalk = false;
    }

    private void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        moveDir = new Vector2(x, y);

        SideChange();
        HoriMove();
        Jump();
        BetterJump();
        Evade();
        ResetSkyWalk();
    }

    private void SideChange()
    {
        if (!character.freeDirection.canDo)
            return;

        if (x > 0)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (x < 0)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void HoriMove()
    {
        if (character.move.canDo)
        {
            character.operationController.StartMoveAnim(x);
            rb.velocity = new Vector2(moveDir.x * character.data.moveSpeed.Value * basicMoveSpeed, rb.velocity.y);
        }
        else
        {
            if (character.operationController.isEvading || character.operationController.isSkillCasting || character.operationController.isSkillUsing)
                return;

            // 若不能移動，則會隨慣性移動至停止
            if (rb.velocity.x > 0)
            {
                rb.velocity = new Vector2(1.2f * transform.right.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0.5f * transform.right.x, rb.velocity.y);
            }
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(HotKeyController.GetHotKey(HotKeyType.JumpKey)) 
            && character.jump.canDo 
            && character.operationController.isGrounding)   // 或有多段跳躍
        {
            character.operationController.StartJumpAnim(delegate { JumpAction(); });
        }
    }

    private void JumpAction()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * character.data.jumpForce.Value * basicJumpForce;
    }

    private void BetterJump()
    {
        if (character.operationController.isPreAttacking || character.operationController.isAttacking)
        {
            SkyWalk();
            return;
        }

        if (rb.velocity.y > 0 && !Input.GetKey(HotKeyController.GetHotKey(HotKeyType.JumpKey)))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void SkyWalk()
    {
        if (!startSkyWalk && !character.operationController.isGrounding)
        {
            startSkyWalk = true;
            nextSkyWalkFallenTimes = Time.time + skyWalkLimitDuration;
        }
        if (startSkyWalk)
        {
            // 滯空時間限制
            if (Time.time < nextSkyWalkFallenTimes)
            {
                rb.velocity = new Vector2(transform.right.x * 0.01f, transform.up.y * -0.05f);
            }
        }
    }

    private void ResetSkyWalk()
    {
        if (startSkyWalk && character.operationController.isGrounding)
        {
            startSkyWalk = false;
        }
    }

    private void Evade()
    {
        if (Input.GetKeyDown(HotKeyController.GetHotKey(HotKeyType.EvadeKey))
            && character.evade.canDo)
        {
            character.operationController.StartEvadeAnim(delegate { EvadeAction(); });
        }
    }

    private void EvadeAction()
    {
        rb.velocity = transform.right * new Vector2(basicEvadeSpeed * character.data.moveSpeed.Value, rb.velocity.y);
    }
}
