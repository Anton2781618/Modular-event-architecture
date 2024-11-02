using UnityEngine;
// using Unity.Mathematics;

public class HpPart : MonoBehaviour
{
    public HumanBodyBones BodyPart;
    [SerializeField] private BodyPartsModule BodyPartsModule;
    [Space(10)]
    
    public int Half = 0; //номер части тела
    public int countBloodCopy = 5; //сколько можно создать копий крови
    [SerializeField] private int separationLimit = 15;
    [SerializeField] private int damageLimit = 4;
    [Space(10)]
    
    private Rigidbody rb;
    [SerializeField] private CharacterJoint characterJoint;
    [SerializeField] private bool Destroyable = true;
    private bool isDestroyed = false;
    [SerializeField] private GameObject GroundBlood;
    
    [Header("сюда ставится проектор крови, каждый проектор уникален так что ставить нужно вручную пока что")]
    [SerializeField] private GameObject BodyBlood;
    public GameObject[] BloodFX;


    [SerializeField] private LayerMask triggerMask;

    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        characterJoint = GetComponent<CharacterJoint>();
    }

    public void Setup(HumanBodyBones partType, BodyPartsModule modeule)
    {
        BodyPart = partType;

        BodyPartsModule = modeule;

        GroundBlood = BodyPartsModule.BodyPartsSettings.GroundBlood;


        BloodFX = BodyPartsModule.BodyPartsSettings.BloodFX;

        triggerMask = BodyPartsModule.BodyPartsSettings.TriggerMask;
    }
    
    private void OnCollisionEnter(Collision other) 
    {
        if ((triggerMask.value & (1 << other.gameObject.layer)) == 0) return;
        
        CheckDamage(other);
    }

    public GameObject HeadPrefab;

    [ContextMenu("проверить натяжение в суставе")]
    public bool CheckSeparation()
    {
        // Debug.Log($"Натяжение в суставе {BodyPart}: {characterJoint.currentForce.magnitude}");
        if (characterJoint.currentForce.magnitude > 10000)
        {
            CreateBloodSplashes(this.transform.position);

            if (!isDestroyed && Destroyable)
            {
                DestroyPart(false);

                isDestroyed = true;
            }

            return true;
        }
     
        return false;
    }

    //проверить возможность повреждения конечности
    private void CheckDamage(Collision other)
    {
         if(rb.velocity.magnitude > damageLimit)
        {
            Vector3 point =  other.collider.ClosestPoint(transform.position);

            if(GetNearestObject(other.transform.root, point) != null)
            {
                if(GroundBlood) CreateBloodSticker(other, point, false);
                
                if(!isDestroyed && BodyBlood && countBloodCopy > 0) 
                {
                    CreateBloodSticker(other, point, true);

                    countBloodCopy --;
                }
            }
            
            if(rb.velocity.magnitude > separationLimit)
            {
                CreateBloodSplashes(point);

                if (!isDestroyed && Destroyable)
                {
                    DestroyPart();

                    isDestroyed = true;

                }
            } 
        }
    }

    

    [ContextMenu("DestroyPart")]
    public void DestroyPart(bool createnew = true) => BodyPartsModule.DestroyPart(BodyPart, Half, transform, createnew);

    //создать брызги крови
    public void CreateBloodSplashes(Vector3 point) => Instantiate(BloodFX[UnityEngine.Random.Range(0, BloodFX.Length)], point, Quaternion.LookRotation(point - transform.position));

    //создает наклейку крови
    private void CreateBloodSticker(Collision other, Vector3 point, bool yourself)
    {
        // var attachBloodInstance = Instantiate(BloodAttach);
        var attachBloodInstance = Instantiate(yourself ? BodyBlood : GroundBlood);
        
        var bloodT = attachBloodInstance.transform;

        /* if(!yourself) */ bloodT.position = yourself ? transform.position : other.collider.ClosestPoint(transform.position);

        bloodT.localRotation = Quaternion.identity;
        
        bloodT.localScale = Vector3.one * UnityEngine.Random.Range(0.8f, 1.4f);
        
        bloodT.LookAt(point, Vector3.up);
        
        bloodT.Rotate(0, UnityEngine.Random.Range(0, 360), 0);
        
        bloodT.transform.parent = yourself ? transform : other.transform;
        
        // Destroy(attachBloodInstance, 30);
    }
    
    private Transform GetNearestObject(Transform hit, Vector3 hitPos)
    {
        var closestPos = 100f;
        Transform closestBone = null;
        var childs = hit.GetComponentsInChildren<Transform>();

        foreach (var child in childs)
        {
            var dist = Vector3.Distance(child.position, hitPos);
            if (dist < closestPos)
            {
                closestPos = dist;
                closestBone = child;
            }
        }

        var distRoot = Vector3.Distance(hit.position, hitPos);
        if (distRoot < closestPos)
        {
            closestPos = distRoot;
            closestBone = hit;
        }
        return closestBone;
    }
}
