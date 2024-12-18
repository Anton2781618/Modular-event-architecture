namespace ModularEventArchitecture
{
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
    }
}
