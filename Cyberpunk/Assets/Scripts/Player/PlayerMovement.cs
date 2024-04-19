using Unity.VisualScripting;
using UnityEngine;

namespace HL
{
    public class PlayerMovement : MonoBehaviour
    {
        // ======= Script Caches =======
        private Rigidbody2D rb;
        private PlayerManager player;

        // ======= Custom Editor Variables =======
        // So usually with custom editor variables, they need to be public in order for the editor to access them 
        // However, by making them a [SerializeField], they can be accessed as a SerializedProperty in the editor
        // This means you can avoid making them public but still have them accessed by the editor
        [SerializeField] private bool showRunSettings;
        [SerializeField] private bool showJumpSettings;
        [SerializeField] private bool showWallJumpSettings;
        [SerializeField] private bool showFallSettings;
        [SerializeField] private bool showCheckSettings;
        [SerializeField] private bool showAssistSettings;

        // ======= Settings =======
        [Header("Running")]
        [Tooltip("The max running speed")]
        [SerializeField] private float maxRunSpeed;
        [Tooltip("How fast we can reach our max speed")]
        [SerializeField] private float runAcceleration;
        [Tooltip("How fast we can stop")]
        [SerializeField] private float runDecceleration;
        [Tooltip("How fast we can reach our max speed in the air")]
        [SerializeField][Range(0f, 1)] private float accelInAir;
        [Tooltip("How fast we can stop in the air")]
        [SerializeField][Range(0f, 1)] private float deccelInAir;
        [Tooltip("If we exceed our max speed, should we let it happen until we slow down naturally?")]
        [SerializeField] private bool doConserveMomentum;

        [Header("Jumping")]
        [Tooltip("The height of the jump")]
        [SerializeField] private float jumpHeight;
        [Tooltip("The time it takes to reach the highest point of the jump (the apex)")]
        [SerializeField] private float jumpTimeToApex;
        [Tooltip("The gravity multiplier when we let go of the jump button before the apex")]
        [SerializeField] private float jumpCutGravityMult;
        [Tooltip("The gravity multiplier when we are at the apex")]
        [SerializeField][Range(0f, 1)] private float jumpHangGravityMult;
        [Tooltip("How long we stay at the apex")]
        [SerializeField] private float jumpHangTimeThreshold;
        [Tooltip("How fast we speed up at the apex")]
        [SerializeField] private float jumpHangAccelerationMult;
        [Tooltip("The max speed we can go at the apex")]
        [SerializeField] private float jumpHangMaxSpeedMult;

        [Header("Wall Jumping")]
        [Tooltip("The force of our wall jump, both horizontally and vertically")]
        [SerializeField] private Vector2 wallJumpForce;
        [Tooltip("How much control the player has over their movement after a wall jump")]
        [SerializeField][Range(0f, 1f)] private float wallJumpRunLerp;
        [Tooltip("How long it takes before we can wall jump again")]
        [SerializeField][Range(0.1f, 1.5f)] private float wallJumpCooldown;
        [Tooltip("Should the character sprite flip after a wall jump?")]
        [SerializeField] private bool doFlipOnWallJump;
        [Tooltip("The speed we slide down a wall")]
        [SerializeField] private float slideSpeed;
        [Tooltip("How fast we reach the Slide Speed")]
        [SerializeField] private float slideAccel;

        [Header("Falling")]
        [Tooltip("How fast we fall")]
        [SerializeField] private float maxFallSpeed;
        [Tooltip("The gravity multiplier when falling")]
        [SerializeField] private float fallGravityMult;

        [Header("Checks")]
        [Tooltip("The GameObject used to reference where we check for the ground")]
        [SerializeField] private Transform groundCheckPoint;
        [Tooltip("The size of the box we use to check the ground, use Gizmos to see a visual representation")]
        [SerializeField] private Vector2 groundCheckSize = new(0.49f, 0.03f);
        [Tooltip("The layer(s) we can run, jump and wall jump on")]
        [SerializeField] private LayerMask groundLayer;
        [Tooltip("The GameObject used to reference where we check the wall in the direction our character is facing")]
        [SerializeField] private Transform frontWallCheckPoint;
        [Tooltip("The GameObject used to reference where we check the wall in the opposite direction our character is facing")]
        [SerializeField] private Transform backWallCheckPoint;
        [Tooltip("The size of the box we use to check the wall, use Gizmos to see a visual representation")]
        [SerializeField] private Vector2 wallCheckSize = new(0.5f, 1f);

