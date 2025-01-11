namespace ModularEventArchitecture
{
    public class GlobalEventBus : EventBus
    {
        //todo: надо продумать глубже....не нравится что использую синглтон
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
    }
}
