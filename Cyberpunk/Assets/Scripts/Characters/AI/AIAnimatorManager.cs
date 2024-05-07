using UnityEngine;

namespace HL
{
    public class AIAnimatorManager : CharacterAnimatorManager
    {
        AIManager ai;

        protected override void Awake()
        {
            base.Awake();
            ai = GetComponent<AIManager>();
        }

        public void SetAnimatorParams()
        {
            animator.SetBool("Running", ai.isRunning);
            animator.SetBool("Grounded", ai.isGrounded);
        }

        public override void PlayTargetAnimation(string animation)
        {
            base.PlayTargetAnimation(animation);
        }
    }
}