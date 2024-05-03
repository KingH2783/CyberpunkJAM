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

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
        }

        protected override void HandleDeath()
        {
            base.HandleDeath();
        }
    }
}