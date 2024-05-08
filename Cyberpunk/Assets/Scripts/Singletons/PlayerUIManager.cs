using UnityEngine;

namespace HL
{
    public class PlayerUIManager : MonoBehaviour
    {
        [HideInInspector] public static PlayerUIManager Instance { get; private set; }
        [HideInInspector] public EquippedWeaponsUI equippedWeaponsUI;
        [HideInInspector] public PlayerHealthBar healthBar;
        [HideInInspector] public PauseMenu pauseMenu;
        [HideInInspector] public AmmoUI ammoUI;

        [SerializeField] private GameObject HUD;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            equippedWeaponsUI = GetComponentInChildren<EquippedWeaponsUI>(true);
            healthBar = GetComponentInChildren<PlayerHealthBar>(true);
            pauseMenu = GetComponentInChildren<PauseMenu>(true);
            ammoUI = GetComponentInChildren<AmmoUI>(true);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void EnableHUD()
        {
            HUD.SetActive(true);
        }

        public void DisableHUD()
        {
            HUD.SetActive(false);
        }
    }
}