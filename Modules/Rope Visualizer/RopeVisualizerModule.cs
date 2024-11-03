using System.Collections;
using MoreMountains.Tools;
using RopeToolkit;
using UnityEngine;

[CompatibleUnit(typeof(RopeController))]
public class RopeVisualizerModule : ModuleBase
{
    // [Header("Этот модуль занимается расчетом натяжения веревки, если натяжение слишком сильное то веревка порвется")]
    [MMInformation("Этот модуль занимается расчетом натяжения веревки, если натяжение слишком сильное то веревка порвется",MoreMountains.Tools.MMInformationAttribute.InformationType.Info,false)]

    [Tooltip("Этот партикл появится по центру веревки в момент ее разрыва")]
    [SerializeField] private GameObject particle;

    [Tooltip("Цвет веревки в нормальном состоянии")]
    [SerializeField] private Color _originalColor = Color.gray;
    [Tooltip("Цвет веревки при перенапряжении")]
    [SerializeField] private Color _overstretchedColor = Color.red;

    [Tooltip("Значение насколько меторов можно растянуть веревку, после порвется")]
    [SerializeField] private float _maxTension = 5f; // Максимальное напряжение веревки

    // [Tooltip("Сила, с которой веревка отлетает после того как оторвется")]
    // [SerializeField] private float _detachForce = 1f; // Сила, с которой веревка отрывается

    [Tooltip("Префаб объекта, который будет появляться при разрыве на концах оторваных веревок")]
    [SerializeField] private GameObject _splitObjectPrefab; // Префаб объекта, который будет появляться при разрыве
    
    [Tooltip("Скрипт с работой веревки")]
    [SerializeField] private Rope rope;
    
    [Tooltip("Скрипт управления работы веревки")]
    [SerializeField] private RopeController ropeController;

    private bool _isObjectsSpawned = false;
    private GameObject _splitObject1;
    private GameObject _splitObject2;
    private float _offset;

    //веревка под напряжением
    private bool _isRopeStretched = false;

    protected override void Initialize()
    {
        base.Initialize();
        
        rope = ropeController.GetRope();
    }

    public override void UpdateMe()
    {
        //находим коэффициент напряжения веревки учитывая стартовую длину
        float tensionFactor = Mathf.Clamp01((ropeController.CalculateRopeTension() - ropeController.GetStartLength()) / _maxTension);

        if(!_isRopeStretched && tensionFactor > 0.1f)
        {
            _isRopeStretched = true;

            LocalEvents.Publish(LocalEventBus.События.Команды.Веревка.Веревка_натянулась, new BaseEvent {Enabled = true});
        }
        else
        if(_isRopeStretched && tensionFactor < 0.1f)
        {
            _isRopeStretched = false;

            LocalEvents.Publish(LocalEventBus.События.Команды.Веревка.Веревка_раслабилась, new BaseEvent {Enabled = true});
        }

        rope.material.color = Color.Lerp(_originalColor, _overstretchedColor, tensionFactor);

        if (tensionFactor >= 1)
        {
            _isRopeStretched = false;

            LocalEvents.Publish(LocalEventBus.События.Команды.Веревка.Веревка_раслабилась, new BaseEvent {Enabled = true});

            AnimateGap();
        }

        if(ropeController.part != null)
        {
            if (ropeController.part.CheckSeparation())
            {
                RopeSpawnerModule ropeSpawner = ropeController.RopeSpawnerModule;

                ropeSpawner.Detach();
                
                _splitObject1 = Instantiate(ropeController.part.partPerfab, ropeController.part.transform.position, ropeController.part.transform.rotation);

                ropeSpawner.Attach(ropeSpawner.FerstTarget, _splitObject1.GetComponent<Rigidbody>());

            }
        }
    }

    //метод производит разрыв веревки
    private void AnimateGap()
    {
        RopeSpawnerModule ropeSpawner = ropeController.RopeSpawnerModule;

        ropeSpawner.Detach();

        if (_splitObjectPrefab == null) return;

        Vector3 centerPosition = GetRopeCenterPosition();
        
        _splitObject1 = Instantiate(_splitObjectPrefab, centerPosition, Quaternion.identity);
        _splitObject2 = Instantiate(_splitObjectPrefab, centerPosition, Quaternion.identity);
        
        _isObjectsSpawned = true;

        SetSplitObjectsPosition();

        GameObject newParticle = Instantiate(particle, centerPosition, Quaternion.identity);
        
        Destroy(newParticle, 5f);

        ropeSpawner.Attach(ropeSpawner.FerstTarget, _splitObject1.GetComponent<Rigidbody>());
        ropeSpawner.Attach(ropeSpawner.LastTarget, _splitObject2.GetComponent<Rigidbody>());

        Destroy(_splitObject1, 0.1f);
     
        Destroy(_splitObject2, 0.1f);
    }

    private void SetSplitObjectsPosition()
    {
        if (!_isObjectsSpawned) return;

        Vector3 centerPosition = GetRopeCenterPosition();
        Vector3 ropeDirection = rope.GetPositionAt(rope.measurements.particleCount - 1) - rope.GetPositionAt(0);

        _splitObject1.transform.position = centerPosition;
        // _splitObject1.GetComponent<Rigidbody>().AddForce(_detachForce * -ropeDirection, ForceMode.Impulse);
        
        _splitObject2.transform.position = centerPosition;
        // _splitObject2.GetComponent<Rigidbody>().AddForce(_detachForce * ropeDirection, ForceMode.Impulse);
    }

    private Vector3 GetRopeCenterPosition()
    {
        int centerParticleIndex = rope.measurements.particleCount / 2;
        return rope.GetPositionAt(centerParticleIndex);
    }
}