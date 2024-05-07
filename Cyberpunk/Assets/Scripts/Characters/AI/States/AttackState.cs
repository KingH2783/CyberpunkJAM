using UnityEngine;

namespace HL
{
    public class AttackState : State
    {
        private bool willDoComboOnNextAttack = false;

        public override State Tick(AIManager ai)
        {
            if (ai.isDead)
            {
                ResetStateFlags(ai);
                return deathState;
            }

            if (ai.currentTarget.isDead)
            {
                ResetStateFlags(ai);
                return idleState;
            }

            return ai.aiType switch
            {
                AIType.BasicMelee => ProcessBasicMeleeAttack(ai),
                AIType.BasicRanged => this,
                AIType.Heavy => this,
                AIType.FastGrounded => this,
                AIType.FastFlying => this,
                AIType.Boss => this,
                _ => this,
            };
        }

        private State ProcessBasicMeleeAttack(AIManager ai)
        {
            // Target dies
            if (ai.currentTarget.isDead)
            {
                ResetStateFlags(ai);
                ai.currentTarget = null;
                return idleState;
            }

            // Target is too far, get closer
            if (ai.distanceFromTarget > ai.maxCirclingDistance)
            {
                ResetStateFlags(ai);
                return pursueTargetState;
            }

            ai.aiLocomotion.StopAIMovement();

            // If we're gonna combo next, assign when ready
            if (!ai.isPerformingAction && willDoComboOnNextAttack)
            {
                // Assign combo attack to current attack
                ai.currentAttack = ai.currentAttack.comboAttack;
            }

            // Attack and figure out if we can combo
            if (!ai.isPerformingAction && ai.currentAttack != null)
            {
                AttackTarget(ai);
                RollForComboChance(ai);
            }

            // We check if we still have an attack -->
            // If we do that means we can combo so go back to the top of this state
            if (ai.currentAttack == null)
                return combatStanceState;
            else
                return this;
        }

        private void AttackTarget(AIManager ai)
        {
            ai.isPerformingAction = true;
            ai.aiAnimatorManager.PlayTargetAnimation(ai.currentAttack.attackAnimationName);
            ai.currentRecoveryTime = ai.currentAttack.recoveryTime;
        }

        private void RollForComboChance(AIManager ai)
        {
            float comboChance = Random.Range(0, 100);

            if (ai.allowAIToPerformCombos &&
                ai.currentAttack.actionCanCombo &&
                comboChance <= ai.comboLikelyHood)
            {
                willDoComboOnNextAttack = true;
            }
            else
            {
                willDoComboOnNextAttack = false;
                ai.currentAttack = null;
            }
                
        }

        private void ResetStateFlags(AIManager ai)
        {
            willDoComboOnNextAttack = false;
            ai.currentAttack = null;
        }
    }
}