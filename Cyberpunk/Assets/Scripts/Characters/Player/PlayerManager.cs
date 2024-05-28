using UnityEngine;

namespace HL
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerLocomotion playerLocomotion { get; private set; }
        [HideInInspector] public PlayerStatsManager playerStatsManager { get; private set; }
        [HideInInspector] public PlayerCombatManager playerCombatManager { get; private set; }
        [HideInInspector] public PlayerSoundFXManager playerSoundFXManager { get; private set; }
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager { get; private set; }

        // ======= Flags =======
        [Header("Player Flags")]
        public bool isWallJumping;
        public bool isOnWall = false;

        protected override void Awake()
        {
            base.Awake();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerSoundFXManager = GetComponent<PlayerSoundFXManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        }

        private void Start()
        {
            //DontDestroyOnLoad(gameObject);

            PlayerInputsManager.Instance.player = this;
            PlayerCamera.Instance.player = this;
        }

        protected override void Update()
        {
            base.Update();
            playerAnimatorManager.SetAnimatorParams();
            playerCombatManager.Timers(deltaUpdate);
        }

        private void LateUpdate()
        {
            PlayerCamera.Instance.HandleAllCameraMovement();
        }
    }
}