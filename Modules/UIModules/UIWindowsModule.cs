using UnityEngine;

[CompatibleUnit(typeof(UIManager))]
public class UIWindowsModule : ModuleBase
{
    [SerializeField] private UIHelpMenu helpMenu;
    // Добавьте другие окна по мере необходимости

    protected override void Initialize()
    {
        base.Initialize();

        // Подписываемся на события показа окон
        Globalevents.Add((GlobalEventBus.События.UI.Показать_Окно_Помощи, (data) => OnShowHelpMenuRequested((UIStateChangedEvent)data)));

        // Изначально скрываем все окна
        if (helpMenu) helpMenu.gameObject.SetActive(false);
    }

    private void OnShowHelpMenuRequested(UIStateChangedEvent data)
    {
        if (!helpMenu) return;

        // Переключаем состояние окна помощи
        bool newState = !helpMenu.gameObject.activeSelf;

        helpMenu.gameObject.SetActive(newState);

        // Публикуем событие об изменении состояния UI
        Character.LocalEvents.Publish(GlobalEventBus.События.UI.UI_Состояние_Изменилось,new UIStateChangedEvent { IsAnyWindowOpen = IsAnyWindowOpen() });
    }

    public bool IsAnyWindowOpen()
    {
        // Проверяем, открыто ли какое-либо окно
        if (helpMenu && helpMenu.gameObject.activeSelf) return true;
        // Добавьте проверки для других окон

        return false;
    }

    public override void UpdateMe()
    {
        
    }
}

// Событие для оповещения об изменении состояния UI
public struct UIStateChangedEvent : IEventData
{
    public bool IsAnyWindowOpen;
}
