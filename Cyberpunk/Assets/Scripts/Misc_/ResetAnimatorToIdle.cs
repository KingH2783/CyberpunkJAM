using UnityEngine;

namespace HL
{
    public class ResetAnimatorToIdle : StateMachineBehaviour
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.TryGetComponent(out CharacterAnimatorManager character))
            {
                character.ResetOnAnimatorIdle();
            }
        }
    }
}