using UnityEngine;

namespace ModularEventArchitecture
{
    public class TestModule : ModuleBase
    {
        public override void Initialize()
        {
            Entity.LocalEvents.Subscribe<EventBase>(BasicActionsTypes.SystemRequirements.Test_Event, TestLocal);

            Entity.Globalevents.Add((BasicActionsTypes.SystemRequirements.Test_Event, (data) => TestGlobal((EventBase)data)));
        }

        private void TestLocal(EventBase eventBase)
        {
            Debug.Log($"<color=green>Тестовое ЛОКАЛЬНОЕ событие</color> на объекте {Entity.name}");
        }
        private void TestGlobal(EventBase eventBase)
        {
            Debug.Log($"<color=red>Тестовое ГЛОБАЛЬНОЕ событие</color> на объекте {Entity.name}");
        }
    }
}