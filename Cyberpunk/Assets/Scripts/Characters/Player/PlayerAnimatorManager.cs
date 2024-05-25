using UnityEngine;

namespace HL
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        public void SetAnimatorParams()
        {
            animator.SetBool("Running", player.isRunning);
            animator.SetBool("Jumping", player.isJumping);
            animator.SetBool("Grounded", player.isGrounded);
            animator.SetBool("OnWall", player.isOnWall);
            animator.SetBool("DoingRangedAttack", player.isDoingRangedAttack);
        }

        public override void PlayTargetAnimation(string animation, bool stopMovement = false)
        {
            base.PlayTargetAnimation(animation, stopMovement);
        }

        private void FirstHalfOfRunningAnimation()
        {

        }

        private void SecondHalfOfRunningAnimation()
        {

        }
    }
}