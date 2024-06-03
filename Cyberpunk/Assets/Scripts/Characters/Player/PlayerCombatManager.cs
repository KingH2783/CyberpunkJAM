using UnityEngine;

namespace HL
{
    public class PlayerCombatManager : MonoBehaviour
    {
        PlayerManager player;
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private Transform bulletSpawnPointCrouching;

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
            if (fireRateTimer > 0)
                fireRateTimer -= delta;
            else
            {
                fireRateTimer = 0;
                player.isDoingRangedAttack = false;
            }
            reloadTimer = (reloadTimer > 0) ? reloadTimer - delta : 0;
        }

        public void HandleMeleeAttack()
        {
            if (player.isDead || !player.isGrounded || player.isPerformingAction || player.isDoingMeleeAttack || player.isDoingRangedAttack)
                return;

            // The collider gets enabled during the animation
            if (!player.isCrouching)
                player.playerAnimatorManager.PlayTargetAnimation("Melee_Attack_1", currentMelee.stopMovement);
            else
                player.playerAnimatorManager.PlayTargetAnimation("Melee_Attack_1_Crouching", true);
            player.isDoingMeleeAttack = true;
        }

        public void HandleRangedAttack()
        {
            if (player.isDead || player.isPerformingAction || player.isDoingMeleeAttack || player.isDoingRangedAttack)
                return;

            if (roundsLeftInClip == 0)
                Reload();

            else if (reloadTimer <= 0 && 
                fireRateTimer <= 0)
            {
                GameObject bulletGameObject;
                if (!player.isCrouching)
                    bulletGameObject = Instantiate(currentRanged.bulletType, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                else
                    bulletGameObject = Instantiate(currentRanged.bulletType, bulletSpawnPointCrouching.position, bulletSpawnPointCrouching.rotation);

                Bullet bullet = bulletGameObject.GetComponent<Bullet>();
                bullet.characterWhoFiredMe = player;
                bullet.weapon = currentRanged;

                player.isDoingRangedAttack = true;
                if (player.isCrouching)
                    player.playerAnimatorManager.PlayTargetAnimation("Ranged_Attack_1_Crouching", true);
                else if (!player.isRunning)
                    player.playerAnimatorManager.PlayTargetAnimation("Ranged_Attack_1_Stationary", currentRanged.stopMovement);
                else
                    player.playerAnimatorManager.PlayTargetAnimation("Ranged_Attack_1_Running", currentRanged.stopMovement);
                player.playerSoundFXManager.PlayPlasmaGunShoot();
                PlayerUIManager.Instance.ammoUI.UseOneAmmoUI();

                fireRateTimer = bullet.weapon.fireRate;
                roundsLeftInClip -= 1;
            }
        }

        public void Reload()
        {
            if (roundsLeftInClip == currentRanged.ammoCapacity)
                return;

            if (!player.isCrouching)
                player.playerAnimatorManager.PlayTargetAnimation("Reload", true);
            else
                player.playerAnimatorManager.PlayTargetAnimation("Reload_Crouching", true);

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