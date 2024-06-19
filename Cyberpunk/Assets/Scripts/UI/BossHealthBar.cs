using UnityEngine;
using UnityEngine.UI;

namespace HL
{
    public class BossHealthBar : MonoBehaviour
    {
        Slider slider;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
        }

        private void Start()
        {
            TurnOffBossHealthBar();
        }

        public void SetMaxStat(float maxStat)
        {
            slider.maxValue = Mathf.RoundToInt(maxStat);
            slider.value = Mathf.RoundToInt(maxStat);
        }

        public void SetCurrentStat(float currentStat)
        {
            slider.value = Mathf.RoundToInt(currentStat);
        }

        public void TurnOnBossHealthBar()
        {
            slider.gameObject.SetActive(true);
        }

        public void TurnOffBossHealthBar()
        {
            slider.gameObject.SetActive(false);
        }
    }
}