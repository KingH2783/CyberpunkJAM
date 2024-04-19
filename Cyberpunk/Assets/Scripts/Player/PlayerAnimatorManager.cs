using UnityEngine;

namespace HL
{
    public class PlayerAnimatorManager : MonoBehaviour
    {
        Animator animator;
        PlayerManager player;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            player = GetComponent<PlayerManager>();
        }

        public void SetAnimatorParams()
        {
            animator.SetBool("Running", player.isRunning);
            animator.SetBool("Grounded", player.isGrounded);
            animator.SetBool("OnWall", player.isOnWall);
        }

        public void PlayTargetAnimation(string animation)
        {
            animator.CrossFade(animation, 0);
        }
    }
}