        [Header("Assists")]
        [Tooltip("The amount of time given to the player to jump after they have already fallen off a platform")]
        [SerializeField][Range(0.01f, 0.5f)] private float coyoteTime;
        [Tooltip("The amount of time given to jump if the player has pressed the jump button but the conditions haven't been met yet")]
        [SerializeField][Range(0.01f, 0.5f)] private float jumpInputBufferTime;

        // ======= Private Variables =======
        private float lastOnWallTimer;
        private float lastOnGroundTimer;
        private float lastOnLeftWallTimer;
        private float lastOnRightWallTimer;
        private float lastPressedJumpTimer;

        private float gravityStrength;
        private float gravityScale;

        private float runAccelAmount;
        private float runDeccelAmount;

        private float horizontalInput;
        private bool jumpInput;

        private bool wasWallSliding;
        private float wallJumpStartTime;
        private int lastWallJumpDir;
        private bool isFacingRight;
        private bool isJumpFalling;
        private bool isOnRightWall;
        private bool isOnLeftWall;
        private bool isJumpCut;
        private float jumpForce;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            SetGravityScale(gravityScale);
            wasWallSliding = false;
            isFacingRight = true;
        }

        #region Update Functions

        public void PlayerMovementUpdate(float delta)
        {
            Timers(delta);
            UpdatePlayerFlags();
            GetPlayerInputs();

            if (jumpInput)
            {
                lastPressedJumpTimer = jumpInputBufferTime;

                if (CanJump())
                    HandleJump();
                else if (CanWallJump())
                    HandleWallJump();
            }
                
            if (horizontalInput != 0 && 
                (horizontalInput > 0) != isFacingRight)
                HandleFlip();

            HandleGravity();
        }

        public void PlayerMovementFixedUpdate(float delta)
        {
            HandleRunning();

            if (doFlipOnWallJump)
            {
                // This flips our character the moment we hit the ground if we're wall sliding
                // It's dependent on how Alex creates his animations, we may or may not need this
                bool isWallSliding = CanWallSlide();
                if (isWallSliding)
                    HandleSliding(delta);
                else if (player.isGrounded && wasWallSliding)
                    HandleFlip();
                wasWallSliding = isWallSliding;
            }
            else
            {
                if (CanWallSlide())
                    HandleSliding(delta);
            }
        }

        private void Timers(float delta)
        {
            lastOnWallTimer -= delta;
            lastOnGroundTimer -= delta;
            lastOnLeftWallTimer -= delta;
            lastOnRightWallTimer -= delta;
            lastPressedJumpTimer -= delta;
        }

        private void UpdatePlayerFlags()
        {
            player.isRunning = IsRunning();
            player.isGrounded = IsGrounded();
            player.isOnWall = IsOnWall();
            isOnLeftWall = IsOnLeftWall();
            isOnRightWall = IsOnRightWall();

            // When we reach the apex of our jump
            if (player.isJumping && rb.velocity.y < 0)
            {
                player.isJumping = false;

                if (!player.isWallJumping)
                    isJumpFalling = true;
            }

            // Checks how long our movement has been slowed for
            if (player.isWallJumping && Time.time - wallJumpStartTime > wallJumpCooldown)
            {
                player.isWallJumping = false;
            }

            // Grounded and not jumping
            if (player.isGrounded && !player.isJumping && !player.isWallJumping)
            {
                isJumpCut = false;
                isJumpFalling = false;
            }
        }

        private void GetPlayerInputs()
        {
            horizontalInput = PlayerInputsManager.Instance.movementInput.x;
            jumpInput = PlayerInputsManager.Instance.jumpInput;
        }

        #endregion

