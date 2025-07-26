using System.Collections.Generic;
using UnityEngine;

namespace ModularEventArchitecture
{
    [CompatibleUnit(typeof(LevelManager))] [DefaultExecutionOrder(-148)]
    public class EntityLifecycleModule : ModuleBase
    {
        //-------------------------------------------------------------------------------------
        //список всех сущностей на уровне
        private Dictionary<GameObject, GameEntity> _dictEntities = new Dictionary<GameObject, GameEntity>();
        private List<GameEntity> _entities = new List<GameEntity>(); 

        //!-------------------------------------------------------------------------------------

        public override void Initialize()
        {
            Entity.Globalevents.Add((BasicActionsTypes.Commands.Unit_Created, (data) => AddEntity((CreateUnitEvent)data)));

            Entity.Globalevents.Add((BasicActionsTypes.Commands.Unit_Die, (data) => RemoveEntity((DieEvent)data)));
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

        [Button("Вывести списки в консоль")]
        public void ShowEntetys()
        {
            Debug.Log($"В СЛОВАРЕ <color=green> {_dictEntities.Values.Count}</color>");

            foreach (var entity in _dictEntities.Values)
            {
                Debug.Log($"<color=green>{entity.gameObject.name}</color>");
            }

            Debug.Log("<color=green>====================================</color>");

            Debug.Log($"В СПИСКЕ <color=red> {_entities.Count}</color>");

            foreach (var item in _entities)
            {
                Debug.Log($"<color=red>{item.gameObject.name}</color>");
            }

            Debug.Log("<color=red>====================================</color>");
        }
    }
}