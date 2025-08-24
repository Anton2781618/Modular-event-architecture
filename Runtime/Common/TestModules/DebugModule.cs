using UnityEngine;

namespace ModularEventArchitecture
{
    public class DebugModule : ModuleBase
    {
        public override void Initialize()
        {
            Entity.PublishGlobalEvent(BasicActionsTypes.Test_Event, new EventBase());
        }

        [Button("Вывести в консоль события LocalEventBus")]
        private void DebugLocalEventBus()
        {
            Entity.LogLocalEvents();
        }

        [Button("Вывести в консоль события GlobalEventBus")]
        private void DebugGlobalEventBus()
        {
            Entity.LogGlobalEvents();
        }

        [Button("Вызвать тестовое ЛОКАЛЬНОЕ событие")]
        private void TestLocalEvent()
        {
            //вызвать событие без ответа
            Entity.PublishLocalEvent(BasicActionsTypes.Test_Event, new EventBase());
        }

        [Button("Вызвать тестовое ЛОКАЛЬНОЕ событие с ответом")]
        private void TestLocalResponceEvent()
        {
            //вызвать событие с ответом
            ResponseEvent response = Entity.PublishLocalEvent<EventBase, ResponseEvent>(BasicActionsTypes.Test_Event, new EventBase());

            Debug.Log($"Получен ответ на локальный запрос: {response.Result}");
        }

        [Button("Вызвать тестовое ГЛОБАЛЬНОЕ событие")]
        private void TestGlobalEvent()
        {
            Entity.PublishGlobalEvent(BasicActionsTypes.Test_Event, new EventBase());
        }

        [Button("Вызвать тестовое ГЛОБАЛЬНОЕ событие с ответом")]
        private void TestGlobalResponceEvent()
        {
            ResponseEvent response = Entity.PublishGlobalEvent<EventBase, ResponseEvent>(BasicActionsTypes.Test_Event, new EventBase());
            Debug.Log($"Получен ответ на глобальный запрос: {response.Result}");
        }
    }
}
