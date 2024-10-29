using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Text dialogueText;
    [SerializeField] private List<Button> choiceButtons;
    public bool IsOpen {get => dialoguePanel.gameObject.activeSelf;} 


    public void Initialize()
    {
        DialogueManager.Instance.OnDialogueChanged += UpdateDialogueUI;
        dialoguePanel.SetActive(false);
    }

    public void ShowDialogue(DialogueNode node)
    {
        dialoguePanel.SetActive(true);
        UpdateDialogueUI(node);
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }

    private void UpdateDialogueUI(DialogueNode node)
    {
        dialogueText.text = node.Text;

        for (int i = 0; i < choiceButtons.Count; i++)
        {
            if (i < node.Choices.Count)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<Text>().text = node.Choices[i].Text;
                int choiceIndex = i;
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(choiceIndex));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        DialogueManager.Instance.MakeChoice(choiceIndex);
    }
}
