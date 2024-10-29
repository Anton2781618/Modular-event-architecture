using System.Collections.Generic;
using UnityEngine;

public class RopeSpawner : MonoBehaviour
{
    [SerializeField] private RopeController ropePrefab;
    private List<RopeController> ropes = new List<RopeController>();

    [Space(10)]
    public Rigidbody FerstTarget;
    public Rigidbody LastTarget;
    
    public void Detach()
    {
        foreach (var item in ropes)
        {
            Destroy(item.gameObject);
        }

        ropes.Clear();

        // GlobalEventBus.Instance.Publish(GlobalEventBus.События.Веревка.Веревка_отсоеденена, new BaseEvent {Enabled = false});
    }

    public void Attach()
    {
        RopeController rope = Instantiate(ropePrefab, transform.position, Quaternion.identity);
        
        rope.Connect(FerstTarget, LastTarget);

        ropes.Add(rope);

        // GlobalEventBus.Instance.Publish(GlobalEventBus.События.Веревка.Веревка_присоеденена, new BaseEvent {Enabled = true});
        
    }
    public void Attach(Rigidbody fers, Rigidbody last)
    {
        RopeController rope = Instantiate(ropePrefab, transform.position, Quaternion.identity);
        
        rope.Connect2(fers, last);

        ropes.Add(rope);
    }
}
