using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularEventArchitecture
{
    public class TestModule : ModuleBase
    {
        public override void Initialize()
        {
            Entity.LocalEvents.Subscribe<EventBase>(LocalEventBus.События.Тестовое_событие, Test);
        }

        public override void UpdateMe()
        {
            
        }

        private void Test(EventBase eventBase)
        {
            Debug.Log($"<color=green>Тестовое событие</color> на объекте {Entity.name}");
        }
    }
}
