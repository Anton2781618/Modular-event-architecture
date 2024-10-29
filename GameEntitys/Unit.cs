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

    protected override void OnEnable()
    {
        base.OnEnable();

        GlobalEventBus.Instance.Publish(GlobalEventBus.События.Юнит_создан, new CreateUnitEvent {Unit = this});
    }

    protected override void OnDisable()
    {
        GlobalEventBus.Instance.Publish(GlobalEventBus.События.Юнит_погиб, new DieEvent { Unit = this });
    }
}
