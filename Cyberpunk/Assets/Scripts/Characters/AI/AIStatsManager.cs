using UnityEngine;

namespace HL
{
    public class AIStatsManager : CharacterStatsManager
    {
        AIManager ai;

        protected override void Awake()
        {
            base.Awake();
            ai = GetComponent<AIManager>();
        }

        public override void TakeDamage(int damage, CharacterManager characterDealingDamage = null)
        {
            base.TakeDamage(damage, characterDealingDamage);
        }

        protected override void HandleDeath()
        {
            base.HandleDeath();
        }
    }
}