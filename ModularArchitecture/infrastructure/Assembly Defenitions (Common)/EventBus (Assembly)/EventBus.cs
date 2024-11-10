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
        System_RestartScene,
        SpawnMob,
        ShowHelpWindow,
        // События для игры 2048
        Game2048_Move,
        Game2048_TileSpawned,
        Game2048_TileMove,
        Game2048_ScoreChanged,
        Game2048_GameOver,
        Game2048_RestartGame
    }

    protected EventBus()
    {
        Initialize();
    }

    protected void Initialize()
    {
        int count = Enum.GetValues(typeof(ActionsType)).Length;
        _events = new Action<IEventData>[count];
    }

    public void Subscribe<T>(int eventId, Action<T> handler) where T : IEventData
    {
        if (eventId < 0 || eventId >= _events.Length) return;

        Action<IEventData> wrapper = (data) => handler((T)data);
        
        if (!_handlerWrappers.ContainsKey(handler))
        {
            _handlerWrappers[handler] = wrapper;
        }
        
        _events[eventId] += wrapper;
    }

    public void Unsubscribe<T>(int eventId, Action<T> handler) where T : IEventData
    {
        Debug.Log("Отписаться от события " + Enum.GetName(typeof(ActionsType), eventId));
        
        if (eventId < 0 || eventId >= _events.Length) return;

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

        // Debug.Log("Publish " + Enum.GetName(typeof(ActionsType), eventId));
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
