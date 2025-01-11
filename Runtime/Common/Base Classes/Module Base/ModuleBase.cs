using UnityEngine;

namespace ModularEventArchitecture
{
    public abstract class ModuleBase : MonoBehaviour
    {
        //todo: наддо продумать глубже....не нравится что свойство публичное....возможно стоит сделать защищеннымы
        [field: SerializeField, HideInInspector] public GameEntity Entity{get; set;}

        public void Setup(GameEntity character)
        {
            Entity = character;
        }

        public abstract void Initialize();

        //todo: наддо продумать глубже....не нравится что метод абстрактынй, возможно стоит сделать виртуальным 
        public abstract void UpdateMe();
    }
}