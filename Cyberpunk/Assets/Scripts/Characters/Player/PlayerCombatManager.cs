using UnityEngine;

namespace HL
{
    public class PlayerCombatManager : MonoBehaviour
    {
        PlayerManager player;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        public void HandleMeleeAttack()
        {
            // The collider gets enabled during the animation
            player.playerAnimatorManager.PlayTargetAnimation("Attack1");
        }

        public void HandleRangedAttack()
        {

        }

        public void SwitchMeleeWeapon()
        {

        }

        public void SwitchRangedWeapon()
        {

        }
    }
}