using UnityEngine;

namespace HL
{
    public class PursueTargetState : State
    {
        public override State Tick(AIManager ai)
        {
            if (ai.isDead)
                return deathState;

            if (ai.currentTarget.isDead)
            {
                ai.currentTarget = null;
                return idleState;
            }

            return ai.aiType switch
            {
                AIType.BasicMelee => BasicMeleePursuit(ai),
                AIType.BasicRanged => this,
                AIType.Heavy => this,
                AIType.FastGrounded => this,
                AIType.FastFlying => this,
                AIType.Boss => this,
                _ => this,
            };
        }

        private State BasicMeleePursuit(AIManager ai)
        {
            // Target is outside of aggro range
            if (ai.distanceFromTarget > ai.maxAggroRange)
                return idleState;

            // Stop movement while interacting
            if (ai.isPerformingAction)
            {
                ai.aiLocomotion.StopAIMovement();
                return this;
            }

            // Make AI run towards target
            if (ai.distanceFromTarget > ai.maxCirclingDistance)
            {
                if (TargetIsToTheRight(ai))
                    ai.aiLocomotion.leftOrRightAIMovementInput = 1;
                else
                    ai.aiLocomotion.leftOrRightAIMovementInput = -1;
            }

            // AI has reached the target
            if (ai.distanceFromTarget <= ai.maxCirclingDistance)
                return combatStanceState;
            else
                return this;
        }

        private bool TargetIsToTheRight(AIManager ai)
        {
            return ai.currentTarget._transform.position.x > ai._transform.position.x;
        }
    }
}