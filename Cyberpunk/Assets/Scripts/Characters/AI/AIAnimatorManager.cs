using UnityEngine;

namespace HL
{
    public class AIAnimatorManager : CharacterAnimatorManager
    {
        AIManager ai;
        AttackState aiAttackState;

        protected override void Awake()
        {
            base.Awake();
            ai = GetComponent<AIManager>();
            aiAttackState = GetComponentInChildren<AttackState>();
        }

        public void SetAnimatorParams()
        {
            animator.SetBool("Running", ai.isRunning);
            animator.SetBool("Grounded", ai.isGrounded);
            animator.SetBool("BeingDamaged", ai.isBeingDamaged);
        }

        public override void PlayTargetAnimation(string animation, bool stopMovement = false)
        {
            base.PlayTargetAnimation(animation, stopMovement);
        }

        public void ShootAnimEvent()
        {
            StartCoroutine(aiAttackState.ShootTarget(ai));
        }
    }
}