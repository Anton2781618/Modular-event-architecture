using System;

namespace ModularEventArchitecture
{
    public interface IModule  
    {
        public GameEntity Entity {get; }
        public void SetLocalEventBus(LocalEventBus localEventBus);
        public void UpdateMe();
    }
}