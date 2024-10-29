using UnityEngine;

[CreateAssetMenu(fileName = "New Health Potion", menuName = "Inventory/Health Potion")]
public class HealthPotion : Item
{
    public int healAmount;

    public override void Use(Player player)
    {
        // player.Heal(healAmount);
        Debug.Log($"Player used {itemName} and healed for {healAmount}.");
    }
}
