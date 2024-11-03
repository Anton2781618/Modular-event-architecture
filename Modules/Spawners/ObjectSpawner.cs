using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[CompatibleUnit(typeof(LevelManager))]
public class ObjectSpawner : ModuleBase
{
    [Serializable]
    public class SpawnableUnit
    {
        public string prefabName = "NPC";
        public ScriptableObject unitData;
        public float spawnChance = 1f;
    }

    public List<SpawnableUnit> spawnableUnits;
    public float spawnInterval = 5f;
    public int maxSpawnedUnits = 10;
    public Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f);

    private float nextSpawnTime;
    public ObjectPool objectPool;

    protected override void Initialize()
    {
        base.Initialize();

        // objectPool = GetComponent<ObjectPool>();

        // if (objectPool == null)
        // {
        //     objectPool = gameObject.AddComponent<ObjectPool>();
        // }

        nextSpawnTime = Time.time + spawnInterval;

        Globalevents.Add((GlobalEventBus.События.Команды.Заспавнить_моба, (data) => SpawnUnit((SpawnEvent)data)));
    }

    public override void UpdateMe()
    {
        
    }


    //заспавнить юнитов
    private IEnumerator SpawnUnits(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnUnit();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnUnit(SpawnEvent data) => SpawnUnit();
    
    [Tools.Button("Заспавнить юнитов")]
    private void SpawnUnit()
    {
        SpawnableUnit unitToSpawn = ChooseUnitToSpawn();
        if (unitToSpawn != null)
        {
            Vector3 spawnPosition = CalculateSpawnPosition();
            GameObject spawnedObject = objectPool.GetObject(unitToSpawn.prefabName, spawnPosition, Quaternion.identity);
            GameEntity spawnedUnit = spawnedObject.GetComponent<GameEntity>();
        }
    }

    private SpawnableUnit ChooseUnitToSpawn()
    {
        float totalChance = 0f;
        foreach (var unit in spawnableUnits)
        {
            totalChance += unit.spawnChance;
        }

        float randomValue = UnityEngine.Random.Range(0f, totalChance);
        float currentChance = 0f;

        foreach (var unit in spawnableUnits)
        {
            currentChance += unit.spawnChance;
            if (randomValue <= currentChance)
            {
                return unit;
            }
        }

        return null;
    }

    private Vector3 CalculateSpawnPosition()
    {
        return transform.position + new Vector3( UnityEngine.Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2), 0, UnityEngine.Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2));
    }

    public void ReturnUnitToPool(GameEntity unit, string prefabName)
    {
        unit.Deactivate();
        objectPool.ReturnObject(unit.gameObject, prefabName);
    }
}
