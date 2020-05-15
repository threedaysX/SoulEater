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

    [Header("閃避")]
    public float basicEvadeSpeed = 6f; 

    void Start()
    {
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        moveDir = new Vector2(x, y);

        SideChange();
        HoriMove();
        Jump();
        BetterJump();
        Evade();
    }

    void SideChange()
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
                rb.velocity = new Vector2(0.6f, rb.velocity.y);
            }
            else if (rb.velocity.x < 0)
            {
                rb.velocity = new Vector2(-0.6f, rb.velocity.y);
            }
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(HotKeyController.jumpKey) 
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
        if (rb.velocity.y > 0 && !Input.GetKey(HotKeyController.jumpKey))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Evade()
    {
        if (Input.GetKeyDown(HotKeyController.evadeKey)
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
