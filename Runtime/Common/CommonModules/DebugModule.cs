using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularEventArchitecture
{
    public class DebugModule : ModuleBase
    {
        public override void Initialize()
        {

        }

        [Tools.Button("Вывести в консоль события LocalEventBus")]
        private void DebugLocalEventBus()
        {
            Entity.LocalEvents.ShowAllEvents();
        }

        [Tools.Button("Вывести в консоль события GlobalEventBus")]
        private void DebugGlobalEventBus()
        {
            GlobalEventBus.Instance.ShowAllEvents();
        }

        [Tools.Button("Вызвать тестовое ЛОКАЛЬНОЕ событие")]
        private void TestLocalEvent()
        {
            Entity.LocalEvents.Publish(BasicActionsTypes.SystemRequirements.Test_Event, new EventBase());
        }
        [Tools.Button("Вызвать тестовое ГЛОБАЛЬНОЕ событие")]
        private void TestGlobalEvent()
        {
            GlobalEventBus.Instance.Publish(BasicActionsTypes.SystemRequirements.Test_Event, new EventBase());
        }
    }
}
