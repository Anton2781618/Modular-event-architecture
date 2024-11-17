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
            public const int Показать_Окно_Помощи = (int)ActionsType.ShowHelpWindow;
        }

        public static class Системные
        {
            public const int Перезапуск_Сцены = (int)ActionsType.System_RestartScene;
        }

        public static class Команды
        {
            public const int Показать_текст_в_точке = (int)ActionsType.ShowText_ToPoint;
            public const int Заспавнить_моба = (int)ActionsType.SpawnMob;
        }

        public static class Game2048
        {
            public const int Move = (int)ActionsType.Game2048_Move;
            public const int TileSpawned = (int)ActionsType.Game2048_TileSpawned;
            public const int TileMove = (int)ActionsType.Game2048_TileMove;
            public const int ScoreChanged = (int)ActionsType.Game2048_ScoreChanged;
            public const int GameOver = (int)ActionsType.Game2048_GameOver;
            public const int RestartGame = (int)ActionsType.Game2048_RestartGame;
        }
    }
}
