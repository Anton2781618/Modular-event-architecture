using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartsSettings : MonoBehaviour
{
    [Serializable]
    public class BodyPartsMeshes
    {
        //
        public GameObject Head;
        public GameObject Tors;
        public GameObject Hips;
        public GameObject[] LeftArm;
        public GameObject[] RightArm;
        public GameObject[] LeftLeg;
        public GameObject[] RightLeg;
    }

    [SerializeField] private BodyPartsMeshes _bodyPartsMeshes;
    [SerializeField] private HpPart[] _visualDemageComponents;
        
    

    [Space(10)]
    public GameObject HeadPrefab;
    public GameObject HipsPrefab;
    public GameObject LeftArmPrefab;
    public GameObject RightArmPrefab;
    public GameObject LeftLegPrefab;
    public GameObject RightLegPrefab;

    public enum BodyPart
    {
        Head, Tors, LeftArm, RightArm, LeftLeg, RightLeg, Hips
    }

    public void DestroyPart(BodyPart bodyPart, int index, Transform partTransform)
    {
        if (bodyPart == BodyPart.Head)
        {
            Instantiate(HeadPrefab, partTransform.position, partTransform.rotation);

            _bodyPartsMeshes.Head.SetActive(false);
        } 

        if (bodyPart == BodyPart.LeftArm) 
        {
            DestroyMassivePart(_bodyPartsMeshes.LeftArm, index);

            Instantiate(LeftArmPrefab, partTransform.position, partTransform.rotation);
        }

        if (bodyPart == BodyPart.RightArm) 
        {
            DestroyMassivePart(_bodyPartsMeshes.RightArm, index);

            Instantiate(RightArmPrefab, partTransform.position, partTransform.rotation);
        }

        if (bodyPart == BodyPart.LeftLeg) 
        {
            DestroyMassivePart(_bodyPartsMeshes.LeftLeg, index);

            Instantiate(LeftLegPrefab, partTransform.position, partTransform.rotation);
        }

        if (bodyPart == BodyPart.RightLeg) 
        {
            DestroyMassivePart(_bodyPartsMeshes.RightLeg, index);

            Instantiate(RightLegPrefab, partTransform.position, partTransform.rotation);
        }

        if (bodyPart == BodyPart.Hips)
        {
            _bodyPartsMeshes.Hips.SetActive(false);

            DestroyMassivePart(_bodyPartsMeshes.LeftLeg, 0);

            DestroyMassivePart(_bodyPartsMeshes.RightLeg, 0);

            Instantiate(HipsPrefab, partTransform.position, partTransform.rotation);
        } 
    }

    private void DestroyMassivePart(GameObject[] arr, int index)
    {
        for (int i = index; i < arr.Length; i++)
        {
            arr[i].SetActive(false);
        }
    }

    [ContextMenu("Найти все визуальные компоненты урона")]
    public void FindVisualDemageComponents() => _visualDemageComponents = GetComponentsInChildren<HpPart>();
}
