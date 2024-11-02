using UnityEngine;
using System.Collections.Generic;
using System;

[DefaultExecutionOrder(-150)]
public class LevelManager : ManagerEntity
{
    private Dictionary<GameObject, GameEntity> DictEntities = new Dictionary<GameObject, GameEntity>();
    private List<GameEntity> entities = new List<GameEntity>(); 
    [SerializeField] private ObjectSpawner objectSpawner;
    [SerializeField] private GameObject text;
    
    protected override void Initialize()
    {
        Globalevents.Add((GlobalEventBus.События.Юнит_создан, (data) => AddEntity((CreateUnitEvent)data)));

        Globalevents.Add((GlobalEventBus.Команды.Показать_текст_в_точке, (data) => ShowText((ShowTextEvent)data)));

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
        Debug.Log("Сущность добавлена в Список " + obj.Unit.transform.name);
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