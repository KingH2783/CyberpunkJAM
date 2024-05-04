using UnityEngine;

namespace HL
{
    [CreateAssetMenu(menuName = "Weapon/Ranged Weapon")]
    public class RangedWeapon : Weapon
    {
        public float fireRate;
        public float reloadTime;
        public int roundCapacity;
        public GameObject bulletType;
    }
}