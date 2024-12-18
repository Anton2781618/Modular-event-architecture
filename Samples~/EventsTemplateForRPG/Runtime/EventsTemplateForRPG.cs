namespace ModularEventArchitecture
{
    public class EventsTemplateForRPG : IEventType
    {
        private ActionsRPGEnum _type;

        private EventsTemplateForRPG(ActionsRPGEnum type)
        {
            _type = type;
        }

        public int GetEventId() => (int)_type;
        public string GetEventName() => _type.ToString();

        public static EventsTemplateForRPG Move_To_point => new EventsTemplateForRPG(ActionsRPGEnum.Move_To_point);
        public static EventsTemplateForRPG Move_To_target => new EventsTemplateForRPG(ActionsRPGEnum.Move_To_target);
        public static EventsTemplateForRPG Stop => new EventsTemplateForRPG(ActionsRPGEnum.Stop);
        public static EventsTemplateForRPG Attack => new EventsTemplateForRPG(ActionsRPGEnum.Attack);
        public static EventsTemplateForRPG Stop_Fight => new EventsTemplateForRPG(ActionsRPGEnum.Stop_Fight);
        public static EventsTemplateForRPG Take_Damage => new EventsTemplateForRPG(ActionsRPGEnum.Take_Damage);
        public static EventsTemplateForRPG Unit_Die => new EventsTemplateForRPG(ActionsRPGEnum.Unit_Die);
        public static EventsTemplateForRPG Unit_Created => new EventsTemplateForRPG(ActionsRPGEnum.Unit_Created);
        public static EventsTemplateForRPG ShowText_ToPoint => new EventsTemplateForRPG(ActionsRPGEnum.ShowText_ToPoint);
        public static EventsTemplateForRPG Rope_Stretched => new EventsTemplateForRPG(ActionsRPGEnum.Rope_Stretched);
        public static EventsTemplateForRPG IsUIOpen => new EventsTemplateForRPG(ActionsRPGEnum.IsUIOpen);
        public static EventsTemplateForRPG StartSprint => new EventsTemplateForRPG(ActionsRPGEnum.StartSprint);
        public static EventsTemplateForRPG StopSprint => new EventsTemplateForRPG(ActionsRPGEnum.StopSprint);
        public static EventsTemplateForRPG Stamina_change => new EventsTemplateForRPG(ActionsRPGEnum.Stamina_change);
        public static EventsTemplateForRPG UpdateUI => new EventsTemplateForRPG(ActionsRPGEnum.UpdateUI);
        public static EventsTemplateForRPG Health_change => new EventsTemplateForRPG(ActionsRPGEnum.Health_change);
        public static EventsTemplateForRPG Stunned => new EventsTemplateForRPG(ActionsRPGEnum.Stunned);
        public static EventsTemplateForRPG Effect_extend => new EventsTemplateForRPG(ActionsRPGEnum.Effect_extend);
        public static EventsTemplateForRPG EffectCreated => new EventsTemplateForRPG(ActionsRPGEnum.EffectCreated);
        public static EventsTemplateForRPG Rope_Connected => new EventsTemplateForRPG(ActionsRPGEnum.Rope_Connected);
        public static EventsTemplateForRPG Rope_Disconnected => new EventsTemplateForRPG(ActionsRPGEnum.Rope_Disconnected);
        public static EventsTemplateForRPG Rope_Relaxed => new EventsTemplateForRPG(ActionsRPGEnum.Rope_Relaxed);
        public static EventsTemplateForRPG Tired => new EventsTemplateForRPG(ActionsRPGEnum.Tired);
        public static EventsTemplateForRPG System_RestartScene => new EventsTemplateForRPG(ActionsRPGEnum.System_RestartScene);
        public static EventsTemplateForRPG SpawnMob => new EventsTemplateForRPG(ActionsRPGEnum.SpawnMob);
        public static EventsTemplateForRPG ShowHelpWindow => new EventsTemplateForRPG(ActionsRPGEnum.ShowHelpWindow);

        public enum ActionsRPGEnum
        {
            Move_To_point,
            Move_To_target,
            Stop,
            Attack,
            Stop_Fight,
            Take_Damage,
            Unit_Die,
            Unit_Created,
            ShowText_ToPoint,
            Rope_Stretched,
            IsUIOpen,
            StartSprint,
            StopSprint,
            Stamina_change,
            UpdateUI,
            Health_change,
            Stunned,
            Effect_extend,
            EffectCreated,
            Rope_Connected,
            Rope_Disconnected,
            Rope_Relaxed,
            Tired,
            System_RestartScene,
            SpawnMob,
            ShowHelpWindow,
        }
    }
}
