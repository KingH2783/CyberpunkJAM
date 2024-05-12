using UnityEngine;

namespace HL
{
    public class IdleState : State
    {
        public override State Tick(AIManager ai)
        {
            if (ai.isDead)
                return deathState;

            return ai.aiType switch
            {
                AIType.BasicMelee => BasicIdle(ai),
                AIType.BasicRanged => BasicIdle(ai),
                AIType.Heavy => this,
                AIType.FastGrounded => this,
                AIType.FastFlying => this,
                AIType.Boss => this,
                _ => this,
            };
        }

        #region AI Types

        private State BasicIdle(AIManager ai)
        {
            HandleDetection(ai);

            ai.aiLocomotion.StopAIMovement();

            if (ai.currentTarget != null)
                return pursueTargetState;
            else
                return this;
        }

        #endregion

        private void HandleDetection(AIManager ai)
        {
            // Searches for a potential target within the detection radius
            Collider2D[] colliders = Physics2D.OverlapCircleAll(ai._transform.position, ai.detectionRadius, ai.detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                // If a potential target is found
                if (colliders[i].transform.TryGetComponent(out CharacterManager potentialTarget))
                {
                    // If target is not on the same team (includes self) && 
                    // If target is not dead
                    if (potentialTarget.characterStatsManager.teamID != ai.aiStatsManager.teamID &&
                        !potentialTarget.isDead)
                    {
                        // If our line of sight hits a wall
                        if (Physics.Linecast(ai.lineOfSightTransform.position, potentialTarget.lineOfSightTransform.position, ai.layersThatBlockLineOfSight))
                            continue; // Continue to the next potentialTarget
                        else
                            ai.currentTarget = potentialTarget;
                    }
                }
            }
        }
    }
}