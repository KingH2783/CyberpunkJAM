using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

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

        private void Start()
        {
            if (ai.aiType == AIType.Boss)
                PlayerUIManager.Instance.bossHealthBar.SetMaxStat(maxHealth);
        }

        public override void TakeDamage(int damage, CharacterManager characterDealingDamage = null)
        {
            if (ai.isDead || ai.isInvulnerable)
                return;

            currentHealth -= damage;

            if (currentHealth > 0) // The "true" at the end stops you from doing other things during the hurt animation
            {
                if (ai.aiType != AIType.Boss)
                {
                    ai.isBeingDamaged = true;
                    ai.characterAnimatorManager.PlayTargetAnimation("Hurt", true);
                }
            }
            else
                HandleDeath();

            if (characterDealingDamage != null && ai.aiType != AIType.Boss)
            {
                if (characterDealingDamage._transform.position.x < ai._transform.position.x)
                    ai.characterLocomotion.HandleKnockbackOnHit(false);
                else
                    ai.characterLocomotion.HandleKnockbackOnHit(true);
            }

            if (ai.aiType == AIType.Boss)
                PlayerUIManager.Instance.bossHealthBar.SetCurrentStat(currentHealth);
        }

        protected override void HandleDeath()
        {
            base.HandleDeath();
            if (ai.aiType == AIType.Boss)
                StartCoroutine(BossHealthBarOff());
        }

        private IEnumerator BossHealthBarOff()
        {
            yield return new WaitForSeconds(5);
            PlayerUIManager.Instance.bossHealthBar.TurnOffBossHealthBar();
        }
    }
}