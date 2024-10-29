using System;

public interface IModule  
{
    public GameEntity Character {get; }
    public void SetLocalEventBus(LocalEventBus localEventBus);
    public void UpdateMe();
}