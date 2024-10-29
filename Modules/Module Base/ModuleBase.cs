using UnityEngine;

public abstract class ModuleBase : MonoEventBus, IModule
{
    public GameEntity Character {get; set;}

    protected override void Initialize()
    {
        Debug.Log("ModuleBase Initialize " + transform.name);
        if (!Character) Character = GetComponent<GameEntity>();

        Character.AddModule(this);
    }

    public abstract void UpdateMe();
}
