using UnityEngine;

namespace ModularEventArchitecture
{
    public abstract class ModuleBase : MonoEventBus, IModule
    {

        [SerializeField, HideInInspector] public GameEntity Entity{get; set;}

        public void SetCharacter(GameEntity character) => Entity = character;

        protected override void Initialize()
        {
            // Debug.Log("ModuleBase Initialize " + transform.name);
            
            if (!Entity) Entity = GetComponent<GameEntity>();

            Entity.AddModule(this);
        }

        public abstract void UpdateMe();
    }
}