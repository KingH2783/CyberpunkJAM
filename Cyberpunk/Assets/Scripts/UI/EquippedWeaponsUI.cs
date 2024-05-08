using UnityEngine;
using UnityEngine.UI;

namespace HL
{
    public class EquippedWeaponsUI : MonoBehaviour
    {
        [SerializeField] private Image meleeWeaponIcon;
        [SerializeField] private Image rangedWeaponIcon;

        public void UpdateMeleeWeaponIcon(Sprite icon)
        {
            meleeWeaponIcon.sprite = icon;
            meleeWeaponIcon.enabled = true;
        }

        public void UpdateRangedWeaponIcon(Sprite icon)
        {
            rangedWeaponIcon.sprite = icon;
            rangedWeaponIcon.enabled = true;
        }
    }
}