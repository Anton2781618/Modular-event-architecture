using System;

namespace ModularEventArchitecture
{
    public class BasicActionsTypes : IEventType
    {
        public int Id { get; }
        public string EventName {get;}

        private BasicActionsTypes(string eventName)
        {
             EventName = eventName;
            // Получаем хеш-код имени события, который будет уникален
            // Добавляем префикс чтобы еще больше избежать коллизий
            Id = ("BasicActionsTypes_" + eventName).GetHashCode();
        }

        public static class UI
        {
            public static readonly IEventType Is_UI_Open = new BasicActionsTypes("IsUIOpen");
            public static readonly IEventType Show_Help_Window = new BasicActionsTypes("ShowHelpWindow");
        }

        public static class SystemRequirements
        {
            public static readonly IEventType System_Restart_Scene = new BasicActionsTypes("System_RestartScene");
            public static readonly IEventType Test_Event = new BasicActionsTypes("TestEvent");
        }
        public static class Commands
        {
            public static readonly IEventType Unit_Die = new BasicActionsTypes("Unit_Die");
            public static readonly IEventType Unit_Created = new BasicActionsTypes("Unit_Created");
        }
    }

    [Serializable]
    public struct DieEvent : IEventData
    {    
        public GameEntity Unit;
    }

    [Serializable]
    public struct CreateUnitEvent : IEventData
    {
        public GameEntity Unit;
    }
}