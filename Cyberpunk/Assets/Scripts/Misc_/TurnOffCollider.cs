using UnityEngine;

namespace HL
{
    public class TurnOffCollider : StateMachineBehaviour
    {
        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.TryGetComponent(out CharacterAnimatorManager character))
            {
                character.DisableMeleeDamageCollider();
            }
        }
    }
}