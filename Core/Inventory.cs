using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public int maxSize = 10;

    public bool AddItem(Item item)
    {
        if (items.Count < maxSize)
        {
            items.Add(item);
            return true;
        }
        return false;
    }

    public bool RemoveItem(Item item)
    {
        return items.Remove(item);
    }

    public void UseItem(Item item, Player player)
    {
        item.Use(player);
        RemoveItem(item);
    }
}
