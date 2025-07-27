using UnityEngine;


namespace ModularEventArchitecture
{
    public class TestModule : ModuleBase
    {
        public override void Initialize()
        {
            Entity.SubscribeLocalEvent<EventBase>(BasicActionsTypes.Test_Event, TestLocal);


            Entity.SubscribeGlobalEvent<EventBase>(BasicActionsTypes.Test_Event, TestGlobal);

        }

        private void TestLocal(EventBase eventBase)
        {
            Debug.Log($"<color=green>Тестовое ЛОКАЛЬНОЕ событие</color> на объекте {Entity.name}");
        }
        private void TestGlobal(EventBase eventBase)
        {
            Debug.Log($"<color=red>Тестовое ГЛОБАЛЬНОЕ событие</color> на объекте {Entity.name}");
        }

        public override void UpdateMe()
        {
            // Здесь можно реализовать логику обновления модуля, если требуется
            Debug.Log($"<color=blue>Обновление модуля {GetType().Name} на объекте {Entity.name}</color>");
        }
    }
}