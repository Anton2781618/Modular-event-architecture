using UnityEngine;

[CompatibleUnit(typeof(InputManager))] 
public class InputModule : ModuleBase
{
    protected override void Initialize()
    {
        base.Initialize();
    }

    public override void UpdateMe()
    {
        CheckUIInput();
        CheckGameplayInput();
        CheckSystemInput();
    }

    private void CheckUIInput()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            // Публикуем через GlobalEventBus, так как это глобальное событие
            GlobalEventBus.Instance.Publish(GlobalEventBus.События.UI.Показать_Окно_Помощи, new UIStateChangedEvent { IsAnyWindowOpen = true });
        }
    }

    private void CheckGameplayInput()
    {
        // Здесь будет проверка игрового ввода
        // Например, атака, прыжок и т.д.
    }

    private void CheckSystemInput()
    {
        // Здесь будет проверка системного ввода
        // Например, пауза, выход и т.д.
    }
}
