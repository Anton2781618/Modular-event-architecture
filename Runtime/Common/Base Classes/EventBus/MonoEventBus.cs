using System;
using System.Collections.Generic;
using UnityEngine;

namespace ModularEventArchitecture
{

    public abstract class MonoEventBus : MonoBehaviour
    {
        private List<(int id, Action<IEventData> action)> _globalEvents;
        public List<(int id, Action<IEventData> action)> Globalevents
        {
            get
            {
                if (_globalEvents == null)
                {
                    _globalEvents = new List<(int, Action<IEventData>)>();
                }
                return _globalEvents;
            }
            private set => _globalEvents = value;
        }

        private LocalEventBus _localEvents;
        public LocalEventBus LocalEvents
        {
            get
            {
                if (_localEvents == null)
                {
                    _localEvents = new LocalEventBus();
                    // Debug.Log($"Создан новый <color=green>LocalEventBus</color> для {gameObject.name}");
                }
                return _localEvents;
            }
            private set
            {
                _localEvents = value;
                // Debug.Log($"Установлен <color=red>LocalEventBus</color> для {gameObject.name}");

            } 
        }
        
        public void SetLocalEventBus(LocalEventBus localEventBus)
        {
            if (localEventBus == null)
            {
                Debug.LogError($"Попытка установить null LocalEventBus в {gameObject.name}");
                return;
            }

            _localEvents = localEventBus;
        }

        protected virtual void Awake() => Initialize();
        protected virtual void OnEnable() => SubscribeToEvents();
        protected virtual void OnDisable() => UnsubscribeFromAllEvents();
        protected virtual void OnDestroy() => UnsubscribeFromLocalEvents();
        protected abstract void Initialize();

        private void SubscribeToEvents()
        {
            // Debug.Log($"Подписка на события для {gameObject.name}");
            foreach (var item in Globalevents)
            {
                GlobalEventBus.Instance.Subscribe(item.id, item.action);
            }
        }

        //отписаться от всех событий вообще
        private void UnsubscribeFromAllEvents()
        {
            UnsubscribeFromLocalEvents();

            if (Globalevents != null)
            {
                foreach (var item in Globalevents)
                {
                    GlobalEventBus.Instance.Unsubscribe(item.id, item.action);
                }
                
                Globalevents.Clear();
            }
        }
        
        //отписаться от всех локальных событий
        private void UnsubscribeFromLocalEvents()
        {
            LocalEvents?.UnsubscribeAll();
        }
    }
}