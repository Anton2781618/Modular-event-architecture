using UnityEngine;
using DynamicMeshCutter;
using static BodyPartsSettings;
using System;
// using Unity.Mathematics;

public class HpPart : MonoBehaviour
{
    public BodyPart BodyPart;
    [SerializeField] private BodyPartsModule BodyPartsModule;
    [Space(10)]
    
    public int Half = 0; //номер части тела
    public int countBloodCopy = 5; //сколько можно создать копий
    [SerializeField] private int separationLimit = 15;
    [SerializeField] private int damageLimit = 4;
    [Space(10)]
    
    private Rigidbody rb;
    [SerializeField] private bool Destroyable = true;
    private bool isDestroyed = false;
    [SerializeField] private GameObject GroundBlood;
    [SerializeField] private GameObject BodyBlood;
    public GameObject[] BloodFX;


    [SerializeField] private LayerMask triggerMask;

    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void OnCollisionEnter(Collision other) 
    {
        if ((triggerMask.value & (1 << other.gameObject.layer)) == 0) return;
        
        if(rb.velocity.x > damageLimit || rb.velocity.y > damageLimit || rb.velocity.z > damageLimit)
        {
            Vector3 point =  other.collider.ClosestPoint(transform.position);

            if(GetNearestObject(other.transform.root, point) != null)
            {
                Debug.Log("CreateBloodSticker " + other.relativeVelocity + " ! " + other.transform.name);
                
                if(GroundBlood) CreateBloodSticker(other, point, false);
                
                if(!isDestroyed && BodyBlood && countBloodCopy > 0) 
                {
                    CreateBloodSticker(other, point, true);

                    countBloodCopy --;
                }
            }
            
            if(rb.velocity.x > separationLimit || rb.velocity.y > separationLimit || rb.velocity.z > separationLimit)
            {
                Debug.Log("DestroyPart " + transform.name);
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
    public void DestroyPart()
    {

        BodyPartsModule.DestroyPart(BodyPart, Half, transform);
    }

    public void CreateBloodSplashes(Vector3 point)
    {
        Instantiate(BloodFX[UnityEngine.Random.Range(0, BloodFX.Length)] , point, Quaternion.LookRotation(point - transform.position));
    }
    
    //создать брызги крови
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