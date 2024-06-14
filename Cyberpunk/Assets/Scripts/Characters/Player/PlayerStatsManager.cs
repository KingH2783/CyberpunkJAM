using UnityEngine;

namespace HL
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            PlayerUIManager.Instance.healthBar.SetMaxStat(maxHealth);
        }

        public override void TakeDamage(int damage, CharacterManager characterDealingDamage = null)
        {
            base.TakeDamage(damage, characterDealingDamage);
            PlayerUIManager.Instance.healthBar.SetCurrentStat(currentHealth);
        }

        protected override void HandleDeath()
        {
            base.HandleDeath();
        }
    }
}