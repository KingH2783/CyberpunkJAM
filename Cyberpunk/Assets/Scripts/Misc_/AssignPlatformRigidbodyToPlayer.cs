using UnityEngine;

namespace HL
{
    public class AssignPlatformRigidbodyToPlayer : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.TryGetComponent(out CharacterManager character))
            {
                if (character != null)
                {
                    character.characterLocomotion.platformRB = GetComponent<Rigidbody2D>();
                    character.isOnPlatform = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.transform.TryGetComponent(out CharacterManager character))
            {
                if (character != null)
                {
                    character.characterLocomotion.platformRB = null;
                    character.isOnPlatform = false;
                }
            }
        }
    }
}