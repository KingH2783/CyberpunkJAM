using UnityEngine;
using UnityEngine.UI;

namespace HL
{
    public class PlayerHealthBar : MonoBehaviour
    {
        Slider slider;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
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
    }
}