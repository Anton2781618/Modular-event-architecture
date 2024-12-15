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

        public override void UpdateMe()
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

        [Tools.Button("Вызвать тестовое событие")]
        private void TestEvent()
        {
            Entity.LocalEvents.Publish(LocalEventBus.События.Тестовое_событие, new EventBase());
        }
    }
}
