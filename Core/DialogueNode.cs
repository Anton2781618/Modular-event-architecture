using System.Collections.Generic;
using System;
using UnityEngine;


public class DialogueNode
{
    public string Text { get; private set; }
    public List<DialogueChoice> Choices { get; private set; }
    public Quest AssociatedQuest { get; private set; }

    public DialogueNode(string text, Quest associatedQuest = null)
    {
        Text = text;
        Choices = new List<DialogueChoice>();
        AssociatedQuest = associatedQuest;
    }

    public void AddChoice(string text, DialogueNode nextNode)
    {
        Choices.Add(new DialogueChoice(text, nextNode));
    }
}


public class DialogueChoice
{
    public string Text { get; private set; }
    public DialogueNode NextNode { get; private set; }

    public DialogueChoice(string text, DialogueNode nextNode)
    {
        Text = text;
        NextNode = nextNode;
    }
}
