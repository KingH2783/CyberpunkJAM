using System.Collections;
using UnityEngine;

namespace HL
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager player;

        [SerializeField][Range(0, 100)] private int healAmount;
        [SerializeField] private float respawnTime;

        private Vector3 spawnPosition;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            PlayerUIManager.Instance.healthBar.SetMaxStat(maxHealth);
            spawnPosition = player._transform.position;
        }

        public override void TakeDamage(int damage, CharacterManager characterDealingDamage = null)
        {
            base.TakeDamage(damage, characterDealingDamage);
            PlayerUIManager.Instance.healthBar.SetCurrentStat(currentHealth);
        }

        protected override void HandleDeath()
        {
            base.HandleDeath();
            StartCoroutine(Respawn());
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(respawnTime);
            player._transform.position = spawnPosition;
            player.isDead = false;
            player.isPerformingAction = false;
            player.playerAnimatorManager.PlayTargetAnimation("Heal");
            currentHealth = maxHealth;
            PlayerUIManager.Instance.healthBar.SetCurrentStat(currentHealth);
        }

        public void HandleHeal()
        {
            if (player.isDead ||
                player.isPerformingAction ||
                player.isBeingDamaged ||
                player.isJumping ||
                player.isWallJumping)
                return;

            player.playerAnimatorManager.PlayTargetAnimation("Heal", true);
        }

        public void ApplyHeal()
        {
            currentHealth += healAmount;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
            PlayerUIManager.Instance.healthBar.SetCurrentStat(currentHealth);
        }
    }
}