using UnityEngine;

namespace HL
{
    public class PlayerUIManager : MonoBehaviour
    {
        [HideInInspector] public static PlayerUIManager Instance { get; private set; }
        [HideInInspector] public PauseMenu pauseMenu;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            pauseMenu = GetComponentInChildren<PauseMenu>(true);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}