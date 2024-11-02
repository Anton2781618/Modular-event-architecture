public class GlobalEventBus : EventBus
{
    private static GlobalEventBus instance;
    public static GlobalEventBus Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GlobalEventBus();
            }

            return instance;
        }
    }  

    public static class События
    {
        public const int Юнит_погиб = (int)ActionsType.Unit_Die;
        public const int Юнит_создан = (int)ActionsType.Unit_Created;
        
        public static class UI
        {
            public const int UI_Состояние_Изменилось = (int)ActionsType.IsUIOpen;

        }
    }
    public static class Команды
    {
        public const int Показать_текст_в_точке = (int)ActionsType.ShowText_ToPoint;
        
    }

}
