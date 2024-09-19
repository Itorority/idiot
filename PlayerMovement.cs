
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Header("Movement Parameters")]
    [SerializeField] private float movementSpeed = 7f;
    private float horizontalInput;
    private bool isFacingRight = true;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpHeight;
    private bool jumping;

    //Gravity manipulation
    private float fallGravityScale = 10f;

    // Coyote Handler
    [SerializeField] private float coyoteTime;
    private float coyoteCounter;

    // Components 
    private Rigidbody2D body;
    private BoxCollider2D BoxCollider2D;

    // Wall Sliding
    private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed = 2f;
    [SerializeField] private Transform wallCheck;

    // Wall Jumping 
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [Header("Layer")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }
    // Movement Handler
    private void MovementHandler() {
        horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2 (horizontalInput * movementSpeed, body.velocity.y);

    }
    // Flip the player 
    private void Flip() {
        if(isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f) {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    // Jump Handler
    private void JumpHandler() {
        if (isGrounded()) {
            body.velocity = new Vector2(body.velocity.x, jumpHeight);
            body.gravityScale = 7f;
            jumping = true;
        } else {
            if (coyoteCounter > 0) {
                body.velocity = new Vector2(body.velocity.x, jumpHeight);
            }
            coyoteCounter = 0;
        }
    }
    // Check if he player is grounded or not 
    private bool isGrounded() {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(BoxCollider2D.bounds.center, BoxCollider2D.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        if (raycastHit2D) {
            return raycastHit2D.collider != null;
        }
        return false;
    }
    // Check if the player is on the wall or not 
    private bool onWall() {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    // Wall Slide Handler
    private void WallSlide() {
        if(onWall() && !isGrounded() && horizontalInput != 0f) {
            isWallSliding = true;
            body.velocity = new Vector2(body.velocity.x, -wallSlidingSpeed); // adjust the wall sliding speed (Mathf.Clamp didn't work for some reasons)
        } else {
            isWallSliding = false;
        }
    }
    // Wall Jump Handler
    private void WallJump() {
        if (isWallSliding) {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping)); // Stop wall jumping if the conditioons didn't meet
        } else {
            wallJumpingCounter -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f) {
            isWallJumping = true;
            body.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            if (transform.localScale.x != wallJumpingDirection) {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
    }
    private void StopWallJumping() {
        isWallJumping = false;
    }
    private void Update() {
        
        MovementHandler();
        // Jump mechanic related
        if (Input.GetKeyDown(KeyCode.Space)) {
            JumpHandler();
        }
        if (isGrounded()) {
            coyoteCounter = coyoteTime; // Reset coyote counter when on the ground
        } else {
            coyoteCounter -= Time.deltaTime; // Start decreasing coyote counter when not on the ground
        }
        // adjust the fall gravity scale to the player following the fixed time
        if (body.velocity.y < 0) {
            body.gravityScale = fallGravityScale;
            jumping = false;
        }
        // Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0) {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
        }
        // Related to wall mechanic
        WallSlide();
        WallJump();
        if (!isWallJumping) {
            Flip();
        }
        Debug.Log(body.velocity.y);

    }
    private void FixedUpdate() {

    }
    private void OnDrawGizmosSelected() {
        if (wallCheck != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wallCheck.position, 0.1f); // Adjust the radius to match the one used in your code
        }
    }
}