using UnityEngine;

namespace HL
{
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Team ID")]
        public int teamID;

        [Header("Health Stats")]
        public int maxHealth;
        public int currentHealth;

        [Header("Current Weapons")]
        public MeleeWeapon currentMeleeWeapon;
        public RangedWeapon currentRangedWeapon;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void TakeDamage(int damage, CharacterManager characterDealingDamage = null)
        {
            if (character.isDead || character.isInvulnerable)
                return;

            currentHealth -= damage;

            if (currentHealth > 0) // The "true" at the end stops you from doing other things during the hurt animation
                character.characterAnimatorManager.PlayTargetAnimation("Hurt", true);
            else
                HandleDeath();

            if (characterDealingDamage != null)
            {
                if (characterDealingDamage._transform.position.x < character._transform.position.x)
                    character.characterLocomotion.HandleKnockbackOnHit(false);
                else
                    character.characterLocomotion.HandleKnockbackOnHit(true);
            }
        }

        protected virtual void HandleDeath()
        {
            currentHealth = 0;
            character.isDead = true;
            character.characterAnimatorManager.PlayTargetAnimation("Death", true);
        }
    }
}