using UnityEngine;

namespace ModularEventArchitecture
{
    [CompatibleUnit(typeof(GameManager))] 
    public class InputModule : ModuleBase
    {
        public override void Initialize()
        {
        }

        public override void UpdateMe()
        {
            CheckUIInput();
            CheckGameplayInput();
            CheckSystemInput();
        }

        private void CheckUIInput()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Debug.Log("!!!!!!!!!!!!!!!!!!");
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                // Публикуем через GlobalEventBus, так как это глобальное событие
                GlobalEventBus.Instance.Publish(BasicActionsTypes.UI.Show_Help_Window, new UIStateChangedEvent { IsAnyWindowOpen = true });
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
}