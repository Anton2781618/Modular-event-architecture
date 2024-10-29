using RopeToolkit;
using UnityEngine;

public class RopeController : GameEntity
{
    public RopeSpawnerModule RopeSpawnerModule { get; set; }
    [SerializeField] private Rope rope;
    [SerializeField] private RopeConnection firstConnection;
    [SerializeField] private RopeConnection lastConnection;

    [SerializeField] private float targetRopeLength = 2f; // Целевая длина веревки в метрах
    
    private float initialRopeLength;
    private Material ropeMaterial;
    [SerializeField] Color originalColor;


    protected override void Initialize()
    {
        
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        GlobalEventBus.Instance.Publish(GlobalEventBus.События.Юнит_создан, new CreateUnitEvent {Unit = this});
    }

    protected override void OnDisable()
    {
        GlobalEventBus.Instance.Publish(GlobalEventBus.События.Юнит_погиб, new DieEvent { Unit = this });
    }

    public Rope GetRope() => rope;

    // Подключить веревку и растянуть до заданной длины 
    public void Connect(Rigidbody firstRigidbody, Rigidbody lastRigidbody)
    {
        var start = firstRigidbody.transform.TransformPoint(Vector3.zero);
        var end = lastRigidbody.transform.TransformPoint(Vector3.zero);

        rope.spawnPoints.Clear(); // Clear existing spawn points
        rope.spawnPoints.Add(transform.InverseTransformPoint(start));
        rope.spawnPoints.Add(transform.InverseTransformPoint(end));

        firstConnection.rigidbodySettings.body = firstRigidbody;
        firstConnection.transformSettings.transform = firstRigidbody.transform;
        
        lastConnection.rigidbodySettings.body = lastRigidbody;
        lastConnection.transformSettings.transform = lastRigidbody.transform;

        // Принудительно обновите конфигурацию троса
        rope.ResetToSpawnCurve();

        // Рассчитайте начальную длину веревки
        initialRopeLength = rope.GetCurrentLength();

        // Рассчитайте и установите множитель длины для достижения заданной длины
        float lengthMultiplier = targetRopeLength / initialRopeLength;
        rope.simulation.lengthMultiplier = lengthMultiplier;

        // Снова перетяните веревку, чтобы задать новую длину
        rope.ResetToSpawnCurve();

        rope.material.color = originalColor;
    }

    //просто подключить веревку
    public void Connect2(Rigidbody firstRigidbody, Rigidbody lastRigidbody, int destroyTime = 5)
    {
        var start = firstRigidbody.transform.TransformPoint(Vector3.zero);
        var end = lastRigidbody.transform.TransformPoint(Vector3.zero);

        rope.spawnPoints.Clear(); // Clear existing spawn points
        rope.spawnPoints.Add(transform.InverseTransformPoint(start));
        rope.spawnPoints.Add(transform.InverseTransformPoint(end));

        firstConnection.rigidbodySettings.body = firstRigidbody;
        firstConnection.transformSettings.transform = firstRigidbody.transform;
        
        lastConnection.rigidbodySettings.body = lastRigidbody;
        lastConnection.transformSettings.transform = lastRigidbody.transform;
    }

    public float CalculateRopeTension() => rope.GetCurrentLength();
    public float GetStartLength() => targetRopeLength;
}