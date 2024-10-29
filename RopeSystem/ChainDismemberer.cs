using System.Collections;
using System.Collections.Generic;
using RopeToolkit;
using UnityEngine;

public class ChainDismemberer : MonoBehaviour
{
    public CharacterJoint characterJoint;
    public HpPart hpPart; 
    public GameObject ropeObject;

    public int DismembererForce = 2500;

    private void Update() 
    {
        Debug.Log($"{characterJoint.currentForce.magnitude}");
        
        // if(characterJoint.currentForce.x > DismembererForce || characterJoint.currentForce.y > DismembererForce || characterJoint.currentForce.z > DismembererForce)
        if(characterJoint.currentForce.magnitude > DismembererForce)
        {
            hpPart.CreateBloodSplashes(hpPart.transform.position);
            hpPart.DestroyPart();
            Destroy(ropeObject);
        }
    }
}
