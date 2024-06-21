using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace HL
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenuParent;
        [SerializeField] private GameObject pauseMenuButtons;
        [SerializeField] private GameObject audioSettings;
        [HideInInspector] public bool isGamePaused;
        [HideInInspector] public bool areSettingsOpen;

        [Header("Volume Sliders")]
        public Slider masterVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider soundFXVolumeSlider;

        private void Start()
        {
            isGamePaused = false;
            areSettingsOpen = false;
            masterVolumeSlider.value = WorldAudioManager.Instance.startAllSlidersAtThisVolume;
            musicVolumeSlider.value = WorldAudioManager.Instance.startAllSlidersAtThisVolume;
            soundFXVolumeSlider.value = WorldAudioManager.Instance.startAllSlidersAtThisVolume;
        }

        public void PauseGame()
        {
            PlayerInputsManager.Instance.SwitchActionMap(ActionMaps.Menu);
            pauseMenuParent.SetActive(true);
            isGamePaused = true;
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            PlayerInputsManager.Instance.SwitchActionMap(PlayerInputsManager.Instance.lastActionMap);
            pauseMenuParent.SetActive(false);
            isGamePaused = false;
            Time.timeScale = 1;
        }

        public void OpenSettings()
        {
            pauseMenuButtons.SetActive(false);
            audioSettings.SetActive(true);
            areSettingsOpen = true;
        }

        public void CloseSettings()
        {
            pauseMenuButtons.SetActive(true);
            audioSettings.SetActive(false);
            areSettingsOpen = false;
        }

        public void QuitGame()
        {
            Application.Quit();

#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
        }

        public void SetMasterVolume(float sliderValue)
        {
            WorldAudioManager.Instance.SetMasterVolume(sliderValue);
        }

        public void SetMusicVolume(float sliderValue)
        {
            WorldAudioManager.Instance.SetMusicVolume(sliderValue);
        }

        public void SetSoundFXVolume(float sliderValue)
        {
            WorldAudioManager.Instance.SetSoundFXVolume(sliderValue);
        }
    }
}