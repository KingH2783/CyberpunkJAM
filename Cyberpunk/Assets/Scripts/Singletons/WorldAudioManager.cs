using UnityEngine;
using UnityEngine.Audio;

namespace HL
{
    public class WorldAudioManager : MonoBehaviour
    {
        [HideInInspector] public static WorldAudioManager Instance { get; private set; }
        [SerializeField] private AudioMixer mixer; 
        [Range(0.001f, 1)] public float startAllSlidersAtThisVolume;

        private float currentMasterVolume;
        private float currentMusicVolume;
        private float currentSoundFXVolume;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            SetMasterVolume(startAllSlidersAtThisVolume);
            SetMusicVolume(startAllSlidersAtThisVolume);
            SetSoundFXVolume(startAllSlidersAtThisVolume);
        }

        public void SetMasterVolume(float sliderValue)
        {
            currentMasterVolume = Mathf.Log10(sliderValue) * 20;
            mixer.SetFloat("MasterVolume", currentMasterVolume);
        }

        public void SetMusicVolume(float sliderValue)
        {
            currentMusicVolume = Mathf.Log10(sliderValue) * 20;
            mixer.SetFloat("MusicVolume", currentMusicVolume);
        }

        public void SetSoundFXVolume(float sliderValue)
        {
            currentSoundFXVolume = Mathf.Log10(sliderValue) * 20;
            mixer.SetFloat("SoundFXVolume", currentSoundFXVolume);
        }
    }
}