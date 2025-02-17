using UnityEngine;

namespace ModularEventArchitecture
{
    [CompatibleUnit(typeof(UIManager))]
    public class UIWindowsModule : ModuleBase
    {
        //-------------------------------------------------------------------------------------
        /// модуль создаст окна и установит их в нужный канвас
        [Tools.Information("Этот модуль представляет из себя систему окон в игре, он отвечает за отображение и управление окнами. Модуль произведет (Instantiate) окон и установит их в нужный канвас", Tools.InformationAttribute.InformationType.Info, false)]
        //-------------------------------------------------------------------------------------
        [SerializeField] private Canvas canvas;
        [SerializeField] private UIWindow helpMenuPrefab;
        private UIWindow helpMenu;
        // Добавьте другие окна по мере необходимости

        //!-------------------------------------------------------------------------------------

        public override void Initialize()
        {

            // Подписываемся на события показа окон
            Entity.Globalevents.Add((BasicActionsTypes.UI.Show_Help_Window, (data) => OnShowHelpMenuRequested((UIStateChangedEvent)data)));

            CreateWindow();
        }

        private void CreateWindow()
        {
            helpMenu = Instantiate(helpMenuPrefab, canvas.transform);

            helpMenu.gameObject.SetActive(false);
        }



        private void OnShowHelpMenuRequested(UIStateChangedEvent data)
        {
            helpMenu.gameObject.SetActive(!helpMenu.gameObject.activeSelf);

            // Публикуем событие об изменении состояния UI
            Entity.LocalEvents.Publish(BasicActionsTypes.UI.Is_UI_Open ,new UIStateChangedEvent { IsAnyWindowOpen = IsAnyWindowOpen() });
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
}