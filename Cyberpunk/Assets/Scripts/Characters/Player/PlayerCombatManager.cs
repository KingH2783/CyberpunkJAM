using UnityEngine;

namespace HL
{
    public class PlayerCombatManager : MonoBehaviour
    {
        PlayerManager player;
        [SerializeField] private Transform bulletSpawnPoint;

        private float fireRateTimer;
        private float reloadTimer;
        private float roundsLeftInClip;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            roundsLeftInClip = player.playerStatsManager.currentRangedWeapon.roundCapacity;
        }

        public void Timers(float delta)
        {
            fireRateTimer = (fireRateTimer > 0) ? fireRateTimer - delta : 0;
            reloadTimer = (reloadTimer > 0) ? reloadTimer - delta : 0;
        }

        public void HandleMeleeAttack()
        {
            // The collider gets enabled during the animation
            player.playerAnimatorManager.PlayTargetAnimation("Attack1");
        }

        public void HandleRangedAttack()
        {
            if (roundsLeftInClip == 0)
                Reload();

            else if (reloadTimer <= 0 && 
                fireRateTimer <= 0)
            {
                GameObject bulletGameObject = Instantiate(player.playerStatsManager.currentRangedWeapon.bulletType, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                Bullet bullet = bulletGameObject.GetComponent<Bullet>();
                bullet.characterWhoFiredMe = player;
                bullet.weapon = player.playerStatsManager.currentRangedWeapon;

                //player.playerAnimatorManager.PlayTargetAnimation("Shoot");

                fireRateTimer = bullet.weapon.fireRate;
                roundsLeftInClip -= 1;
            }
        }

        public void Reload()
        {
            if (roundsLeftInClip == player.playerStatsManager.currentRangedWeapon.roundCapacity)
                return;

            //player.playerAnimatorManager.PlayTargetAnimation("Reload");

            reloadTimer = player.playerStatsManager.currentRangedWeapon.reloadTime;
            roundsLeftInClip = player.playerStatsManager.currentRangedWeapon.roundCapacity;
        }

        public void SwitchMeleeWeapon()
        {

        }

        public void SwitchRangedWeapon()
        {

        }
    }
}