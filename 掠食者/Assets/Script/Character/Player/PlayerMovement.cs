using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Character character;
    private Rigidbody2D rb;

    private float xRaw;

    public int side = 1;       //right

    public int dashSpeed = 1;
    public float dashCD = 1;
    public float startDashCD = 1;
    private bool canDash = true;
    public bool endDash = false;

    public float lowJumpMultiplier;
    public float fallMultiplier;
    public float fallAcceelerator;

    private LayerMask groundLayer;
    private bool IsGrounded;

    private bool canJump;
    private float nextTimeJump;
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
        //Debug.Log(dashCD);

        xRaw = Input.GetAxisRaw("Horizontal");

        SideChange();

        IsGrounded = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);

        dashCD -= Time.deltaTime;
        if (dashCD <= -5) { dashCD = -5; }
        canDash = dashCD <= 0 ? true : false;
        endDash = dashCD < 0 && dashCD > -0.3f ? true : false;


    }

    private void FixedUpdate()
    {
        HoriMove();
        Jump();
        BetterJump();
        //Bounce();

        if (Input.GetKeyDown(HotKeyController.evadeKey) && canDash)
        {
            Evade();
            //Debug.Log(side * dashSpeed);
        }

        if (endDash == true)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

    }

    void SideChange()
    {
        if (xRaw == 1)
        {
            side = 1;
        }
        else if (xRaw == -1)
        {
            side = -1;
        }
    }

    void HoriMove()
    {
        Vector3 movementX = new Vector3(xRaw, 0, 0);
        transform.position += movementX * character.data.moveSpeed.Value * Time.deltaTime;
    }

    void Jump()
    {
        Vector2 movementY = new Vector2(0, character.data.jumpForce.Value);
        if (Input.GetKeyDown(HotKeyController.jumpKey) && IsGrounded)
        {
            //transform.position += movementY * Time.deltaTime;
            rb.velocity = Vector2.zero;
            rb.velocity = movementY;
        }
    }

    void BetterJump()
    {
        if (dashCD > 0.2f)
        {
            rb.gravityScale = 0f;
        }
        else if (rb.velocity.y < 0 && Input.GetKey(HotKeyController.moveDown))
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

    void Bounce()
    {
        if (rb.velocity.y < -12 && IsGrounded)
        {
            rb.velocity = Vector2.zero;
            rb.velocity = new Vector2(0, 5f);

            //Debug.Log("y");
        }
    }

    void Dash(float x, float y) //not using
    {
        rb.velocity = Vector2.zero;
        Vector2 dash = new Vector2(x, 0);

        rb.velocity += dash * y * Time.deltaTime;

        //Debug.Log(dash);
    }


    void Evade()
    {
        //rb.position += new Vector2(rb.position.x + (dashSpeed * side), 0);
        //rb.AddForce(new Vector2(dashSpeed * side, 0));

        rb.velocity = Vector2.zero;
        rb.velocity += Vector2.right * dashSpeed * side;

        dashCD = startDashCD;
    }
}
