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

        public virtual void PlayTargetAnimation(string animation)
        {
            animator.CrossFade(animation, 0);
        }

        #region Animation Events

        public void EnableInvulnerability()
        {
            character.isInvulnerable = true;
        }

        public void DisableInvulnerability()
        {
            character.isInvulnerable = false;
        }

        public void EnableMeleeDamageCollider()
        {
            meleeCollider.EnableCollider();
        }

        public void DisableMeleeDamageCollider()
        {
            meleeCollider.DisableCollider();
        }

        #endregion
    }
}