using System.Collections.Generic;
using UnityEngine;

[CompatibleUnit(typeof(UnitEntity))]
public class RopeSpawnerModule : ModuleBase
{
    [SerializeField] private RopeController ropePrefab;
    private List<RopeController> ropes = new List<RopeController>();

    public Rigidbody FerstTarget;
    public Rigidbody LastTarget;

    public LayerMask HitLayers = default;
    private RaycastHit hit = new RaycastHit();
    private Camera cameraMain;

    protected override void Initialize()
    {
        base.Initialize();

        cameraMain = Camera.main;

    }
    public override void UpdateMe()
    {
        if(Input.GetKeyUp(KeyCode.E))
        {
            if (OnHit() && Vector3.Distance(transform.position, hit.transform.position ) < 3)
            {
                LastTarget = hit.collider.GetComponent<Rigidbody>();

                Attach();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Detach();
        }
    }
    
    public void Attach()
    {
        RopeController rope = Instantiate(ropePrefab, transform.position, Quaternion.identity);

        rope.RopeSpawnerModule = this;
        
        rope.Connect(FerstTarget, LastTarget);

        ropes.Add(rope);

        LocalEvents.Publish(LocalEventBus.События.Команды.Веревка.Веревка_присоеденена, new BaseEvent {Enabled = true});
        
    }
    
    public void Detach()
    {
        foreach (var item in ropes)
        {
            Destroy(item.gameObject);
        }

        ropes.Clear();

        LocalEvents.Publish(LocalEventBus.События.Команды.Веревка.Веревка_отсоеденена, new BaseEvent {Enabled = false});
    }


    public void Attach(Rigidbody fers, Rigidbody last)
    {
        RopeController rope = Instantiate(ropePrefab, transform.position, Quaternion.identity);

        rope.RopeSpawnerModule = this;
        
        rope.Connect2(fers, last);

        ropes.Add(rope);
    }

    private bool OnHit()
    {
        Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 50f, HitLayers))
        {
            return true;
        }

        return false;
    }
}
