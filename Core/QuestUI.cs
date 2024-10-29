using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private GameObject questLogPanel;
    [SerializeField] private GameObject questEntryPrefab;
    [SerializeField] private Transform questLogContent;

    public bool IsOpen {get => questLogPanel.gameObject.activeSelf;} 

    private Dictionary<Quest, GameObject> questEntries = new Dictionary<Quest, GameObject>();

    public void Initialize()
    {
        QuestManager.Instance.OnQuestAdded += AddQuestToUI;
        QuestManager.Instance.OnQuestCompleted += UpdateQuestInUI;
        QuestManager.Instance.OnQuestProgressUpdated += UpdateQuestInUI;

        questLogPanel.SetActive(false);
    }

    public void ToggleQuestLog()
    {
        questLogPanel.SetActive(!questLogPanel.activeSelf);
        if (questLogPanel.activeSelf)
        {
            RefreshQuestLog();
        }
    }

    private void RefreshQuestLog()
    {
        foreach (var quest in QuestManager.Instance.GetActiveQuests())
        {
            if (!questEntries.ContainsKey(quest))
            {
                AddQuestToUI(quest);
            }
            else
            {
                UpdateQuestInUI(quest);
            }
        }
    }

    private void AddQuestToUI(Quest quest)
    {
        GameObject questEntryObject = Instantiate(questEntryPrefab, questLogContent);
        questEntries[quest] = questEntryObject;

        UpdateQuestEntryUI(quest, questEntryObject);
    }

    private void UpdateQuestInUI(Quest quest)
    {
        if (questEntries.TryGetValue(quest, out GameObject questEntryObject))
        {
            UpdateQuestEntryUI(quest, questEntryObject);
        }
    }

    private void UpdateQuestEntryUI(Quest quest, GameObject questEntryObject)
    {
        Text questTitleText = questEntryObject.transform.Find("QuestTitle").GetComponent<Text>();
        Text questDescriptionText = questEntryObject.transform.Find("QuestDescription").GetComponent<Text>();
        Text questProgressText = questEntryObject.transform.Find("QuestProgress").GetComponent<Text>();

        questTitleText.text = quest.Title;
        questDescriptionText.text = quest.Description;

        string progressText = "";
        foreach (var objective in quest.Objectives)
        {
            progressText += $"{objective.Description}: {objective.CurrentAmount}/{objective.RequiredAmount}\n";
        }
        questProgressText.text = progressText;

        if (quest.IsCompleted)
        {
            questEntryObject.GetComponent<Image>().color = Color.green;
        }

        questEntryObject.SetActive(true);
    }
}
