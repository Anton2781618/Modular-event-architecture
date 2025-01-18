namespace ModularEventArchitecture
{
    public class BasicActionsTypes : IEventType
    {
        private ActionsTypeEnum _type;

        private BasicActionsTypes(ActionsTypeEnum type)
        {
            _type = type;
        }

        public int GetEventId() => (int)_type;
        public string GetEventName() => _type.ToString();
        
        public static class UI
        {
            public static readonly IEventType Is_UI_Open = new BasicActionsTypes(ActionsTypeEnum.IsUIOpen);
            public static readonly IEventType Show_Help_Window = new BasicActionsTypes(ActionsTypeEnum.ShowHelpWindow);
            public static readonly IEventType Update_UI = new BasicActionsTypes(ActionsTypeEnum.UpdateUI);
        }

        public static class SystemRequirements
        {
            public static readonly IEventType System_RestartScene = new BasicActionsTypes(ActionsTypeEnum.System_RestartScene);
        }
        public static class Commands
        {
            public static readonly IEventType Spawn_Mob = new BasicActionsTypes(ActionsTypeEnum.SpawnMob);
            public static readonly IEventType Unit_Die = new BasicActionsTypes(ActionsTypeEnum.Unit_Die);
            public static readonly IEventType Unit_Created = new BasicActionsTypes(ActionsTypeEnum.Unit_Created);
        }

        public static IEventType Test_Event => new BasicActionsTypes(ActionsTypeEnum.TestEvent);
        

        private enum ActionsTypeEnum
        {
            Unit_Die,
            Unit_Created,
            TestEvent,
            IsUIOpen,
            ShowHelpWindow,
            System_RestartScene,
            SpawnMob,
            UpdateUI,
        }
    }
}
