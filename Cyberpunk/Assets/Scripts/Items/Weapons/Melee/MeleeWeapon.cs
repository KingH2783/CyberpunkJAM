using UnityEngine;

namespace HL
{
    [CreateAssetMenu(menuName = "Items/Weapons/Melee Weapon")]
    public class MeleeWeapon : Weapon
    {
        public float cooldown;
    }
}