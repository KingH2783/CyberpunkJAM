using UnityEngine;

namespace HL
{
    public class InteractableNPC : Interactable
    {
        [SerializeField] private NPCDialogue[] NPCDialogues;

        private int interactionIndex = 0;
        private int textIndex = 0;

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            while (interactionIndex < NPCDialogues.Length)
            {
                NPCDialogue currentDialogue = NPCDialogues[interactionIndex];

                if (textIndex == 0)
                {
                    PlayerUIManager.Instance.npcDialogueUI.EnableNPCDialogue(currentDialogue.NPCPortrait, currentDialogue.NPCName);
                    PlayerUIManager.Instance.npcDialogueUI.UpdateNPCDialogue(currentDialogue.NPCDialogueText[textIndex]);
                    textIndex++;
                    break;
                }
                else if (textIndex < currentDialogue.NPCDialogueText.Length)
                {
                    PlayerUIManager.Instance.npcDialogueUI.UpdateNPCDialogue(currentDialogue.NPCDialogueText[textIndex]);
                    textIndex++;
                    break;
                }
                else
                {
                    // Move to the next dialogue struct
                    interactionIndex++;
                    textIndex = 0;

                    if (interactionIndex < NPCDialogues.Length)
                    {
                        currentDialogue = NPCDialogues[interactionIndex];
                        PlayerUIManager.Instance.npcDialogueUI.EnableNPCDialogue(currentDialogue.NPCPortrait, currentDialogue.NPCName);
                        PlayerUIManager.Instance.npcDialogueUI.UpdateNPCDialogue(currentDialogue.NPCDialogueText[textIndex]);
                        textIndex++;
                        break;
                    }
                }
            }

            // Disable the UI if we've reached the end of all dialogues
            if (interactionIndex >= NPCDialogues.Length)
            {
                PlayerUIManager.Instance.npcDialogueUI.DisableNPCDialogue();
                interactionIndex = 0;
            }
        }

        [System.Serializable]
        private struct NPCDialogue
        {
            public Sprite NPCPortrait;
            public string NPCName;
            [TextArea] public string[] NPCDialogueText;
        }
    }
}