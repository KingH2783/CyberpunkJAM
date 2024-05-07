using UnityEngine;

namespace HL
{
    public class DeathState : State
    {
        public override State Tick(AIManager ai)
        {
            if (ai.currentAttack != null)
                ai.currentAttack = null;

            ai.aiLocomotion.StopAIMovement();

            return this;
        }
    }
}