        private void HandleRunning()
        {
            float targetSpeed = horizontalInput * maxRunSpeed;

            // We can reduce our control using Lerp() this smooths changes to our direction and speed
            float lerpAmount = player.isWallJumping ? wallJumpRunLerp : 1;
            targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, lerpAmount);

            // If we are moving then set an acceleration rate
            // If we're also in the air then adjust this to acceleration values in air
            float accelRate;
            if (player.isGrounded)
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
            else
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? (runAccelAmount * accelInAir) : (runDeccelAmount * deccelInAir);

            // Increase our acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
            if ((player.isJumping || player.isWallJumping || isJumpFalling) && Mathf.Abs(rb.velocity.y) < jumpHangTimeThreshold)
            {
                accelRate *= jumpHangAccelerationMult;
                targetSpeed *= jumpHangMaxSpeedMult;
            }

            // We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
            if (doConserveMomentum && 
                Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && 
                Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && 
                Mathf.Abs(targetSpeed) > 0.01f && 
                !player.isGrounded)
            {
                // Prevent any deceleration from happening, or in other words conserve are current momentum
                // You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
                accelRate = 0;
            }

            // Calculate difference between current velocity and desired velocity
            float speedDif = targetSpeed - rb.velocity.x;

            // Calculate force along x-axis to apply to the player
            float movement = speedDif * accelRate;

