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

        public void EnableNPCDialogue(Sprite portrait, string name)
        {
            PlayerUIManager.Instance.DisableHUD();
            PlayerUIManager.Instance.pauseMenu.isGamePaused = true;
            Time.timeScale = 0;

            NPCDialogueBackground.SetActive(true);
            NPCCharacterPortrait.sprite = portrait;
            NPCNameText.text = name;
        }

        public void UpdateNPCDialogue(string text)
        {
            NPCText.text = text;
        }

        public void DisableNPCDialogue()
        {
            PlayerUIManager.Instance.EnableHUD();
            PlayerUIManager.Instance.pauseMenu.isGamePaused = false;
            Time.timeScale = 1;
            // This will cause a bug if you open and close the pause menu while in a
            // dialogue box where the the game will unpause but still be in dialogue

            NPCDialogueBackground.SetActive(false);
        }
    }
}