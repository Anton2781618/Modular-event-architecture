using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

[DefaultExecutionOrder(-149)]
public class UIManager : ManagerEntity
{
    /* UIManager в событийно-ориентированной архитектуре должен отвечать за:

    1)Управление состоянием UI:
        Какие окна открыты/закрыты
        Какое окно активно
        Предотвращение конфликтов между окнами

    2)Навигация по UI:
        хранить ссылки на все UI окна
        Открытие/закрытие основных окон
        Управление стеком окон (например, для вложенных меню)
        Возврат к предыдущему окну

    3)Глобальные UI события:
        Пауза игры при открытии важных окон
        Блокировка ввода при открытых окнах
        Уведомление других систем о состоянии UI

    4)Инициализация UI:
        Создание необходимых UI сущностей
        Настройка начального состояния
        Подписка на глобальные события

    UIManager НЕ должен:
        Управлять конкретной логикой UI элементов
        Обновлять данные в UI */

    [SerializeField] private Canvas canvas;

    [Space(10)]
    [SerializeField] private QuestUI questUI;
    [SerializeField] private DialogueUI dialogueUI;
    // Добавьте здесь ссылки на другие UI компоненты
    

    protected override void Initialize()
    {
        // Globalevents.Add((GlobalEventBus.События.Юнит_создан, (data) => AddEntity((CreateUnitEvent)data)));
    }

    private void AddEntity(CreateUnitEvent obj)
    {
        if (obj.Unit is Player) 
        {
        }

        if (obj.Unit is not Player)
        {

        } 
    }

    private void Start()
    {
        // Инициализация UI компонентов
        questUI.Initialize();
        
        dialogueUI.Initialize();
        // Инициализация других UI компонентов

    }

    public bool AnySindowIsOpen()
    {
        if (questUI.IsOpen) return true;
        if (dialogueUI.IsOpen) return true;

        return false;
    }


    public void ShowQuestLog()
    {
        questUI.ToggleQuestLog();
    }

    public void ShowDialogue(DialogueNode dialogueNode)
    {

        dialogueUI.ShowDialogue(dialogueNode);
    }

    public void HideDialogue()
    {
        dialogueUI.HideDialogue();
    }


}
