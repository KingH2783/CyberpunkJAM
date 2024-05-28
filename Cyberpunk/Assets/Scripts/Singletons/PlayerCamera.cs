using UnityEngine;

namespace HL
{
    public class PlayerCamera : MonoBehaviour
    {
        [HideInInspector] public static PlayerCamera Instance { get; private set; }
        [HideInInspector] public PlayerManager player;

        // ======= Transform Variables =======
        [Header("Transform Variables")]
        private Camera mainCamera;
        private Transform cameraHolderTransform; // transform of holder (this gameObject)

        // ======= Camera Settings =======
        [Header("Camera Settings")]
        [SerializeField] private float playerFollowSpeed = 0.1f;
        [SerializeField] private float cameraSize = 8f;
        [SerializeField] private Vector2 cameraOffset = new(0, 4.5f);

        // ======= Private Variables =======
        private Vector3 cameraFollowVelocity = Vector3.zero;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            cameraHolderTransform = transform;
            mainCamera = GetComponentInChildren<Camera>();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void HandleAllCameraMovement()
        {
            FollowTarget();
            mainCamera.transform.localPosition = new(cameraOffset.x, cameraOffset.y, mainCamera.transform.localPosition.z);
            mainCamera.orthographicSize = cameraSize;
        }

        private void FollowTarget()
        {
            cameraHolderTransform.position = Vector3.SmoothDamp(cameraHolderTransform.position, player._transform.position, ref cameraFollowVelocity, playerFollowSpeed);
        }

        private void OnValidate()
        {
            mainCamera = GetComponentInChildren<Camera>();
            mainCamera.transform.localPosition = new(cameraOffset.x, cameraOffset.y, mainCamera.transform.localPosition.z);
            mainCamera.orthographicSize = cameraSize;
        }
    }
}