using System;
using System.Collections.Generic;
using UnityEngine;

namespace ModularEventArchitecture
{
    public interface IEventData { }
    
    public interface IEventType
    {
        int GetEventId();
        string GetEventName();
    }

    public abstract class EventBus
    {
        private Dictionary<int, Action<IEventData>> _events = new Dictionary<int, Action<IEventData>>();
        private Dictionary<Delegate, Action<IEventData>> _handlerWrappers = new Dictionary<Delegate, Action<IEventData>>();

        protected EventBus() { }

        public void Subscribe<T>(IEventType eventType, Action<T> handler) where T : IEventData
        {
            int eventId = eventType.GetEventId();
            Debug.Log($"Подписаться на событие {eventType.GetEventName()}");

            Action<IEventData> wrapper = (data) => handler((T)data);
            
            if (!_handlerWrappers.ContainsKey(handler))
            {
                _handlerWrappers[handler] = wrapper;
            }

            if (!_events.ContainsKey(eventId))
            {
                _events[eventId] = null;
            }
            
            _events[eventId] += wrapper;
        }

        public void Unsubscribe<T>(IEventType eventType, Action<T> handler) where T : IEventData
        {
            int eventId = eventType.GetEventId();
            Debug.Log($"Отписаться от события {eventType.GetEventName()}");
            
            if (!_events.ContainsKey(eventId)) return;

            if (_handlerWrappers.TryGetValue(handler, out var wrapper))
            {
                _events[eventId] -= wrapper;
                _handlerWrappers.Remove(handler);
                
                if (_events[eventId] == null)
                {
                    _events.Remove(eventId);
                }
            }
        }

        public void UnsubscribeAll()
        {
            Debug.Log("Отписаться от всех событий");
            _events.Clear();
            _handlerWrappers.Clear();
        }

        public void Publish<T>(IEventType eventType, T data) where T : IEventData
        {
            int eventId = eventType.GetEventId();
            if (!_events.ContainsKey(eventId)) return;

            Debug.Log($"Publish {eventType.GetEventName()}");
            _events[eventId]?.Invoke(data);
        }

        public void ShowAllEvents()
        {
            foreach (var eventPair in _events)
            {
                if (eventPair.Value != null)
                {
                    Debug.Log($"Событие {eventPair.Key} имеет {eventPair.Value.GetInvocationList().Length} подписчиков");
                }
            }
        }
    }
}
