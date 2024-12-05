using System;
using System.Collections.Generic;
using UnityEngine;

namespace ModularEventArchitecture
{

    public abstract class MonoEventBus : MonoBehaviour
    {
        //Инициализировать себя при запуске или ждать пока кто нибудь другой проинициализирует объект
        [Tooltip("Инициализировать себя при запуске или ждать пока кто нибудь другой проинициализирует объект")]
        public bool InitializeSelf = true;
        
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
            
            if (!InitializeSelf)
            {
                Initialize();
                SubscribeToEvents();
            }
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
        
        private void UnsubscribeFromAllEvents()
        {
            LocalEvents?.UnsubscribeAll();

            if (Globalevents != null)
            {
                foreach (var item in Globalevents)
                {
                    GlobalEventBus.Instance.Unsubscribe(item.id, item.action);
                }
            }
        }
        
        private void UnsubscribeFromLocalEvents()
        {
            LocalEvents?.UnsubscribeAll();
        }
    }
}