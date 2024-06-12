using UnityEngine;

namespace HL
{
    public class InteractableNPC : Interactable
    {
        [SerializeField] private Sprite NPCPortrait;
        [SerializeField] private string NPCName;
        [SerializeField][TextArea] private string[] NPCDialogue;

        private int interactionIndex = 0;

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            if (interactionIndex == 0)
            {
                PlayerUIManager.Instance.npcDialogueUI.EnableNPCDialogue(NPCPortrait, NPCName);
                PlayerUIManager.Instance.npcDialogueUI.UpdateNPCDialogue(NPCDialogue[interactionIndex]);
                interactionIndex++;
            }
            else if (interactionIndex < NPCDialogue.Length)
            {
                PlayerUIManager.Instance.npcDialogueUI.UpdateNPCDialogue(NPCDialogue[interactionIndex]);
                interactionIndex++;
            }
            else
            {
                PlayerUIManager.Instance.npcDialogueUI.DisableNPCDialogue();
                interactionIndex = 0;
            }
        }
    }
}