using UnityEngine;
using ModularEventArchitecture;

namespace ModularEventArchitecture.Modules.Inventory
{
    public class InventoryManager : ManagerEntity
    {
        protected override void OnInitialize()
        {
            Debug.Log("InventoryManager initialized");
        }

        protected override void OnDispose()
        {
            Debug.Log("InventoryManager disposed");
        }
    }
}
