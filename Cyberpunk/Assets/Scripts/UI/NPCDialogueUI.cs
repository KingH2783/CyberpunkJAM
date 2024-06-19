using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HL
{
    public class NPCDialogueUI : MonoBehaviour
    {
        [SerializeField] private GameObject NPCDialogueBackground;
        [SerializeField] private Image NPCCharacterPortrait;
        [SerializeField] private TextMeshProUGUI NPCText;
        [SerializeField] private TextMeshProUGUI NPCNameText;
        [SerializeField] private float dialogueSpeed;

        public void EnableNPCDialogue(Sprite portrait, string name)
        {
            PlayerUIManager.Instance.DisableHUD();
            NPCDialogueBackground.SetActive(true);
            NPCCharacterPortrait.sprite = portrait;
            NPCNameText.text = name;
        }

        public void UpdateNPCDialogue(string text)
        {
            NPCText.text = "";
            StartCoroutine(WriteSentence(text));
        }

        public void DisableNPCDialogue()
        {
            PlayerUIManager.Instance.EnableHUD();
            NPCDialogueBackground.SetActive(false);
        }

        private IEnumerator WriteSentence(string text)
        {
            foreach (char character in text.ToCharArray())
            {
                NPCText.text += character;
                yield return new WaitForSeconds(dialogueSpeed);
            }
        }
    }
}