using UnityEngine;
using static BodyPartsSettings;

[CompatibleUnit(typeof(Unit))]
public class BodyPartsModule : ModuleBase
{
    [Tools.Information("Этот модуль представляет из себя систему частей тела персонажа, Модуль отвечает за создание и уничтожение частей тела персонажа", Tools.InformationAttribute.InformationType.Info, false)]

    [SerializeField] private BodyPartsSettings _bodyPartsSettings;
    [SerializeField] private Animator _animator;

    protected override void Initialize()
    {
        base.Initialize();

        if (!_animator) _animator = GetComponent<Animator>();
    }

    public override void UpdateMe()
    {
        
    }

    public void DestroyPart(BodyPart bodyPart, int index, Transform partTransform)
    {
        if (bodyPart == BodyPart.Head)
        {
            Instantiate(_bodyPartsSettings.HeadPrefab, partTransform.position, partTransform.rotation);

            _bodyPartsSettings._bodyPartsMeshes.Head.SetActive(false);
        } 

        if (bodyPart == BodyPart.LeftArm) 
        {
            DestroyMassivePart(_bodyPartsSettings._bodyPartsMeshes.LeftArm, index);

            Instantiate(_bodyPartsSettings.LeftArmPrefab, partTransform.position, partTransform.rotation);
        }

        if (bodyPart == BodyPart.RightArm) 
        {
            DestroyMassivePart(_bodyPartsSettings._bodyPartsMeshes.RightArm, index);

            Instantiate(_bodyPartsSettings.RightArmPrefab, partTransform.position, partTransform.rotation);
        }

        if (bodyPart == BodyPart.LeftLeg) 
        {
            DestroyMassivePart(_bodyPartsSettings._bodyPartsMeshes.LeftLeg, index);

            Instantiate(_bodyPartsSettings.LeftLegPrefab, partTransform.position, partTransform.rotation);
        }

        if (bodyPart == BodyPart.RightLeg) 
        {
            DestroyMassivePart(_bodyPartsSettings._bodyPartsMeshes.RightLeg, index);

            Instantiate(_bodyPartsSettings.RightLegPrefab, partTransform.position, partTransform.rotation);
        }

        if (bodyPart == BodyPart.Hips)
        {
            _bodyPartsSettings._bodyPartsMeshes.Hips.SetActive(false);

            DestroyMassivePart(_bodyPartsSettings._bodyPartsMeshes.LeftLeg, 0);

            DestroyMassivePart(_bodyPartsSettings._bodyPartsMeshes.RightLeg, 0);

            Instantiate(_bodyPartsSettings.HipsPrefab, partTransform.position, partTransform.rotation);
        } 
    }

    private void DestroyMassivePart(GameObject[] arr, int index)
    {
        for (int i = index; i < arr.Length; i++)
        {
            arr[i].SetActive(false);
        }
    }

    [Tools.Button("Найти все BodyPart на теле персонажа")]
    public void FindVisualDemageComponents() => _bodyPartsSettings._visualDemageComponents = GetComponentsInChildren<HpPart>();

    [Tools.Button("Найти все места для BodyPart на теле персонажа")]
    public void FindBodyPartsPlaces()
    {
        if (!_animator) _animator = GetComponent<Animator>();

        _bodyPartsSettings._visualDemagePlace = new Transform[]
        {
            _animator.GetBoneTransform(HumanBodyBones.Head),
            _animator.GetBoneTransform(HumanBodyBones.Chest),

            _animator.GetBoneTransform(HumanBodyBones.LeftUpperArm),
            _animator.GetBoneTransform(HumanBodyBones.LeftLowerArm),
            _animator.GetBoneTransform(HumanBodyBones.LeftHand),

            _animator.GetBoneTransform(HumanBodyBones.RightUpperArm),
            _animator.GetBoneTransform(HumanBodyBones.RightLowerArm),
            _animator.GetBoneTransform(HumanBodyBones.RightHand),
            
            _animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg),
            _animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg),
            _animator.GetBoneTransform(HumanBodyBones.LeftFoot),

            _animator.GetBoneTransform(HumanBodyBones.RightUpperLeg),
            _animator.GetBoneTransform(HumanBodyBones.RightLowerLeg),
            _animator.GetBoneTransform(HumanBodyBones.RightFoot),

        };

        //
        foreach (var place in _bodyPartsSettings._visualDemagePlace)
        {
            if (place.GetComponent<HpPart>() == null) place.gameObject.AddComponent<HpPart>();
        }
    
    }
}
