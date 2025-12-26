using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("跳躍設定")]
    public float coyoteTime = 0.1f;
    public int maxJumpCount = 2;
    public int jumpCount = 2;
    private float coyoteTimeCounter;
    public bool wasGrounded;

    [Header("地板偵測")]
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public LayerMask windAreaLayer;
    public bool isGrounded;
    //public bool wasGrounded;
    public bool inWindArea;
    public float windForce = 5f;

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Animator anim;

    private float horizontalInput;
    //private float coyoteTimeCounter;
    private bool facingRight = false;
    public bool hasKey = false;


    [Header("Spawn Point Info")]
    public Vector2[] spawnPoints = {
        new Vector2(-3.8f, 3.19f),

    };
    public int currentLevel = 0; 


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    void Update()
    {
        horizontalInput = moveAction.ReadValue<Vector2>().x;

        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
        inWindArea = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, windAreaLayer);

        if (transform.position.y <= -2)
        {
            Death();
        }

        // Coyote time logic
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            if (!wasGrounded) // Only reset jumpCount when actually landing
            {
                jumpCount = maxJumpCount;
            }
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        tryJumping();
        wasGrounded = isGrounded;



    }

    [Header("Wall Check")]
    public float wallCheckDistance = 0.2f;
    public float characterHeight = 1f;  // Adjust this to match your character's height

    private bool IsWallAhead()
    {
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        
        // Bottom raycast
        Vector2 bottomStart = (Vector2)transform.position + Vector2.down * (characterHeight / 2);
        RaycastHit2D bottomHit = Physics2D.Raycast(bottomStart, direction, wallCheckDistance, groundLayer);
        
        // Top raycast
        Vector2 topStart = (Vector2)transform.position + Vector2.up * (characterHeight / 2);
        RaycastHit2D topHit = Physics2D.Raycast(topStart, direction, wallCheckDistance, groundLayer);
        
        // Center raycast
        RaycastHit2D centerHit = Physics2D.Raycast(transform.position, direction, wallCheckDistance, groundLayer);

        // Draw debug rays in Scene view
        Debug.DrawRay(bottomStart, direction * wallCheckDistance, Color.red);
        Debug.DrawRay(topStart, direction * wallCheckDistance, Color.red);
        Debug.DrawRay(transform.position, direction * wallCheckDistance, Color.red);

        return bottomHit.collider != null || topHit.collider != null || centerHit.collider != null;
    }

    void FixedUpdate()
    {
        bool isMoving = horizontalInput != 0;
        anim.SetBool("IsMoving", isMoving);

        if (!IsWallAhead() || (IsWallAhead() && 
            ((facingRight && horizontalInput < 0) || (!facingRight && horizontalInput > 0))))
        {
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            // Stop horizontal movement when hitting a wall
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        if (horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }

        if (inWindArea)
        {
            rb.AddForce(Vector2.left * windForce, ForceMode2D.Force);
        }
    }

    private void Death()
    {
        rb.linearVelocity = new Vector2(0f, 0f);
        Vector2 firstPoint = spawnPoints[0];
        float x = firstPoint.x;
        float y = firstPoint.y;
        transform.position = new Vector2(x, y);
    }

    private void tryJumping()
    {
        if (jumpAction.triggered)
        {
            // Allow jump if we have jumps left or within coyote time
            if ((jumpCount > 0 && isGrounded) || coyoteTimeCounter > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jumpCount--;
                coyoteTimeCounter = 0f; // Prevent further coyote jumps until grounded again
            }
        }

    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    
    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}