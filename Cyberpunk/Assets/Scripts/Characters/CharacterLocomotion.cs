using UnityEngine;

namespace HL
{
    public class CharacterLocomotion : MonoBehaviour
    {
        // ======= Script Caches =======
        protected Rigidbody2D rb;
        protected CharacterManager character;

        // ======= Settings =======
        [Header("Running")]
        [SerializeField] protected float maxRunSpeed;
        [SerializeField] protected float runAcceleration;
        [SerializeField] protected float runDecceleration;
        [SerializeField][Range(0f, 1)] protected float accelInAir;
        [SerializeField][Range(0f, 1)] protected float deccelInAir;
        [SerializeField] protected bool doConserveMomentum;

        [Header("Slopes")]
        [SerializeField] protected float maxSlopeRunSpeed;
        [SerializeField] protected float maxSlopeAngle;

        [Header("Jumping")]
        [SerializeField] protected float jumpHeight;
        [SerializeField] protected bool allowDoubleJump = true;
        [SerializeField] protected float jumpTimeToApex;
        [SerializeField][Range(0f, 1)] protected float jumpHangGravityMult;
        [SerializeField] protected float jumpHangTimeThreshold;
        [SerializeField] protected float jumpHangAccelerationMult;
        [SerializeField] protected float jumpHangMaxSpeedMult;

        [Header("Falling")]
        [SerializeField] protected float maxFallSpeed;
        [SerializeField] protected float fallGravityMult;
        [SerializeField] protected float fallGravityMultOnSteepSlope;

        [Header("Checks")]
        [SerializeField] protected LayerMask groundLayer;
        [SerializeField] protected Transform groundCheckPoint;
        [SerializeField] protected Vector2 groundCheckSize = new(0.75f, 0.03f);
        [SerializeField] protected Vector2 slopeCheckStartOffset = new(0, 0.25f);
        [SerializeField] protected float slopeCheckDistance = 2f;

        protected float runAccelAmount;
        protected float runDeccelAmount;
        protected float gravityStrength;
        protected float gravityScale;
        protected float jumpForce;
        protected float slopeAngle;
        protected Vector2 slopeNormal;

        protected bool isOnSteepSlope;
        protected bool isJumpFalling;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            character = GetComponent<CharacterManager>();
            InitializingVariables();
        }

        protected virtual void Start()
        {
            rb.gravityScale = gravityScale;
            character.isFacingRight = true;
        }

        public virtual void LocomotionUpdate(float delta)
        {
            
        }

        public virtual void LocomotionFixedUpdate(float delta)
        {
            character.isOnSlope = IsOnSlope();
        }

        protected virtual void HandleMovement(float targetDirection)
        {
            if (character.isDashing || isOnSteepSlope)
                return;

            float targetSpeed = targetDirection * maxRunSpeed;

            // Determine acceleration rate based on ground and air status
            float accelRate;
            if (character.isGrounded)
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
            else
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? (runAccelAmount * accelInAir) : (runDeccelAmount * deccelInAir);

            // Modify acceleration and maxSpeed during jump hang time
            if ((character.isJumping || isJumpFalling) && Mathf.Abs(rb.velocity.y) < jumpHangTimeThreshold)
            {
                accelRate *= jumpHangAccelerationMult;
                targetSpeed *= jumpHangMaxSpeedMult;
            }

            // Conserve momentum if conditions are met
            if (doConserveMomentum &&
                Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) &&
                Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) &&
                Mathf.Abs(targetSpeed) > 0.01f &&
                !character.isGrounded)
            {
                accelRate = 0;
            }

            // Calculate the speed difference and required movement force
            float speedDif = targetSpeed - rb.velocity.x;
            float movementForce = speedDif * rb.mass / Time.deltaTime;

            // Apply force considering platform movement
            Vector2 platformVelocity = Vector2.zero;
            if (character.isOnPlatform)
            {
                Rigidbody2D platformRB = transform.parent.GetComponent<Rigidbody2D>();
                platformVelocity = platformRB.velocity;
            }

            // Calculate the total force required, including platform velocity
            Vector2 totalForce = new Vector2(movementForce, 0) + platformVelocity * rb.mass / Time.deltaTime;

            // Apply the force to the player
            rb.AddForce(totalForce, ForceMode2D.Force);
        }

        public virtual void HandleFlip()
        {
            character.isFacingRight = !character.isFacingRight;
            character._transform.Rotate(0, 180, 0);

            /*Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;*/
        }

        protected virtual bool IsOnSlope()
        {
            Vector2 checkPos = character._transform.position + new Vector3(slopeCheckStartOffset.x, slopeCheckStartOffset.y);
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
                character.isGrounded;
        }

        protected virtual void InitializingVariables()
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
        }

        protected virtual void OnValidate()
        {
            InitializingVariables();
        }

        #region Editor Functions

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);

            if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
            }

            Vector2 checkPos = transform.position + new Vector3(slopeCheckStartOffset.x, slopeCheckStartOffset.y);
            RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, groundLayer);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(hit.point, hit.normal);
        }

        #endregion
    }
}