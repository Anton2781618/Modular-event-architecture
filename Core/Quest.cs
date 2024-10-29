using UnityEngine;
using System.Collections.Generic;

public class Quest
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool IsCompleted { get; private set; }
    public List<QuestObjective> Objectives { get; private set; }

    public Quest(string title, string description)
    {
        Title = title;
        Description = description;
        IsCompleted = false;
        Objectives = new List<QuestObjective>();
    }

    public void AddObjective(QuestObjective objective)
    {
        Objectives.Add(objective);
    }

    public void UpdateProgress()
    {
        bool allCompleted = true;
        foreach (var objective in Objectives)
        {
            if (!objective.IsCompleted)
            {
                allCompleted = false;
                break;
            }
        }
        IsCompleted = allCompleted;
    }
}

public class QuestObjective
{
    public string Description { get; private set; }
    public int CurrentAmount { get; private set; }
    public int RequiredAmount { get; private set; }
    public bool IsCompleted { get; private set; }

    public QuestObjective(string description, int requiredAmount)
    {
        Description = description;
        RequiredAmount = requiredAmount;
        CurrentAmount = 0;
        IsCompleted = false;
    }

    public void UpdateProgress(int amount)
    {
        CurrentAmount += amount;
        if (CurrentAmount >= RequiredAmount)
        {
            IsCompleted = true;
        }
    }
}
