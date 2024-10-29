using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

[DefaultExecutionOrder(-149)]
public class UIManager : GameEntity
{

    [SerializeField] private Canvas canvas;
    [SerializeField] private PlayerUI PlayerHud;

    [Space(10)]
    [SerializeField] private QuestUI questUI;
    [SerializeField] private DialogueUI dialogueUI;
    // Добавьте здесь ссылки на другие UI компоненты
    
    [Space(10)]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject pauseMenuPanel;

    protected override void Initialize()
    {
        Globalevents.Add((GlobalEventBus.События.Юнит_создан, (data) => AddEntity((CreateUnitEvent)data)));
    }

    private void AddEntity(CreateUnitEvent obj)
    {
        if (obj.Unit is Player) 
        {
            PlayerHud.SetLocalEventBus(obj.Unit.LocalEvents);
            
            PlayerHud.SetTarget(obj.Unit.gameObject);
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

        // Установка начального состояния UI
        SetActivePanel(mainMenuPanel);
    }

    public bool AnySindowIsOpen()
    {
        if (questUI.IsOpen) return true;
        if (dialogueUI.IsOpen) return true;

        return false;
    }

    public void ShowMainMenu()
    {
        SetActivePanel(mainMenuPanel);
    }

    public void ShowGameplayUI()
    {
        SetActivePanel(gameplayUI);
    }

    public void TogglePauseMenu()
    {
        pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);
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

    private void SetActivePanel(GameObject panel)
    {
        // mainMenuPanel.SetActive(panel == mainMenuPanel);
        // gameplayUI.SetActive(panel == gameplayUI);
        // pauseMenuPanel.SetActive(panel == pauseMenuPanel);
    }

    // Методы для обновления различных элементов UI
    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        // Обновление отображения здоровья игрока
    }

    public void UpdateInventoryUI()
    {
        // Обновление отображения инвентаря
    }

}
