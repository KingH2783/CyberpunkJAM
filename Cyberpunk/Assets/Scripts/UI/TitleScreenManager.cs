using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HL
{
    public class TitleScreenManager : MonoBehaviour
    {
        [SerializeField] private int worldSceneIndex = 1;

        [SerializeField] private GameObject titleScreenButtons;
        [SerializeField] private GameObject audioSettings;

        [Header("Volume Sliders")]
        public Slider masterVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider soundFXVolumeSlider;

        private void Start()
        {
            masterVolumeSlider.value = WorldAudioManager.Instance.startAllSlidersAtThisVolume;
            musicVolumeSlider.value = WorldAudioManager.Instance.startAllSlidersAtThisVolume;
            soundFXVolumeSlider.value = WorldAudioManager.Instance.startAllSlidersAtThisVolume;
        }

        // Called from button
        public void StartNewGame()
        {
            SceneManager.LoadScene(worldSceneIndex);
        }

        public void OpenSettings()
        {
            titleScreenButtons.SetActive(false);
            audioSettings.SetActive(true);
        }

        public void CloseSettings()
        {
            titleScreenButtons.SetActive(true);
            audioSettings.SetActive(false);
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