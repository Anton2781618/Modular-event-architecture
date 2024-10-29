using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] 
public class NPC : Unit
{
    public int ModulesCount { get => modules.Count; }
    
    public Quest AvailableQuest { get; private set; }
    
    [field: SerializeField] public DialogueNode StartDialogue { get; private set; }

    protected override void Initialize()
    {
        base.Initialize();

        InitializeDialogue();

    }

    [ContextMenu("show")]
    public void Show()
    {
        LocalEvents.ShowAllEvents();
    }

    private void InitializeDialogue()
    {
        // Пример создания диалога с квестом
        Quest sampleQuest = new Quest("Помоги деревне", "Убей 10 бандитов в лесу");
        sampleQuest.AddObjective(new QuestObjective("Убить бандитов", 10));

        StartDialogue = new DialogueNode("Привет, путник! Чем я могу тебе помочь?");
        DialogueNode questNode = new DialogueNode("У меня есть важное задание для тебя.", sampleQuest);
        DialogueNode acceptNode = new DialogueNode("Спасибо, что согласился помочь! Вот детали задания.");
        DialogueNode declineNode = new DialogueNode("Жаль, что ты не можешь помочь. Возвращайся, если передумаешь.");
        DialogueNode endNode = new DialogueNode("Удачи в твоих странствиях!");

        StartDialogue.AddChoice("Расскажи о задании", questNode);
        StartDialogue.AddChoice("До свидания", endNode);

        questNode.AddChoice("Я возьмусь за это задание", acceptNode);
        questNode.AddChoice("Извини, но я не могу сейчас этим заняться", declineNode);

        acceptNode.AddChoice("Я понял, приступаю к выполнению", endNode);
        declineNode.AddChoice("Вернуться к началу разговора", StartDialogue);
    }


    [ContextMenu("dialog")]
    public void InteractWithPlayer()
    {
        InteractWithPlayer(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>());
    }

    public void InteractWithPlayer(Player player)
    {
        Debug.Log(StartDialogue.Text);
        DialogueManager.Instance.StartDialogue(this, StartDialogue);
    }

    public override void Use()
    {
        base.Use();
        InteractWithPlayer();
    }
}
