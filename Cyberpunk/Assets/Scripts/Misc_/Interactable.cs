using TMPro;
using UnityEngine;

namespace HL
{
    [RequireComponent (typeof(BoxCollider2D))]
    public class Interactable : MonoBehaviour
    {
        BoxCollider2D boxCollider;
        TextMeshProUGUI text;

        protected virtual void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        public virtual void Interact(PlayerManager player)
        {
            Debug.Log("You interacted with an object!");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerManager player))
            {
                text.enabled = true;
                player.currentInteractableObject = this;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerManager player))
            {
                text.enabled = false;
                player.currentInteractableObject = null;
            }
        }

        #region Editor Debugs

        private void OnValidate()
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
        }

        #endregion
    }
}