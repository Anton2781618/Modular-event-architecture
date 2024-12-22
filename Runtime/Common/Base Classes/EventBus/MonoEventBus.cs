using System;
using System.Collections.Generic;
using UnityEngine;

namespace ModularEventArchitecture
{
    public abstract class MonoEventBus : MonoBehaviour
    {
        private List<(IEventType type, Action<IEventData> action)> _globalEvents;
        public List<(IEventType type, Action<IEventData> action)> Globalevents
        {
            get
            {
                if (_globalEvents == null)
                {
                    _globalEvents = new List<(IEventType, Action<IEventData>)>();
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
                }
                return _localEvents;
            }
            private set => _localEvents = value;
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
        protected abstract void Initialize();

        private void SubscribeToEvents()
        {
            foreach (var item in Globalevents)
            {
                GlobalEventBus.Instance.Subscribe(item.type, item.action);
            }
        }

        private void UnsubscribeFromAllEvents()
        {
            UnsubscribeFromLocalEvents();

            if (Globalevents != null)
            {
                foreach (var item in Globalevents)
                {
                    GlobalEventBus.Instance.Unsubscribe(item.type, item.action);
                }
                
                Globalevents.Clear();
            }
        }
        
        private void UnsubscribeFromLocalEvents()
        {
            LocalEvents?.UnsubscribeAll();
        }
    }
}
