using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace ModularEventArchitecture
{
    public interface IEventData { }
    
    public interface IEventType
    {
        int Id { get; }
        string EventName { get; }
    }

    public abstract class EventBus : IDisposable
    {
        // Для каждого типа события храним Subject<T>
        private readonly Dictionary<int, object> _subjects = new Dictionary<int, object>();

        protected EventBus() { }

        // Получить поток событий определённого типа
        public IObservable<T> Observe<T>(IEventType eventType) where T : IEventData
        {
            int eventId = eventType.Id;
            if (!_subjects.TryGetValue(eventId, out var subject))
            {
                subject = new Subject<T>();
                _subjects[eventId] = subject;
            }
            return (Subject<T>)subject;
        }

        // Публикация события
        public void Publish<T>(IEventType eventType, T data) where T : IEventData
        {
            int eventId = eventType.Id;
            if (_subjects.TryGetValue(eventId, out var subject))
            {
                ((Subject<T>)subject).OnNext(data);
                // Debug.Log($"Publish {eventType.EventName}");
            }
        }

        // Отписка происходит автоматически через Disposable
        public void Dispose()
        {
            foreach (var subject in _subjects.Values)
            {
                (subject as IDisposable)?.Dispose();
            }
            _subjects.Clear();

            Debug.Log("EventBus disposed");
        }

        public void ShowAllEvents()
        {
            foreach (var kvp in _subjects)
            {
                var type = kvp.Value.GetType();
                Debug.Log($"Событие {kvp.Key} Subject: {type.Name}");
            }
        }
    }
}