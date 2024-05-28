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
            //animator.CrossFade(animation, 0);
            animator.Play(animation);
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

        private void MoveCharacterBackwardsForMelee()
        {
            /*float amountToMoveBy;
            if (character.isFacingRight)
                amountToMoveBy = -1;
            else
                amountToMoveBy = 1;
            character._transform.position = new(character._transform.position.x + amountToMoveBy, character._transform.position.y);*/
        }

        private void MoveCharacterForwardsForMelee()
        {
            /*float amountToMoveBy;
            if (character.isFacingRight)
                amountToMoveBy = 1f;
            else
                amountToMoveBy = -1f;
            character._transform.position = new(character._transform.position.x + amountToMoveBy, character._transform.position.y);*/
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