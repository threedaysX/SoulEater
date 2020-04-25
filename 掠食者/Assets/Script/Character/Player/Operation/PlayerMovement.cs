using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Character character;
    private Rigidbody2D rb;

    [Header("移動")]
    public Vector2 moveDir;
    public float x;
    public float y;
    public float basicJumpForce = 6f;
    public float basicMoveSpeed = 4f;

    [Header("跳躍")]
    public float lowJumpMultiplier = 4f;
    public float fallMultiplier = 2f;

    public float jumpCD;

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

        if (Input.GetKeyDown(HotKeyController.evadeKey))
        {
            Evade();
        }
    }

    void SideChange()
    {
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
        if (character.operationController.isEvading)
            return;

        if (character.canMove)
        {
            character.operationController.StartMoveAnim(x);
            rb.velocity = new Vector2(moveDir.x * character.data.moveSpeed.Value * basicMoveSpeed, rb.velocity.y);
        }
        else
        {
            if (rb.velocity.x > 0)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(HotKeyController.jumpKey) 
            && character.canJump 
            && character.operationController.isGrounding)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += Vector2.up * character.data.jumpForce.Value * basicJumpForce;
            character.operationController.StartJumpAnim();
        }
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

    void Evade()
    {
        
    }
}
