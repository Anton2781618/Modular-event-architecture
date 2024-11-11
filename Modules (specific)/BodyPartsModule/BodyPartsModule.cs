using UnityEngine;

namespace ModularEventArchitecture
{
    using static BodyPartsSettings;
    [CompatibleUnit(typeof(UnitEntity))]
    public class BodyPartsModule : ModuleBase
    {
        [Tools.Information("Этот модуль представляет из себя систему частей тела персонажа, он отвечает за создание и уничтожение частей тела", Tools.InformationAttribute.InformationType.Info, false)]

        public BodyPartsSettings BodyPartsSettings;
        private Animator _animator;

        protected override void Initialize()
        {
            base.Initialize();

            if (!_animator) _animator = Entity.GetCachedComponent<Animator>();
        }


        public override void UpdateMe()
        {
        }


        public void DestroyPart(HumanBodyBones bodyPart, int index, Transform partTransform, bool isCreteCut = true)
        {
            if (bodyPart == HumanBodyBones.Head)
            {
                if (isCreteCut) Instantiate(BodyPartsSettings.HeadPrefab, partTransform.position, partTransform.rotation);

                BodyPartsSettings._bodyPartsMeshes.Head.SetActive(false);
            } 

            if (bodyPart == HumanBodyBones.LeftUpperArm) 
            {
                DestroyMassivePart(BodyPartsSettings._bodyPartsMeshes.LeftArm, index);

                if (isCreteCut) Instantiate(BodyPartsSettings.LeftArmPrefab, partTransform.position, partTransform.rotation);
            }

            if (bodyPart == HumanBodyBones.RightUpperArm) 
            {
                DestroyMassivePart(BodyPartsSettings._bodyPartsMeshes.RightArm, index);

                if (isCreteCut) Instantiate(BodyPartsSettings.RightArmPrefab, partTransform.position, partTransform.rotation);
            }

            if (bodyPart == HumanBodyBones.LeftLowerLeg) 
            {
                DestroyMassivePart(BodyPartsSettings._bodyPartsMeshes.LeftLeg, index);

                if (isCreteCut) Instantiate(BodyPartsSettings.LeftLegPrefab, partTransform.position, partTransform.rotation);
            }

            if (bodyPart == HumanBodyBones.RightLowerLeg) 
            {
                DestroyMassivePart(BodyPartsSettings._bodyPartsMeshes.RightLeg, index);

                if (isCreteCut) Instantiate(BodyPartsSettings.RightLegPrefab, partTransform.position, partTransform.rotation);
            }

            if (bodyPart == HumanBodyBones.Hips)
            {
                BodyPartsSettings._bodyPartsMeshes.Hips.SetActive(false);

                DestroyMassivePart(BodyPartsSettings._bodyPartsMeshes.LeftLeg, 0);

                DestroyMassivePart(BodyPartsSettings._bodyPartsMeshes.RightLeg, 0);

                if (isCreteCut) Instantiate(BodyPartsSettings.HipsPrefab, partTransform.position, partTransform.rotation);
            } 
        }

        private void DestroyMassivePart(GameObject[] arr, int index)
        {
            for (int i = index; i < arr.Length; i++)
            {
                arr[i].SetActive(false);
            }
        }

        [Tools.Button("Удалить компоненты HpPart")]
        private void DeletVisualDemages()
        {
            foreach (HpPart part in BodyPartsSettings.VisualDemageComponents)
            {
                //вызвать remove component
                DestroyImmediate(part);
                
        
            }

            BodyPartsSettings.VisualDemageComponents = null;
        }

        [Tools.Button("Установить BodyPart на все части тела персонажа")]
        private void FindBodyPartsPlaces()
        {
            if (!_animator) _animator = GetComponent<Animator>();

            SetVisualDemageOnPlace(HumanBodyBones.Head);
            SetVisualDemageOnPlace(HumanBodyBones.Chest);

            SetVisualDemageOnPlace(HumanBodyBones.LeftUpperArm);
            SetVisualDemageOnPlace(HumanBodyBones.LeftLowerArm);

            SetVisualDemageOnPlace(HumanBodyBones.RightUpperArm);
            SetVisualDemageOnPlace(HumanBodyBones.RightLowerArm);
            
            SetVisualDemageOnPlace(HumanBodyBones.LeftUpperLeg);
            SetVisualDemageOnPlace(HumanBodyBones.LeftLowerLeg);

            SetVisualDemageOnPlace(HumanBodyBones.RightUpperLeg);
            SetVisualDemageOnPlace(HumanBodyBones.RightLowerLeg);

            BodyPartsSettings.VisualDemageComponents = GetComponentsInChildren<HpPart>();
        }

        // Устанавливаем HpPart на место части тела
        private void SetVisualDemageOnPlace(HumanBodyBones partType)
        {
            Transform place = _animator.GetBoneTransform(partType);
            
            HpPart part = place.GetComponent<HpPart>();

            if (part == null) part = place.gameObject.AddComponent<HpPart>();
            
            part.Setup(partType, this);
        }
    }
}