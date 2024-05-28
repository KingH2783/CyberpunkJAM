using UnityEngine;

namespace HL
{
    public class ParentCharacterWhenTouched : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.TryGetComponent(out CharacterManager character))
            {
                if (character != null)
                {
                    character._transform.SetParent(transform);
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
                    character._transform.SetParent(null);
                    character.isOnPlatform = false;
                }
            }
        }
    }
}