namespace ModularEventArchitecture
{
    [CompatibleUnit(EntityTag.UI)] 
    public class DebugModule : ModuleBase
    {
        public override void Initialize()
        {

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
            Entity.PublishLocalEvent(BasicActionsTypes.SystemRequirements.Test_Event, new EventBase());
        }
        [Button("Вызвать тестовое ГЛОБАЛЬНОЕ событие")]
        private void TestGlobalEvent()
        {
            Entity.PublishGlobalEvent(BasicActionsTypes.SystemRequirements.Test_Event, new EventBase());
        }
    }
}
