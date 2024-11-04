using UnityEngine;

namespace ModularEventArchitecture
{

    [RequireComponent(typeof(Animator))]
    public abstract class UnitEntity : GameEntity
    {
        public Inventory inventory;
        
        protected override void Initialize()
        {
            inventory = GetComponent<Inventory>();

            if (inventory == null)
            {
                inventory = gameObject.AddComponent<Inventory>();
            }
        }

    }
}