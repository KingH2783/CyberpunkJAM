using UnityEngine;

namespace HL
{
    public class CombatStanceState : State
    {
        private float movement = 0;

        public override State Tick(AIManager ai)
        {
            if (ai.isDead)
            {
                return deathState;
            }

            if (ai.currentTarget.isDead)
            {
                ai.currentTarget = null;
                return idleState;
            }

            return ai.aiType switch
            {
                AIType.BasicMelee => PrepareBasicMeleeForCombat(ai),
                AIType.BasicRanged => PrepareBasicRangedForCombat(ai),
                AIType.Heavy => this,
                AIType.FastGrounded => this,
                AIType.FastFlying => this,
                AIType.Boss => this,
                _ => this,
            };
        }

        #region AI Types

        private State PrepareBasicMeleeForCombat(AIManager ai)
        {
            // If A.I is falling or performing an action, stop all movement
            if (!ai.isGrounded || ai.isPerformingAction)
            {
                ai.aiLocomotion.StopAIMovement();
                return this;
            }

            // If A.I has gotten too far from it's target, return the A.I to it's pursue target state
            if (ai.distanceFromTarget > ai.maxCirclingDistance)
            {
                return pursueTargetState;
            }

            // Attack target
            if (ai.currentRecoveryTime <= 0 &&
                ai.currentAttack != null)
            {
                return attackState;
            }
            else
            {   // Roll for new attack
                GetNewAttack(ai);
            }

            HandleMovementWhenNearTarget(ai);

            return this;
        }

        private State PrepareBasicRangedForCombat(AIManager ai)
        {
            // If A.I is falling or performing an action, stop all movement
            if (!ai.isGrounded || ai.isPerformingAction)
            {
                ai.aiLocomotion.StopAIMovement();
                return this;
            }

            // If A.I has gotten too far from it's target, return the A.I to it's pursue target state
            if (ai.distanceFromTarget > ai.maxCirclingDistance)
            {
                return pursueTargetState;
            }

            // Attack target
            if (ai.currentRecoveryTime <= 0 &&
                ai.currentAttack != null)
            {
                return attackState;
            }
            else
            {   // Roll for new attack
                GetNewAttack(ai);
            }

            HandleMovementWhenNearTarget(ai);

            return this;
        }

        #endregion

        private void GetNewAttack(AIManager ai)
        {
            int maxScore = 0;

            for (int i = 0; i < ai.aiAttacks.Length; i++)
            {
                AIAttackAction aiAttackAction = ai.aiAttacks[i];

                if (ai.distanceFromTarget <= aiAttackAction.maximumDistanceNeededToAttack &&
                    ai.distanceFromTarget >= aiAttackAction.minimumDistanceNeededToAttack)
                {
                    maxScore += aiAttackAction.attackScore;
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < ai.aiAttacks.Length; i++)
            {
                if (ai.currentAttack != null)
                    return;

                AIAttackAction enemyAttackAction = ai.aiAttacks[i];

                if (ai.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack &&
                    ai.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    temporaryScore += enemyAttackAction.attackScore;

                    if (temporaryScore > randomValue)
                    {
                        ai.currentAttack = enemyAttackAction;
                    }
                }
            }
        }

        private void HandleMovementWhenNearTarget(AIManager ai)
        {
            // Stop moving if we're next to the target
            if (ai.distanceFromTarget <= ai.stoppingDistance)
            {
                if (TargetIsToTheRight(ai) && !ai.isFacingRight)
                    ai.aiLocomotion.HandleFlip();
                else if (!TargetIsToTheRight(ai) && ai.isFacingRight)
                    ai.aiLocomotion.HandleFlip();

                movement = 0;
            }
            else
            {
                if (TargetIsToTheRight(ai))
                    movement = 1;
                else
                    movement = -1;
            }

            // Apply movement
            ai.aiLocomotion.leftOrRightAIMovementInput = movement;
        }

        private bool TargetIsToTheRight(AIManager ai)
        {
            return ai.currentTarget._transform.position.x > ai._transform.position.x;
        }
    }
}