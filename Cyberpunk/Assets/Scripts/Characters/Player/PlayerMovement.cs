using System.Collections;
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
        [SerializeField] private bool showDashSettings;
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

        [Header("Slopes")]
        [SerializeField] private float maxSlopeRunSpeed;
        [SerializeField] private float maxSlopeAngle;
        [SerializeField] private float slopeStickForce;

        [Header("Jumping")]
        [Tooltip("The height of the jump")]
        [SerializeField] private float jumpHeight;
        [SerializeField] private bool allowDoubleJump = true;
        [Tooltip("The time it takes to reach the highest point of the jump (the apex)")]
        [SerializeField] private float jumpTimeToApex;
        [Tooltip("The gravity multiplier when we let go of the jump button before the apex")]
        [SerializeField] private float jumpCutGravityMult;
        [SerializeField] private float timeToHoldJumpForFullJump;
        [Tooltip("The gravity multiplier when we are at the apex")]
        [SerializeField][Range(0f, 1)] private float jumpHangGravityMult;
        [Tooltip("How long we stay at the apex")]
        [SerializeField] private float jumpHangTimeThreshold;
        [Tooltip("How fast we speed up at the apex")]
        [SerializeField] private float jumpHangAccelerationMult;
        [Tooltip("The max speed we can go at the apex")]
        [SerializeField] private float jumpHangMaxSpeedMult;

        [Header("Wall Jumping")]
        [Tooltip("Should we allow the player to wall jump?")]
        [SerializeField] private bool allowWallJump;
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

        [Header("Dashing")]
        [SerializeField] private bool allowDash;
        [SerializeField] private float dashVelocity;
        [SerializeField] private float dashTime;
        [SerializeField] private float dashCooldown;
        [SerializeField] private bool allowMultipleDashesBeforeTouchingGround;

        [Header("Falling")]
        [Tooltip("How fast we fall")]
        [SerializeField] private float maxFallSpeed;
        [Tooltip("The gravity multiplier when falling")]
        [SerializeField] private float fallGravityMult;
        [SerializeField] private float fallGravityMultOnSteepSlope;

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
        [SerializeField] private Vector2 slopeCheckStartOffset = new(0, 0.25f);
        [SerializeField] private float slopeCheckDistance = 0.5f;

        [Header("Assists")]
        [Tooltip("The amount of time given to the player to jump after they have already fallen off a platform")]
        [SerializeField][Range(0.01f, 0.5f)] private float coyoteTime;
        [Tooltip("The amount of time given to jump if the player has pressed the jump button but the conditions haven't been met yet")]
        [SerializeField][Range(0.01f, 0.5f)] private float jumpInputBufferTime;
        [SerializeField][Range(0.01f, 0.5f)] private float dashInputBufferTime;

        // ======= Private Variables =======
        private float lastOnWallTimer;
        private float lastOnGroundTimer;
        private float lastOnLeftWallTimer;
        private float lastOnRightWallTimer;
        private float lastPressedJumpTimer;
        private float lastPressedDashTimer;
        private float lastDashCooldownTimer;
        private float jumpInputStartTimer;

        private float gravityStrength;
        private float gravityScale;

        private float runAccelAmount;
        private float runDeccelAmount;

        private float horizontalInput;
        private bool jumpInput;

        private bool hasDoneDoubleJump;
        private bool hasDoneDashInAir;
        private bool wasWallSliding;
        private float wallJumpStartTime;
        private int lastWallJumpDir;
        private bool isFacingRight;
        private bool isJumpFalling;
        private bool isOnRightWall;
        private bool isOnLeftWall;
        private bool isJumpCut;
        private bool isOnSteepSlope;
        private float jumpForce;
        private float slopeAngle;
        private Vector2 slopeNormal;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GetComponent<PlayerManager>();

            InitializingVariables();
        }

        private void Start()
        {
            SetGravityScale(gravityScale);

            wasWallSliding = false;
            isFacingRight = true;
            isOnRightWall = false;
            isOnLeftWall = false;
        }

        #region Update Functions

        public void PlayerMovementUpdate(float delta)
        {
            Timers(delta);
            UpdatePlayerFlags();
            GetPlayerInputs();
            
            if (jumpInput)
            {
                jumpInputStartTimer += delta;
                lastPressedJumpTimer = jumpInputBufferTime;

                if (CanJump())
                    HandleJump();
                
                else if (CanWallJump())
                    HandleWallJump();

                else if (CanDoubleJump())
                    HandleDoubleJump();
            }

            if (PlayerInputsManager.Instance.dashInput)
            {
                PlayerInputsManager.Instance.dashInput = false;
                lastPressedDashTimer = dashInputBufferTime;
                if (CanDash())
                    HandleDash();
            }
                
            if (horizontalInput != 0 && 
                (horizontalInput > 0) != isFacingRight)
                HandleFlip();

            HandleGravity();
        }

        public void PlayerMovementFixedUpdate(float delta)
        {
            player.isOnSlope = IsOnSlope();
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
            lastOnWallTimer = (lastOnWallTimer > 0) ? lastOnWallTimer - delta : 0;
            lastOnGroundTimer = (lastOnGroundTimer > 0) ? lastOnGroundTimer - delta : 0;
            lastOnLeftWallTimer = (lastOnLeftWallTimer > 0) ? lastOnLeftWallTimer - delta : 0;
            lastOnRightWallTimer = (lastOnRightWallTimer > 0) ? lastOnRightWallTimer - delta : 0;
            lastPressedJumpTimer = (lastPressedJumpTimer > 0) ? lastPressedJumpTimer - delta : 0;
            lastPressedDashTimer = (lastPressedDashTimer > 0) ? lastPressedDashTimer - delta : 0;
            lastDashCooldownTimer = (lastDashCooldownTimer > 0) ? lastDashCooldownTimer - delta : 0;
        }

        private void UpdatePlayerFlags()
        {
            player.isRunning = IsRunning();
            player.isGrounded = IsGrounded();
            if (allowWallJump)
            {
                player.isOnWall = IsOnWall();
                isOnLeftWall = IsOnLeftWall();
                isOnRightWall = IsOnRightWall();
            }

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
                hasDoneDoubleJump = false;
                hasDoneDashInAir = false;
                jumpInputStartTimer = 0;
            }

            if (player.isOnWall && !player.isJumping && !player.isWallJumping)
            {
                // TO DO: Solve a bug here where moving off the wall makes it difficult for the player to do a wall jump
                hasDoneDoubleJump = false;
                hasDoneDashInAir = false;
                jumpInputStartTimer = 0;
            }
        }

        private void GetPlayerInputs()
        {
            horizontalInput = PlayerInputsManager.Instance.movementInput.x;
            jumpInput = PlayerInputsManager.Instance.jumpInput;
            if (jumpInputStartTimer > timeToHoldJumpForFullJump)
                PlayerInputsManager.Instance.jumpInput = false;
        }

        #endregion

        private void HandleRunning()
        {
            if (player.isDashing || isOnSteepSlope)
                return;

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

            if (player.isOnSlope)
            {
                // Calculate the movement along the slope direction
                Vector2 slopeMovement = new Vector2(slopeNormal.y, -slopeNormal.x) * movement;
                rb.AddForce(slopeMovement, ForceMode2D.Force);
            }
            else
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
            /*float force = jumpForce;
            if (rb.velocity.y < 0)
                force -= rb.velocity.y;*/

            // Reset vertical velocity before applying jump force (FIX FOR SLOPES)
            Vector2 currentVelocity = rb.velocity;
            currentVelocity.y = 0;
            rb.velocity = currentVelocity;

            // We increase the force applied if we are falling
            // This means we'll always feel like we jump the same amount 
            float force = jumpForce;

            // Unlike in the run we want to use the Impulse mode.
            // The default mode will apply are force instantly ignoring masss
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);

            player.playerAnimatorManager.PlayTargetAnimation("Jump");
        }

        private void HandleDoubleJump()
        {
            jumpInputStartTimer = 0;
            hasDoneDoubleJump = true;
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

        private void HandleDash()
        {
            if (!player.isGrounded)
                hasDoneDashInAir = true;
            lastDashCooldownTimer = dashCooldown;
            player.isDashing = true;
            player.isInvulnerable = true;

            Vector2 dashDirection = new(player._transform.localScale.x, 0);
            rb.velocity = dashDirection.normalized * dashVelocity;

            StartCoroutine(StopDashing());
        }

        private IEnumerator StopDashing()
        {
            yield return new WaitForSeconds(dashTime);
            player.isDashing = false;
            player.isInvulnerable = false;
            rb.velocity = Vector2.zero;
        }

        private void HandleFlip()
        {
            isFacingRight = !isFacingRight;
            player._transform.Rotate(0, 180, 0);

            /*Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;*/
        }

        private void HandleGravity()
        {
            if (player.isOnWall || player.isDashing || player.isOnSlope)
            {
                SetGravityScale(0);
            }
            else if (isOnSteepSlope)
            {
                // Higher gravity if on a steep slope
                SetGravityScale(gravityScale * fallGravityMultOnSteepSlope);

                // Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
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

            return lastOnGroundTimer > 0 && !isOnSteepSlope;
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

        private bool IsOnSlope()
        {
            Vector2 checkPos = player._transform.position + new Vector3(slopeCheckStartOffset.x, slopeCheckStartOffset.y);
            RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, groundLayer);
            if (hit)
            {
                slopeNormal = hit.normal;
                slopeAngle = Vector2.Angle(slopeNormal, Vector2.up);

                if (Physics2D.OverlapBox(groundCheckPoint.position, new(groundCheckSize.x + 0.5f, groundCheckSize.y), 0, groundLayer))
                    isOnSteepSlope = slopeAngle > maxSlopeAngle;
            }
            return
                slopeAngle <= maxSlopeAngle && 
                (slopeAngle <= -0.01f || slopeAngle >= 0.01f) && 
                player.isGrounded;
        }

        private bool CanJump()
        {
            return
                !player.isJumping &&
                !player.isWallJumping &&
                player.isGrounded &&
                lastPressedJumpTimer > 0;
        }

        private bool CanDoubleJump()
        {
            return
                allowDoubleJump &&
                !hasDoneDoubleJump &&
                !player.isJumping &&
                !player.isWallJumping &&
                !player.isGrounded &&
                lastPressedJumpTimer > 0;
        }

        private bool CanWallJump()
        {
            return
                allowWallJump &&
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
            return 
                (player.isJumping || player.isWallJumping) && 
                rb.velocity.y > 0 &&
                jumpInputStartTimer < timeToHoldJumpForFullJump;
        }

        private bool CanDash()
        {
            // Is grounded OR it behaves the same in the air
            if (player.isGrounded || allowMultipleDashesBeforeTouchingGround)
            {
                return
                    allowDash &&
                    !player.isDashing && 
                    lastDashCooldownTimer <= 0 &&
                    lastPressedDashTimer > 0;
            }
            else // Is not grounded
            {
                return
                    allowDash &&
                    !player.isDashing && 
                    !hasDoneDashInAir &&
                    lastDashCooldownTimer <= 0 &&
                    lastPressedDashTimer > 0;
            }
        }

        private bool CanWallSlide()
        {
            return
                allowWallJump &&
                player.isOnWall &&
                !player.isJumping &&
                !player.isWallJumping &&
                !player.isGrounded;
        }

        #endregion

        #region Editor Functions

        private void InitializingVariables()
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

        private void OnValidate()
        {
            InitializingVariables();
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

            Vector2 checkPos = transform.position + new Vector3(slopeCheckStartOffset.x, slopeCheckStartOffset.y);
            RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, groundLayer);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(hit.point, hit.normal);
        }

        #endregion
    }
}