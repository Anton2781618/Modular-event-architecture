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
        //--------------------------------------------------------------------------------
        // Для каждого типа события храним Subject<T>
        private readonly Dictionary<int, object> _subjects = new Dictionary<int, object>();

        // Для событий-запросов с возвратом результата
        private readonly Dictionary<int, object> _requestSubjects = new Dictionary<int, object>();
        private readonly Dictionary<int, object> _responseSubjects = new Dictionary<int, object>();
        protected EventBus() { }

        //!--------------------------------------------------------------------------------

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


        // Подписка на запрос
        public void SubscribeRequest<TRequest, TResponse>(IEventType eventType, Func<TRequest, TResponse> handler)
            where TRequest : IEventData
        {
            int eventId = eventType.Id;
            var requestSubject = GetOrCreateSubject<TRequest>(_requestSubjects, eventId);
            var responseSubject = GetOrCreateSubject<TResponse>(_responseSubjects, eventId);

            ((ISubject<TRequest>)requestSubject).Subscribe(request =>
            {
                var response = handler(request);
                ((ISubject<TResponse>)responseSubject).OnNext(response);
            });
        }

        // Публикация запроса и получение ответа
        public IObservable<TResponse> PublishRequest<TRequest, TResponse>(IEventType eventType, TRequest request)
            where TRequest : IEventData
        {
            int eventId = eventType.Id;
            var requestSubject = GetOrCreateSubject<TRequest>(_requestSubjects, eventId);
            var responseSubject = GetOrCreateSubject<TResponse>(_responseSubjects, eventId);

            ((ISubject<TRequest>)requestSubject).OnNext(request);
            
            return ((IObservable<TResponse>)responseSubject).Take(1); // только первый ответ
        }

        // Вспомогательный метод для получения/создания Subject
        private object GetOrCreateSubject<T>(Dictionary<int, object> dict, int eventId)
        {
            if (!dict.TryGetValue(eventId, out var subject))
            {
                // Для responseSubject используем ReplaySubject, чтобы не терять первый ответ
                if (dict == _responseSubjects)
                {
                    subject = new ReplaySubject<T>(1);
                }
                else
                {
                    subject = new Subject<T>();
                }
                dict[eventId] = subject;
            }
            return subject;
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

        // Вспомогательный метод: безопасно получить свойство HasObservers при наличии
        private string GetHasObservers(object subject)
        {
            if (subject == null) return "null";
            var prop = subject.GetType().GetProperty("HasObservers");
            if (prop != null && prop.PropertyType == typeof(bool))
            {
                try
                {
                    bool val = (bool)prop.GetValue(subject);
                    return val.ToString();
                }
                catch
                {
                    return "error";
                }
            }
            return "N/A";
        }
    }
}