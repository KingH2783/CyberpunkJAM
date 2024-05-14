using UnityEngine;

namespace HL
{
    public class TurnOffCollider : StateMachineBehaviour
    {
        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.TryGetComponent(out CharacterManager character))
            {
                character.characterAnimatorManager.DisableMeleeDamageCollider();
                character.isPerformingAction = false;
                character.isDoingMeleeAttack = false;
            }
        }
    }
}