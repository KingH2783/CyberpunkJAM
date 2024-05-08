using UnityEngine;

namespace HL
{
    [CreateAssetMenu(menuName = "Items/Weapons/Ranged Weapon")]
    public class RangedWeapon : Weapon
    {
        public float fireRate;
        public float reloadTime;
        public int ammoCapacity;
        public GameObject bulletType;
    }
}