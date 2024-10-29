using System;
using UnityEngine;

[Serializable]
public class BodyPartsSettings
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
    
    [Tooltip("Массивы визуальных компонентов, которые будут отключаться при уроне")]
    public BodyPartsMeshes _bodyPartsMeshes;

    [Tooltip("Массив HpPart, они передают данные о столкновении и уроне")]
    public HpPart[] _visualDemageComponents;
    public Transform[] _visualDemagePlace;
        
    

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
}
