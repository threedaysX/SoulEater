using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Character character;
    private Rigidbody2D rb;

    [Header("移動")]
    public float horizontalMove;
    public float basicMoveSpeed = 4f;

    [Header("跳躍")]
    public float lowJumpMultiplier;
    public float fallMultiplier;
    public float fallAcceelerator;

    private LayerMask groundLayer;

    public float jumpCD;

    private float collisionRadius = 0.25f;
    public Vector2 bottomOffset;

    void Start()
    {
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * character.data.moveSpeed.Value * basicMoveSpeed;
        character.operationController.StartMoveAnim(horizontalMove);

        SideChange();

        character.operationController.IsGrounding = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
    }

    private void FixedUpdate()
    {
        HoriMove();
        Jump();
        BetterJump();

        if (Input.GetKeyDown(HotKeyController.evadeKey))
        {
            Evade();
        }
    }

    void SideChange()
    {
        if (horizontalMove > 0)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (horizontalMove < 0)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void HoriMove()
    {
        Vector3 movementX = new Vector3(horizontalMove, 0, 0);
        transform.position += movementX * Time.fixedDeltaTime;
    }

    void Jump()
    {
        
    }

    void BetterJump()
    {
        if (rb.velocity.y < 0 && Input.GetKey(HotKeyController.moveDown))
        {
            rb.gravityScale = fallAcceelerator;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(HotKeyController.jumpKey))
        {
            rb.gravityScale = lowJumpMultiplier;
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallMultiplier;
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }


    void Evade()
    {
        
    }
}
