using System;
using System.Collections.Generic;
using UnityEngine;

public interface IEventData { }

public abstract class EventBus
{
    private Action<IEventData>[] _events;
    private Dictionary<Delegate, Action<IEventData>> _handlerWrappers = new Dictionary<Delegate, Action<IEventData>>();

    public enum ActionsType
    {
        Move_To_point,
        Move_To_target,
        Stop,
        Attack,
        Stop_Fight,
        Take_Damage,
        Unit_Die,
        Unit_Created,
        ShowText_ToPoint,
        Rope_Stretched,
        IsUIOpen,
        StartSprint,
        StopSprint,
        Stamina_change,
        UpdateUI,
        Health_change,
        Stunned,
        Effect_extend,
        EffectCreated,
        Rope_Connected,
        Rope_Disconnected,
        Rope_Relaxed,
        Tired,
    }

    protected EventBus()
    {
        
        Initialize();
    }

    protected void Initialize()
    {
        Debug.Log("!!!!!!!!!!!!!!!!Initialize!!!!!!!!!!!!!!");
        int count = Enum.GetValues(typeof(ActionsType)).Length;

        _events = new Action<IEventData>[count];
    }

    public void Subscribe<T>(int eventId, Action<T> handler) where T : IEventData
    {
        // Debug.Log("Создана подписка на событие " + Enum.GetName(typeof(ActionsType), eventId));
        if (eventId < 0 || eventId >= _events.Length) return;

        // Создаем замыкание один раз и сохраняем его
        Action<IEventData> wrapper = (data) => handler((T)data);
        
        // Сохраняем wrapper в словаре для последующей отписки
        if (!_handlerWrappers.ContainsKey(handler))
        {
            _handlerWrappers[handler] = wrapper;
        }
        
        _events[eventId] += wrapper;

        Debug.Log("проверКА !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        ShowAllEvents();
    }

    public void Unsubscribe<T>(int eventId, Action<T> handler) where T : IEventData
    {
        Debug.Log("Отписаться от события " + Enum.GetName(typeof(ActionsType), eventId));
        
        if (eventId < 0 || eventId >= _events.Length) return;

        // Получаем сохраненное замыкание
        if (_handlerWrappers.TryGetValue(handler, out var wrapper))
        {
            _events[eventId] -= wrapper;
            _handlerWrappers.Remove(handler);
        }
    }

    public void UnsubscribeAll()
    {
        Debug.Log("Отписаться от всех");
        for (int i = 0; i < _events.Length; i++)
        {
            _events[i] = null;
        }
        _handlerWrappers.Clear();
    }

    public void Publish<T>(int eventId, T data) where T : IEventData
    {
        if (eventId < 0 || eventId >= _events.Length) return;

        Debug.Log("Publish " + Enum.GetName(typeof(ActionsType), eventId));
        _events[eventId]?.Invoke(data);
    }

    public void ShowAllEvents()
    {
        for (int i = 0; i < _events.Length; i++)
        {
            if (_events[i] != null)
            {
                Debug.Log($"Event {Enum.GetName(typeof(ActionsType), i)} has {_events[i].GetInvocationList().Length} subscribers");
            }
        }
    }
}

// Сериализуемые структуры событий
[Serializable]
public class BaseEvent : IEventData 
{
    public Sprite sprite;
    public bool Enabled;
}

[Serializable]
public class EffectEvent : BaseEvent
{
    public Effect Effect;
}

[Serializable]
public struct DieEvent : IEventData
{    
    public GameEntity Unit;
}

[Serializable]
public struct CreateUnitEvent : IEventData
{
    public GameEntity Unit;
}

[Serializable]
public struct ShowTextEvent : IEventData
{
    public Vector3 Point;
    public bool Enabled;
}

[Serializable]
public struct AttackEvent : IEventData
{
    public GameObject Unit;
}

[Serializable]
public struct StopFightEvent : IEventData { }

[Serializable]
public struct MoveToPointEvent : IEventData
{
    [SerializeField] private Vector3 point;
    public Vector3 Point { get => point; set => point = value; }
}

[Serializable]
public struct MoveToTargetEvent : IEventData
{
    [SerializeField] private Transform targetTransform;
    public Transform target { get => targetTransform; set => targetTransform = value; }
}

[Serializable]
public struct DamageEvent : IEventData
{
    [SerializeField] private float damage;
    [SerializeField] private Vector3 hitDirection;
    
    public float Damage { get => damage; set => damage = value; }
    public Vector3 HitDirection { get => hitDirection; set => hitDirection = value; }
}

[Serializable]
public struct StunEvent : IEventData
{
    [SerializeField] private bool stunState;
    [SerializeField] private float stunTime;

    public bool isStun { get => stunState; set => stunState = value; }
    public float Time { get => stunTime; set => stunTime = value; }
}

[Serializable]
public struct StaminaChangedEvent : IEventData 
{
    public float MaxStamina;
    public float CurrentStamina;
}

[Serializable]
public struct HealthChangedEvent : IEventData 
{
    public float MaxHealth;
    public float CurrentHealth;
}
