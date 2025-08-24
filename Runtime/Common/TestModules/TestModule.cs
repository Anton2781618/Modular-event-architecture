using UnityEngine;


namespace ModularEventArchitecture
{
    public class TestModule : ModuleBase
    {
        public override void Initialize()
        {
            // Подписка на тестовые события которое без колбэка
            Entity.SubscribeLocalEvent<EventBase>(BasicActionsTypes.Test_Event, TestLocal);

            // Подписка на тестовое событие которое с колбэком
            Entity.SubscribeLocalEvent<EventBase, ResponseEvent>(BasicActionsTypes.Test_Event, OnTestLocalResponse);

            // Подписка на глобальные тестовые события которое без колбэка
            Entity.SubscribeGlobalEvent<EventBase>(BasicActionsTypes.Test_Event, OnTestGlobal);

            // Подписка на глобальные тестовые события которое с колбэком
            Entity.SubscribeGlobalEvent<EventBase, ResponseEvent>(BasicActionsTypes.Test_Event, OnTestGlobalResponse);
        }

        private void TestLocal(EventBase eventBase)
        {
            Debug.Log($"<color=green>Тестовое ЛОКАЛЬНОЕ событие</color> на объекте {Entity.name}");
        }
        private ResponseEvent OnTestLocalResponse(EventBase eventBase)
        {
            Debug.Log($"<color=green>Тестовое ЛОКАЛЬНОЕ событие</color> на объекте {Entity.name} <color=green> С ОТВЕТОМ</color>");
            return new ResponseEvent { Result = "Колбэк" };
        }
        private void OnTestGlobal(EventBase eventBase)
        {
            Debug.Log($"<color=red>Тестовое ГЛОБАЛЬНОЕ событие</color> на объекте {Entity.name}");
        }
        private ResponseEvent OnTestGlobalResponse(EventBase eventBase)
        {
            Debug.Log($"<color=red>Тестовое ГЛОБАЛЬНОЕ событие</color> на объекте {Entity.name} <color=red> С ОТВЕТОМ</color>");
            return new ResponseEvent { Result = "Колбэк" };
        }

        public override void UpdateMe()
        {
            // Здесь можно реализовать логику обновления модуля, если требуется
            // Debug.Log($"<color=blue>Обновление модуля {GetType().Name} на объекте {Entity.name}</color>");
        }
    }
}