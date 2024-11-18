using System.Collections.Generic;
using UnityEngine;

namespace ModularEventArchitecture
{
    [CompatibleUnit(typeof(LevelManager))] [DefaultExecutionOrder(-148)]
    public class EntityLifecycleModule : ModuleBase
    {
        private Dictionary<GameObject, GameEntity> _dictEntities = new Dictionary<GameObject, GameEntity>();
        private List<GameEntity> _entities = new List<GameEntity>(); 

        protected override void Initialize()
        {
            base.Initialize();

            Globalevents.Add((GlobalEventBus.События.Юнит_создан, (data) => AddEntity((CreateUnitEvent)data)));

            Globalevents.Add((GlobalEventBus.События.Юнит_погиб, (data) => RemoveEntity((DieEvent)data)));
        }

        public override void UpdateMe()
        {
        }
        private void Update()
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                _entities[i].UpdateMe();
            }
        }

        private void AddEntity(CreateUnitEvent obj)
        {
            // Debug.Log("Сущность добавлена в Список " + obj.Unit.transform.name);
            GameEntity entity = obj.Unit;

            if (entity != null && !_dictEntities.ContainsKey(entity.gameObject))
            {
                _dictEntities[entity.gameObject] = entity;
            }
            else
            {
                Debug.LogWarning("Попытка добавить null или дублирующую сущность.");
            }

            if (entity != null && !_entities.Contains(entity))
            {
                _entities.Add(entity);
            }
            else
            {
                Debug.LogWarning("Попытка добавить null или дублирующую сущность.");
            }
        }

        public void RemoveEntity(DieEvent obj)
        {
            Debug.Log("Сущность удалина " + obj.Unit.transform.name);

            GameEntity entity = obj.Unit;

            if (entity != null && _dictEntities.ContainsKey(entity.gameObject))
            {
                _dictEntities.Remove(entity.gameObject);
            
                // if (entity is Unit unit)
                // {
                    // objectSpawner.ReturnUnitToPool(unit, unit.GetType().Name);
                // }
            }

            if (entity != null && _entities.Contains(entity))
            {
                _entities.Remove(entity);
            }
        }

        public GameEntity GetEntity(GameObject gameObject)
        {
            return _dictEntities.TryGetValue(gameObject, out GameEntity entity) ? entity : null;
        }

        public int GetEntityCount() => _dictEntities.Count;

        [Tools.Button("Вывести списки в консоль")]
        public void ShowEntetys()
        {
            Debug.Log($"В словаре {_dictEntities.Values.Count}" );

            foreach (var entity in _dictEntities.Values)
            {
                Debug.Log(entity.gameObject.name);
            }

            Debug.Log($"В списке {_entities.Count}" );

            foreach (var item in _entities)
            {
                Debug.Log(item.gameObject.name);
                
            }
        }

    }
}