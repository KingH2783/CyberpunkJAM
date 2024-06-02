using UnityEngine;

namespace HL
{
    public class AILocomotion : CharacterLocomotion
    {
        AIManager ai;

        [HideInInspector] public float leftOrRightAIMovementInput;

        protected override void Awake()
        {
            base.Awake();
            ai = GetComponent<AIManager>();
        }

        protected override void Start()
        {
            base.Start();
        }

        public override void LocomotionUpdate(float delta)
        {
            base.LocomotionUpdate(delta);

            UpdateAIFlags();
            HandleAIGravity();
        }

        public override void LocomotionFixedUpdate(float delta)
        {
            base.LocomotionFixedUpdate(delta);

            if (ai.isPerformingAction)
            {
                StopAIMovement();
                return;
            }

            HandleMovement(leftOrRightAIMovementInput, delta);

            if (leftOrRightAIMovementInput != 0 &&
                (leftOrRightAIMovementInput > 0) != ai.isFacingRight)
                HandleFlip();
        }

        private void UpdateAIFlags()
        {
            ai.isRunning = IsRunning();
            ai.isGrounded = IsGrounded();

            // When we reach the apex of our jump
            if (ai.isJumping && rb.velocity.y < 0)
            {
                ai.isJumping = false;
                isJumpFalling = true;
            }

            // Grounded and not jumping
            if (ai.isGrounded && !ai.isJumping)
            {
                isJumpFalling = false;
            }
        }

        private void HandleAIGravity()
        {
            if (ai.isDashing || ai.isOnSlope)
            {
                rb.gravityScale = 0;
            }
            else if (isOnSteepSlope)
            {
                // Higher gravity if on a steep slope
                rb.gravityScale = gravityScale * fallGravityMultOnSteepSlope;

                // Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
            }
            else if ((ai.isJumping || isJumpFalling) && Mathf.Abs(rb.velocity.y) < jumpHangTimeThreshold)
            {
                rb.gravityScale = gravityScale * jumpHangGravityMult;
            }
            else if (rb.velocity.y < 0)
            {
                //Higher gravity if falling
                rb.gravityScale = gravityScale * fallGravityMult;

                // Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
            }
            else
            {
                // Default gravity if standing on a platform or moving upwards
                rb.gravityScale = gravityScale;
            }
        }

        #region State Functions

        public void StopAIMovement()
        {
            leftOrRightAIMovementInput = 0;
            rb.velocity = new(0, rb.velocity.y);
        }

        #endregion

        #region Checks

        private bool IsRunning()
        {
            return ai.isGrounded && Mathf.Abs(leftOrRightAIMovementInput) > 0.01f;
        }

        private bool IsGrounded()
        {
            return
                Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer) && 
                !ai.isJumping && 
                !isOnSteepSlope;
        }

        #endregion

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
        }
    }
}