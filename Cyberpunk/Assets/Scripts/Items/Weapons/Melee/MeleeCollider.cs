using UnityEngine;

namespace HL
{
    public class MeleeCollider : MonoBehaviour
    {
        CharacterManager characterAttacking;
        Collider2D damageCollider;

        private void Awake()
        {
            characterAttacking = GetComponentInParent<CharacterManager>();
            damageCollider = GetComponent<Collider2D>();
        }

        public void EnableCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Collider is enabled && 
            // We are hitting a character && 
            // Character is NOT on the same team (includes self) && 
            // Character is NOT dead
            if (damageCollider.enabled &&
                collision.TryGetComponent(out CharacterManager characterThatGotHit) &&
                characterAttacking.characterStatsManager.teamID != characterThatGotHit.characterStatsManager.teamID &&
                !characterThatGotHit.isDead)
            {
                characterThatGotHit.characterStatsManager.TakeDamage(characterAttacking.characterStatsManager.currentMeleeWeapon.weaponDamage, characterAttacking);
            }
        }
    }
}