using UnityEngine;

namespace HL
{
    public class PlayerCombatManager : MonoBehaviour
    {
        PlayerManager player;
        [SerializeField] private Transform bulletSpawnPoint;

        private float fireRateTimer;
        private float reloadTimer;
        private int roundsLeftInClip;

        private MeleeWeapon currentMelee;
        private RangedWeapon currentRanged;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            // Assign current weapons
            currentMelee = player.playerStatsManager.currentMeleeWeapon;
            currentRanged = player.playerStatsManager.currentRangedWeapon;

            // Update ammo capacity and UI
            roundsLeftInClip = currentRanged.ammoCapacity;
            PlayerUIManager.Instance.ammoUI.UpdateUIAmmoCapacity(currentRanged.ammoCapacity);
            StartCoroutine(PlayerUIManager.Instance.ammoUI.ReloadAllAmmoUI(0));
            PlayerUIManager.Instance.equippedWeaponsUI.UpdateMeleeWeaponIcon(currentMelee.itemIcon);
            PlayerUIManager.Instance.equippedWeaponsUI.UpdateRangedWeaponIcon(currentRanged.itemIcon);
        }

        public void Timers(float delta)
        {
            fireRateTimer = (fireRateTimer > 0) ? fireRateTimer - delta : 0;
            reloadTimer = (reloadTimer > 0) ? reloadTimer - delta : 0;
        }

        public void HandleMeleeAttack()
        {
            if (player.isDead || !player.isGrounded || player.isPerformingAction)
                return;

            // The collider gets enabled during the animation
            player.playerAnimatorManager.PlayTargetAnimation("Attack1");
            player.isPerformingAction = true;
        }

        public void HandleRangedAttack()
        {
            if (player.isDead || player.isPerformingAction)
                return;

            if (roundsLeftInClip == 0)
                Reload();

            else if (reloadTimer <= 0 && 
                fireRateTimer <= 0)
            {
                GameObject bulletGameObject = Instantiate(currentRanged.bulletType, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                Bullet bullet = bulletGameObject.GetComponent<Bullet>();
                bullet.characterWhoFiredMe = player;
                bullet.weapon = player.playerStatsManager.currentRangedWeapon;

                //player.playerAnimatorManager.PlayTargetAnimation("Shoot");
                PlayerUIManager.Instance.ammoUI.UseOneAmmoUI();

                fireRateTimer = bullet.weapon.fireRate;
                roundsLeftInClip -= 1;
            }
        }

        public void Reload()
        {
            if (roundsLeftInClip == currentRanged.ammoCapacity)
                return;

            //player.playerAnimatorManager.PlayTargetAnimation("Reload");

            reloadTimer = currentRanged.reloadTime;
            roundsLeftInClip = currentRanged.ammoCapacity;
            StartCoroutine(PlayerUIManager.Instance.ammoUI.ReloadAllAmmoUI(reloadTimer));
        }

        public void SwitchMeleeWeapon()
        {
            PlayerUIManager.Instance.equippedWeaponsUI.UpdateMeleeWeaponIcon(currentMelee.itemIcon);
        }

        public void SwitchRangedWeapon()
        {
            PlayerUIManager.Instance.equippedWeaponsUI.UpdateRangedWeaponIcon(currentRanged.itemIcon); 
            //PlayerUIManager.Instance.ammoUI.UpdateUIAmmoCapacity(currentRanged.ammoCapacity);
        }
    }
}