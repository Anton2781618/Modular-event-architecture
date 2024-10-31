using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEntity : MonoEventBus
{
    public List<IModule> modules = new List<IModule>();
    public virtual void UpdateMe()
    {
        foreach (var module in modules)
        {
            module.UpdateMe();
        }
    }
    public virtual void Deactivate() => gameObject.SetActive(false);
    public virtual void Reactivate() => gameObject.SetActive(true);
    public virtual void Use(){}

    public void AddModule(IModule module)
    {
        if (modules.Contains(module)) return;

        modules.Add(module);

        module.SetLocalEventBus(LocalEvents);
    }

    public void RemoveModule(IModule module) => modules.Remove(module);

    public T GetModule<T>() where T : class
    {
        if (typeof(T) == typeof(IStatus))
        {
            return modules.Find(x => x is IStatus) as T;
        }
        // Добавьте другие проверки для других типов модулей по необходимости
        
        
        return new NotImplementedException($"такого модуля нет в списке модулей у {transform.name}") as T;
    }

    [ContextMenu("Показать подули")]
    public void LogModules()
    {
        Debug.Log($"У объектка {transform.name} {modules.Count} модулей");
        foreach (var item in modules)
        {
            Debug.Log(item.GetType().Name);
        }
    }
}

public abstract class MonoEventBus : MonoBehaviour
{
    //Инициализировать себя при запуске или ждать пока кто нибудь другой проинициализирует объект
    [Tooltip("Инициализировать себя при запуске или ждать пока кто нибудь другой проинициализирует объект")]
    public bool InitializeSelf = true;
    public List<(int id, Action<IEventData> action)> Globalevents {get; private set;} = new List<(int, Action<IEventData>)>();
    public LocalEventBus LocalEvents {get; private set;} = new LocalEventBus();

    public void SetLocalEventBus(LocalEventBus localEventBus)
    {
        LocalEvents = localEventBus;
        
        if (InitializeSelf) return;
        
        Initialize();

        SubscribeToEvents();
    }


    protected virtual void Awake() => Initialize();
    protected virtual void OnEnable() => SubscribeToEvents();
    protected virtual void OnDisable() => UnsubscribeFromAllEvents();
    protected virtual void OnDestroy() => UnsubscribeFromLocalEvents();
    protected abstract void Initialize();
    private void SubscribeToEvents()
    {
        // Debug.Log ( GetType().Name + " Подписался на глобальные события / Имя объекта" + transform.name );
        foreach (var item in Globalevents)
        {
            GlobalEventBus.Instance.Subscribe(item.id, item.action);
        }
    }
    
    private void UnsubscribeFromAllEvents()
    {
        LocalEvents.UnsubscribeAll();

        Debug.Log($"{GetType().Name} отписался от ГЛОБАЛЬНЫХ событи");

        foreach (var item in Globalevents)
        {
            GlobalEventBus.Instance.Unsubscribe(item.id, item.action);
        }
    }

    private void UnsubscribeFromLocalEvents()
    {
        Debug.Log($"{GetType().Name} отписался от ЛОКАЛЬНЫХ событи");

        LocalEvents.UnsubscribeAll();
    }

}