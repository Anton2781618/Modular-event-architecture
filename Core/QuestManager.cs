using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
   public static QuestManager Instance { get; private set; }

    private List<Quest> activeQuests = new List<Quest>();
    private List<Quest> completedQuests = new List<Quest>();

    public delegate void QuestChangeHandler(Quest quest);
    public event QuestChangeHandler OnQuestAdded;
    public event QuestChangeHandler OnQuestCompleted;
    public event QuestChangeHandler OnQuestProgressUpdated;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddQuest(Quest quest)
    {
        activeQuests.Add(quest);
        OnQuestAdded?.Invoke(quest);
    }

    public void UpdateQuestProgress(string questTitle, string objectiveDescription, int amount)
    {
        foreach (var quest in activeQuests)
        {
            if (quest.Title == questTitle)
            {
                foreach (var objective in quest.Objectives)
                {
                    if (objective.Description == objectiveDescription)
                    {
                        objective.UpdateProgress(amount);
                        quest.UpdateProgress();
                        OnQuestProgressUpdated?.Invoke(quest);
                        if (quest.IsCompleted)
                        {
                            CompleteQuest(quest);
                        }
                        return;
                    }
                }
            }
        }
    }

    private void CompleteQuest(Quest quest)
    {
        activeQuests.Remove(quest);
        completedQuests.Add(quest);
        OnQuestCompleted?.Invoke(quest);
    }

    public List<Quest> GetActiveQuests()
    {
        return new List<Quest>(activeQuests);
    }

    public List<Quest> GetCompletedQuests()
    {
        return new List<Quest>(completedQuests);
    }
}
