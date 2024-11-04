using UnityEngine;

namespace ModularEventArchitecture
{
    public abstract class Item : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
        public string description;

        public abstract void Use(Player player);
    }
}