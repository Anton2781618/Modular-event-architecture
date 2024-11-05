using System;
using System.Collections.Generic;
using UnityEngine;

namespace ModularEventArchitecture
{
    [Serializable]
    public class BodyPartsSettings
    {
        [Tooltip("Массивы визуальных компонентов, которые будут отключаться при уроне")]
        public BodyPartsMeshes _bodyPartsMeshes;

        [Tooltip("Массив HpPart, они передают данные о столкновении и уроне")]
        public HpPart[] VisualDemageComponents;

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
        

        [Space(10)]
        [Header("Эти префабы создаются при уничтожении части тела")]
        [Space(5)]
        public GameObject HeadPrefab;
        public GameObject HipsPrefab;
        public GameObject LeftArmPrefab;
        public GameObject RightArmPrefab;
        public GameObject LeftLegPrefab;
        public GameObject RightLegPrefab;
        
        [Space(10)]
        [Header("Эти префабы крови, для инициализации частей тела")]
        [Space(5)]
        public GameObject GroundBlood;
        public GameObject[] BloodFX;

        [Space(10)]
        [Header("Маска столкновений")]
        [Space(5)]
        public LayerMask TriggerMask;
    }
}