public class LocalEventBus : EventBus
{
    public static class События
    {
        public const int Получить_урон = (int)ActionsType.Take_Damage;
        public const int Юнит_погиб = (int)ActionsType.Unit_Die;

        public const int Оглушен = (int)ActionsType.Stunned;
        public const int Выносливость_изменилась = (int)ActionsType.Stamina_change;
        public const int Здоровье_изменилась = (int)ActionsType.Health_change;

        public static class Команды
        {
            public static class Движение
            {
                public const int Двигаться_к_точке = (int)ActionsType.Move_To_point;
                public const int Двигаться_к_таргету = (int)ActionsType.Move_To_target;
                public const int Остановиться = (int)ActionsType.Stop;
                public const int Начать_спринт = (int)ActionsType.StartSprint;
                public const int Закончить_спринт = (int)ActionsType.StopSprint;
            }

            public static class Веревка
            {
                public const int Веревка_натянулась = (int)ActionsType.Rope_Stretched;
                public const int Веревка_раслабилась = (int)ActionsType.Rope_Relaxed;
                public const int Веревка_присоеденена = (int)ActionsType.Rope_Connected;
                public const int Веревка_отсоеденена = (int)ActionsType.Rope_Disconnected;

            }
            public static class Бой
            {
                public const int Атакавать_цель = (int)ActionsType.Attack;
                public const int Перестать_сражаться = (int)ActionsType.Stop_Fight;
            }
            public static class UI
            {
                public const int Обновить_UI = (int)ActionsType.UpdateUI;
            }
        }

        public static class Состояния
        {
            public const int Усталость = (int)ActionsType.Tired;

            public static class Эффекты
            {
                public const int Эффект_создан = (int)ActionsType.EffectCreated;
                public const int Эффект_снять = (int)ActionsType.Effect_extend;
                
            }
        }
    }
}
