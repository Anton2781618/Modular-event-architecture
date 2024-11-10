using UnityEngine;
using System.Collections.Generic;
using System;

namespace ModularEventArchitecture
{
    [DefaultExecutionOrder(-150)]
    public class LevelManager : ManagerEntity
    {
        /* LevelManager в событийно-ориентированной архитектуре должен отвечать за:

        1)Управление состоянием уровня:
            Инициализация уровня
            Управление игровым циклом (старт, пауза, завершение)
            Отслеживание целей/условий уровня
            Управление состоянием игрового мира
        
        2)Спавн и управление сущностями:
            Создание начальных сущностей
            Управление точками спавна
            Отслеживание активных сущностей
            Очистка/сброс уровня
        
        3)Игровые события уровня:
            Триггеры событий уровня
            Контрольные точки
            Условия победы/поражения
            Переходы между состояниями уровня
            Взаимодействие с другими менеджерами:
            Уведомление UIManager о событиях уровня
            Координация с другими системами

        LevelManager НЕ должен:
            Управлять UI
            Заниматься конкретной игровой логикой сущностей
            Обрабатывать ввод игрока */
            
        private Dictionary<GameObject, GameEntity> DictEntities = new Dictionary<GameObject, GameEntity>();
        private List<GameEntity> entities = new List<GameEntity>(); 
        [SerializeField] private GameObject text;
        
        protected override void Initialize()
        {
            Globalevents.Add((GlobalEventBus.События.Юнит_создан, (data) => AddEntity((CreateUnitEvent)data)));

            Globalevents.Add((GlobalEventBus.События.Команды.Показать_текст_в_точке, (data) => ShowText((ShowTextEvent)data)));

            Globalevents.Add((GlobalEventBus.События.Юнит_погиб, (data) => RemoveEntity((DieEvent)data)));
        }

        private void Update()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].UpdateMe();
            }
        }

        private void AddEntity(CreateUnitEvent obj)
        {
            // Debug.Log("Сущность добавлена в Список " + obj.Unit.transform.name);
            GameEntity entity = obj.Unit;

            if (entity != null && !DictEntities.ContainsKey(entity.gameObject))
            {
                DictEntities[entity.gameObject] = entity;
            }
            else
            {
                Debug.LogWarning("Попытка добавить null или дублирующую сущность.");
            }

            if (entity != null && !entities.Contains(entity))
            {
                entities.Add(entity);
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

            if (entity != null && DictEntities.ContainsKey(entity.gameObject))
            {
                DictEntities.Remove(entity.gameObject);
            
                // if (entity is Unit unit)
                // {
                    // objectSpawner.ReturnUnitToPool(unit, unit.GetType().Name);
                // }
            }

            if (entity != null && entities.Contains(entity))
            {
                entities.Remove(entity);
            }
        }

        public GameEntity GetEntity(GameObject gameObject)
        {
            return DictEntities.TryGetValue(gameObject, out GameEntity entity) ? entity : null;
        }

        public int GetEntityCount() => DictEntities.Count;

        internal void ShowText(ShowTextEvent obj)
        {
            text.transform.position = obj.Point;

            text.SetActive(obj.Enabled);
        }

        [ContextMenu("ShowEntetys")]
        public void ShowEntetys()
        {
            Debug.Log(DictEntities.Values.Count);
            foreach (var entity in DictEntities.Values)
            {
                Debug.Log(entity.gameObject.name);
            }
        }
    }
}