using UnityEngine;
using ModularEventArchitecture;

namespace ModularEventArchitecture.Modules.Inventory
{
    public class InventoryModule : ModuleBase
    {
        [SerializeField] private int slots = 20;
        private string[] items;

        public override void Initialize()
        {
            items = new string[slots];
            Debug.Log($"InventoryModule initialized with {slots} slots");
        }

        public override void Dispose()
        {
            Debug.Log("InventoryModule disposed");
        }

        public bool AddItem(string itemId, int slot)
        {
            if (slot < 0 || slot >= slots || !string.IsNullOrEmpty(items[slot]))
                return false;

            items[slot] = itemId;
            Debug.Log($"Added item {itemId} to slot {slot}");
            return true;
        }

        public bool RemoveItem(int slot)
        {
            if (slot < 0 || slot >= slots || string.IsNullOrEmpty(items[slot]))
                return false;

            string itemId = items[slot];
            items[slot] = null;
            Debug.Log($"Removed item {itemId} from slot {slot}");
            return true;
        }

        public string GetItem(int slot)
        {
            if (slot < 0 || slot >= slots)
                return null;

            return items[slot];
        }
    }
}
