using UnityEngine;

namespace HL
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerMovement playerMovement { get; private set; }
        [HideInInspector] public PlayerStatsManager playerStatsManager { get; private set; }
        [HideInInspector] public PlayerCombatManager playerCombatManager { get; private set; }
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager { get; private set; }

        // ======= Flags =======
        [Header("Player Flags")]
        public bool isRunning;
        public bool isGrounded;
        public bool isJumping;
        public bool isWallJumping;
        public bool isDashing;
        public bool isOnWall = false;
        public bool isOnSlope;

        protected override void Awake()
        {
            base.Awake();
            playerMovement = GetComponent<PlayerMovement>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            PlayerInputsManager.Instance.player = this;
            PlayerCamera.Instance.player = this;
        }

        private void Update()
        {
            float delta = Time.deltaTime;
            playerMovement.PlayerMovementUpdate(delta);
            playerAnimatorManager.SetAnimatorParams();
            playerCombatManager.Timers(delta);
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            playerMovement.PlayerMovementFixedUpdate(delta);
        }

        private void LateUpdate()
        {
            PlayerCamera.Instance.HandleAllCameraMovement();
        }
    }
}