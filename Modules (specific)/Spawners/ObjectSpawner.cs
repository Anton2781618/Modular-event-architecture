using UnityEngine;
using System.Collections.Generic;
using System;

namespace ModularEventArchitecture
{
    [CompatibleUnit(typeof(LevelManager))]
    public class ObjectSpawner : ModuleBase
    {
        [Serializable]
        public class SpawnableUnit
        {
            public GameObject prefab;
            [Range(0, 100)] public int spawnChance = 1;
        }

        public List<SpawnableUnit> spawnableUnits;
        public Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f);


        protected override void Initialize()
        {
            base.Initialize();

            Globalevents.Add((GlobalEventBus.События.Команды.Заспавнить_моба, (data) => SpawnUnit((SpawnEvent)data)));
        }

        public override void UpdateMe()
        {
            
        }

        private void SpawnUnit(SpawnEvent data) => SpawnUnit();
        
        [Tools.Button("Заспавнить юнитов")]
        private void SpawnUnit()
        {
            SpawnableUnit unitToSpawn = ChooseUnitToSpawn();
            
            if (unitToSpawn != null)
            {
                Vector3 spawnPosition = CalculateSpawnPosition();

                Instantiate(unitToSpawn.prefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                throw new Exception("No unit to spawn!!!!");
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
    }
}