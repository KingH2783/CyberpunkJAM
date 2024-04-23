using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace HL
{
    public class PlayerInputsManager : MonoBehaviour
    {
        // ======= Script Caches =======
        [HideInInspector] public static PlayerInputsManager Instance { get; private set; }
        [HideInInspector] public PlayerManager player;
        PlayerControls playerControls;

        // ======= Player Movement Inputs =======
        Action<InputAction.CallbackContext> movementInputHandler;
        [HideInInspector] public Vector2 movementInput { get; private set; }
        [HideInInspector] public bool jumpInput { get; private set; }

        // ======= Player Action Inputs =======
        Action<InputAction.CallbackContext> jumpInputHandlerPerformed;

        // ======= Game Managers =======
        [HideInInspector] public Scene currentScene;
        [SerializeField] private ActionMaps currentActionMap;

        #region Initialization and Deactivation

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            
            currentScene = SceneManager.GetActiveScene();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            Initialization();
            SceneManager.activeSceneChanged += OnSceneChange;
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                // Setup for Lambda events
                movementInputHandler = context => movementInput = context.ReadValue<Vector2>();
                jumpInputHandlerPerformed = context => jumpInput = true;
            }

            playerControls.Disable();
            currentActionMap = ActionMaps.None;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            currentScene = newScene;
            Initialization();
        }

        private void Initialization()
        {
            // Menu Scene
            if (currentScene.buildIndex == 0)
            {
                Instance.enabled = true;
                SwitchActionMap(ActionMaps.Menu);
            }
            else // Game Scene
            {
                Instance.enabled = true;
                SwitchActionMap(ActionMaps.Player);
            }
        }

        private void OnDisable()
        {
            if (playerControls != null)
            {
                UnsubscribeFromAllActionMaps();
                playerControls.Disable();
            }
        }

        private void OnDestroy()
        {
            // If we destroy this object, unsubscribe from this event
            SceneManager.activeSceneChanged -= OnSceneChange;
            if (playerControls != null)
                UnsubscribeFromAllActionMaps();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    SwitchActionMap(currentActionMap);
                }
                else
                {
                    UnsubscribeFromAllActionMaps();
                    playerControls.Disable();
                }
            }
        }

        #endregion

        private void HandleJumpInputCanceled(InputAction.CallbackContext context)
        {
            jumpInput = false;
            player.playerMovement.HandleJumpCut();
        }

        private void HandleEscapeInput(InputAction.CallbackContext context)
        {
            PauseMenu pauseMenu = PlayerUIManager.Instance.pauseMenu;

            // Menu Scene
            if (currentScene.buildIndex == 0)
            {

            }
            else // Game Scene
            {
                if (pauseMenu.IsGamePaused)
                {
                    if (pauseMenu.AreSettingsOpen)
                        pauseMenu.CloseSettings();
                    else
                        pauseMenu.ResumeGame();
                }
                else
                    pauseMenu.PauseGame();
            }
        }

        #region Handle Subscribing To Action Maps

        public void SwitchActionMap(ActionMaps actionMap)
        {
            UnsubscribeFromAllActionMaps();
            playerControls.Disable();

            if (actionMap == ActionMaps.Menu)
            {
                SubscribeToMenuActionMap();
                playerControls.Menu.Enable();
                currentActionMap = ActionMaps.Menu;
            }
            else if (actionMap == ActionMaps.Player)
            {
                SubscribeToPlayerActionMap();
                playerControls.Player.Enable();
                currentActionMap = ActionMaps.Player;
            }
        }

        private void UnsubscribeFromAllActionMaps()
        {
            UnsubscribeFromMenuActionMap();
            UnsubscribeFromPlayerActionMap();
        }

        private void SubscribeToMenuActionMap()
        {
            playerControls.Menu.Escape.performed += HandleEscapeInput;
        }

        private void UnsubscribeFromMenuActionMap()
        {
            playerControls.Menu.Escape.performed -= HandleEscapeInput;
        }

        private void SubscribeToPlayerActionMap()
        {
            playerControls.Player.Movement.performed += movementInputHandler;
            playerControls.Player.Jump.performed += jumpInputHandlerPerformed;
            playerControls.Player.Jump.canceled += HandleJumpInputCanceled;
            playerControls.Player.Escape.performed += HandleEscapeInput;
        }

        private void UnsubscribeFromPlayerActionMap()
        {
            playerControls.Player.Movement.performed -= movementInputHandler;
            playerControls.Player.Jump.performed -= jumpInputHandlerPerformed;
            playerControls.Player.Jump.canceled -= HandleJumpInputCanceled;
            playerControls.Player.Escape.performed -= HandleEscapeInput;
        }
        
        #endregion
    }
}