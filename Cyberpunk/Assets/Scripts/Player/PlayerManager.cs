using UnityEngine;

namespace HL
{
    public class PlayerManager : MonoBehaviour
    {
        [HideInInspector] public Transform _transform { get; private set; }
        [HideInInspector] public PlayerMovement playerMovement { get; private set; }
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager { get; private set; }

        // ======= Flags =======
        [Header("Flags")]
        public bool isRunning;
        public bool isGrounded;
        public bool isJumping;
        public bool isWallJumping;
        public bool isDashing;
        public bool isOnWall = false;
        public bool isOnSlope;

        private void Awake()
        {
            _transform = transform;
            playerMovement = GetComponent<PlayerMovement>();
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