using UnityEngine;

namespace HL
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        protected Animator animator;
        CharacterManager character;
        MeleeCollider meleeCollider;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<CharacterManager>();
            meleeCollider = GetComponentInChildren<MeleeCollider>(true);
        }

        public virtual void PlayTargetAnimation(string animation, bool stopMovement = false)
        {
            character.isPerformingAction = stopMovement;
            animator.CrossFade(animation, 0);
        }

        #region Animation Events

        private void EnableMeleeDamageCollider()
        {
            if (meleeCollider != null)
                meleeCollider.EnableCollider();
        }

        public void DisableMeleeDamageCollider()
        {
            if (meleeCollider != null)
                meleeCollider.DisableCollider();
        }

        public void ResetOnAnimatorIdle()
        {
            DisableMeleeDamageCollider();
            character.isPerformingAction = false;
            character.isDoingMeleeAttack = false;
        }

        #endregion
    }
}