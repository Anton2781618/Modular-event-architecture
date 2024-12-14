using UnityEngine;
using ModularEventArchitecture;

namespace ModularEventArchitecture.Samples.Inventory
{
    public class InventoryModule : ModuleBase
    {
        [SerializeField] private int slots = 20;
        
        public override void Initialize()
        {
            Debug.Log("Inventory Module initialized with " + slots + " slots");
        }

        public override void Dispose()
        {
            Debug.Log("Inventory Module disposed");
        }

        // Пример метода для добавления предмета
        public bool AddItem(string itemId)
        {
            Debug.Log($"Adding item {itemId} to inventory");
            return true;
        }

        // Пример метода для удаления предмета
        public bool RemoveItem(string itemId)
        {
            Debug.Log($"Removing item {itemId} from inventory");
            return true;
        }
    }
}
