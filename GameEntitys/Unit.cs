using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Unit : GameEntity
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
