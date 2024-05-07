using UnityEngine;

namespace HL
{
    public abstract class State : MonoBehaviour
    {
        protected IdleState idleState;
        protected PursueTargetState pursueTargetState;
        protected CombatStanceState combatStanceState;
        protected AttackState attackState;
        protected DeathState deathState;

        protected virtual void Awake()
        {
            idleState = GetComponent<IdleState>();
            pursueTargetState = GetComponent<PursueTargetState>();
            combatStanceState = GetComponent<CombatStanceState>();
            attackState = GetComponent<AttackState>();
            deathState = GetComponent<DeathState>();
        }

        public abstract State Tick(AIManager ai);
    }
}