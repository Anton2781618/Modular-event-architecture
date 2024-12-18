using UnityEngine;

namespace ModularEventArchitecture
{
    public abstract class ModuleBase : MonoBehaviour
    {
        [field: SerializeField, HideInInspector] public GameEntity Entity{get; set;}

        public void Setup(GameEntity character)
        {
            Entity = character;
        }

        public abstract void Initialize();

        public abstract void UpdateMe();
    }
}