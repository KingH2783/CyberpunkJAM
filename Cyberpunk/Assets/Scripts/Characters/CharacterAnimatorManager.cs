using UnityEngine;

namespace HL
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        protected Animator animator;
        MeleeCollider meleeCollider;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            meleeCollider = GetComponentInChildren<MeleeCollider>(true);
        }

        public virtual void PlayTargetAnimation(string animation)
        {
            animator.CrossFade(animation, 0);
        }

        #region Animation Events

        public void EnableMeleeDamageCollider()
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
        }

        #endregion
    }
}