            // Convert this to a vector and apply to rigidbody
            rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }

        private void HandleJump()
        {
            player.isJumping = true;
            player.isWallJumping = false;
            isJumpFalling = false;
            isJumpCut = false;

            // Ensures we can't call HandleJump multiple times from one press
            lastPressedJumpTimer = 0;
            lastOnGroundTimer = 0;

            // We increase the force applied if we are falling
            // This means we'll always feel like we jump the same amount 
            float force = jumpForce;
            if (rb.velocity.y < 0)
                force -= rb.velocity.y;

            player.playerAnimatorManager.PlayTargetAnimation("Jump");

            // Unlike in the run we want to use the Impulse mode.
            // The default mode will apply are force instantly ignoring masss
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }

        private void HandleWallJump()
        {
            player.isJumping = false;
            player.isWallJumping = true;
            isJumpFalling = false;
            isJumpCut = false;

            wallJumpStartTime = Time.time;
            lastWallJumpDir = isOnRightWall ? -1 : 1;

            Vector2 force = new(wallJumpForce.x, wallJumpForce.y);
            force.x *= lastWallJumpDir; // Apply a force in the opposite direction of the wall

            if (Mathf.Sign(rb.velocity.x) != Mathf.Sign(force.x))
                force.x -= rb.velocity.x;

            // Checks whether the player is falling, if so we subtract the velocity.y (counteracting the force of gravity).
            // This ensures the player always reaches our desired jump force or greater
            if (rb.velocity.y < 0) 
                force.y -= rb.velocity.y;

            if (doFlipOnWallJump)
                HandleFlip();

            player.playerAnimatorManager.PlayTargetAnimation("Jump");

            rb.AddForce(force, ForceMode2D.Impulse);
        }

        private void HandleSliding(float delta)
        {
            // Works the same as the Run but only in the y-axis
            float speedDif = -slideSpeed - rb.velocity.y;
            float movement = speedDif * slideAccel;

            // We clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
            // The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called.
            movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / delta), Mathf.Abs(speedDif) * (1 / delta));

            rb.AddForce(movement * Vector2.up);
        }

        public void HandleJumpCut()
        {
            if (CanJumpCut())
                isJumpCut = true;
        }

        private void HandleFlip()
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            isFacingRight = !isFacingRight;
        }

        private void HandleGravity()
        {
            if (player.isOnWall)
            {
                SetGravityScale(0);
            }
            else if (isJumpCut)
            {
                // Higher gravity if jump button released
                SetGravityScale(gravityScale * jumpCutGravityMult);

                // Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
            }
            else if ((player.isJumping || player.isWallJumping || isJumpFalling) && Mathf.Abs(rb.velocity.y) < jumpHangTimeThreshold)
            {
                SetGravityScale(gravityScale * jumpHangGravityMult);
            }
            else if (rb.velocity.y < 0)
            {
                //Higher gravity if falling
                SetGravityScale(gravityScale * fallGravityMult);

                // Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
            }
            else
            {
                // Default gravity if standing on a platform or moving upwards
                SetGravityScale(gravityScale);
            }
        }

        private void SetGravityScale(float gravScale)
        {
            rb.gravityScale = gravScale;
        }

        #region Check Functions

        private bool IsRunning()
        {
            return player.isGrounded && Mathf.Abs(horizontalInput) > 0.01f;
        }

        private bool IsGrounded()
        {
            if (!player.isJumping && Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer))
                lastOnGroundTimer = coyoteTime;

            return lastOnGroundTimer > 0;
        }

        private bool IsOnWall()
        {
            lastOnWallTimer = Mathf.Max(lastOnLeftWallTimer, lastOnRightWallTimer);
            return 
                lastOnWallTimer > 0 && 
                ((isOnLeftWall && horizontalInput <= 0) || 
                (isOnRightWall && horizontalInput >= 0));
        }

        private bool IsOnLeftWall()
        {
            if (!player.isJumping && !player.isWallJumping &&
                ((Physics2D.OverlapBox(frontWallCheckPoint.position, wallCheckSize, 0, groundLayer) && !isFacingRight) || 
                (Physics2D.OverlapBox(backWallCheckPoint.position, wallCheckSize, 0, groundLayer) && isFacingRight)))
            {
                lastOnLeftWallTimer = coyoteTime;
            }

            return lastOnLeftWallTimer > 0;
        }

        private bool IsOnRightWall()
        {
            if (!player.isJumping && !player.isWallJumping &&
                ((Physics2D.OverlapBox(frontWallCheckPoint.position, wallCheckSize, 0, groundLayer) && isFacingRight) ||
                (Physics2D.OverlapBox(backWallCheckPoint.position, wallCheckSize, 0, groundLayer) && !isFacingRight)))
            {
                lastOnRightWallTimer = coyoteTime;
            }

            return lastOnRightWallTimer > 0;
        }

        private bool CanJump()
        {
            return
                !player.isJumping &&
                !player.isWallJumping &&
                player.isGrounded &&
                lastPressedJumpTimer > 0;
        }

        private bool CanWallJump()
        {
            return
                player.isOnWall &&
                !player.isJumping &&
                !player.isGrounded &&
                lastPressedJumpTimer > 0 &&
                (!player.isWallJumping ||
                (isOnLeftWall && lastWallJumpDir == -1) ||
                (isOnRightWall && lastWallJumpDir == 1));
        }

        private bool CanJumpCut()
        {
            return (player.isJumping || player.isWallJumping) && rb.velocity.y > 0;
        }

        private bool CanWallSlide()
        {
            return
                player.isOnWall &&
                !player.isJumping &&
                !player.isWallJumping &&
                !player.isGrounded;
        }

        #endregion

        #region Editor Functions

        private void OnValidate()
        {
            // Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
            gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);
            gravityScale = gravityStrength / Physics2D.gravity.y;

            // Calculate our run acceleration & deceleration forces using the formula:
            // amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
            runAccelAmount = ((1 / Time.fixedDeltaTime) * runAcceleration) / maxRunSpeed;
            runDeccelAmount = ((1 / Time.fixedDeltaTime) * runDecceleration) / maxRunSpeed;

            // Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
            jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;

            runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, maxRunSpeed);
            runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, maxRunSpeed);
            slideAccel = Mathf.Clamp(slideAccel, 0.01f, slideSpeed);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
            Gizmos.DrawWireCube(frontWallCheckPoint.position, wallCheckSize);
            Gizmos.DrawWireCube(backWallCheckPoint.position, wallCheckSize);

            if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
            }

            if (Physics2D.OverlapBox(frontWallCheckPoint.position, wallCheckSize, 0, groundLayer))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(frontWallCheckPoint.position, wallCheckSize);
            }

            if (Physics2D.OverlapBox(backWallCheckPoint.position, wallCheckSize, 0, groundLayer))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(backWallCheckPoint.position, wallCheckSize);
            }
        }

        #endregion
    }
}