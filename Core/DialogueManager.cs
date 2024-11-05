using UnityEngine;

namespace ModularEventArchitecture
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }

        private DialogueNode currentNode;
        private NPC currentNPC;
        private Player currentPlayer;

        public delegate void DialogueChangedHandler(DialogueNode node);
        public event DialogueChangedHandler OnDialogueChanged;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void StartDialogue(NPC npc, DialogueNode startNode)
        {
            currentNPC = npc;
            currentNode = startNode;
            currentPlayer = FindObjectOfType<Player>(); // Предполагается, что в сцене есть только один игрок
            OnDialogueChanged?.Invoke(currentNode);
        }

        public void MakeChoice(int choiceIndex)
        {
            if (currentNode != null && choiceIndex >= 0 && choiceIndex < currentNode.Choices.Count)
            {
                if (currentNode.AssociatedQuest != null)
                {
                    OfferQuest(currentNode.AssociatedQuest);
                }

                currentNode = currentNode.Choices[choiceIndex].NextNode;
                if (currentNode != null)
                {
                    OnDialogueChanged?.Invoke(currentNode);
                }
                else
                {
                    EndDialogue();
                }
            }
        }

        private void OfferQuest(Quest quest)
        {
            if (currentPlayer != null)
            {
                // currentPlayer.ReceiveQuest(quest);
            }
        }

        private void EndDialogue()
        {
            currentNode = null;
            currentNPC = null;
            currentPlayer = null;
            OnDialogueChanged?.Invoke(null);
        }
    }
}