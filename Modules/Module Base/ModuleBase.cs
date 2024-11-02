using UnityEngine;

public abstract class ModuleBase : MonoEventBus, IModule
{

    [SerializeField, HideInInspector] public GameEntity Character{get; set;}

    public void SetCharacter(GameEntity character) => Character = character;

    protected override void Initialize()
    {
        // Debug.Log("ModuleBase Initialize " + transform.name);
        
        if (!Character) Character = GetComponent<GameEntity>();

        Character.AddModule(this);
    }

    [ContextMenu("Проверить")]
    private void CheckCgaracter()
    {
        Debug.Log(Character == null);
    }

    public abstract void UpdateMe();
}
