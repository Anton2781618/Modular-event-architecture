using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace ModularEventArchitecture
{
    //Класс представляющий игровую сущность (юнит, предмет и т.д.)
    //Содержит в себе модули, которые реализуют функционал
    public class GameEntity : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------
        [Drop] public string[] EntityTag;
        //-------------------------------------------------------------------------------------
        private IDisposable _updateSubscription;
        //-------------------------------------------------------------------------------------
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
        [SerializeField] [ReadOnly] private List<ModuleBase> _modules = new List<ModuleBase>();
         public List<ModuleBase> Modules
        {
            get
            {
                if (_modules == null)
                {
                    _modules = new List<ModuleBase>();
                }
                return _modules;
            }

            private set => _modules = value; 
        }

        //-------------------------------------------------------------------------------------
        // Кэшируем часто используемые компоненты
        private Dictionary<Type, Component> _cachedComponents;

        //!-------------------------------------------------------------------------------------
      
        public virtual void UpdateMe()
        {
            foreach (var module in Modules)
            {
                if (module == null) throw new NullReferenceException($"Обнаружена пустая ячейка в списке модульей! На объекте {transform.name}");
                    
                module.UpdateMe();
            }
        }

        protected virtual void OnEnable()
        {
            InitializeModules();
            GlobalEventBus.Instance.Publish(BasicActionsTypes.Unit_Created, new CreateUnitEvent {Unit = this});

            // Реактивный Update через UniRx
            _updateSubscription = Observable.EveryUpdate()
                .Subscribe(_ => UpdateMe())
                .AddTo(this);
        }

        protected virtual void OnDisable()
        {
            GlobalEventBus.Instance.Publish(BasicActionsTypes.Unit_Die, new DieEvent { Unit = this });
            // Отписка от реактивного Update
            _updateSubscription?.Dispose();
        }
        // Подписка на глобальные события через UniRx
        public void SubscribeGlobalEvent<T>(IEventType eventType, System.Action<T> handler) where T : IEventData
        {
            GlobalEventBus.Instance.Observe<T>(eventType)
                .Subscribe(handler)
                .AddTo(this);
        }

        // Подписка на локальные события через UniRx
        public void SubscribeLocalEvent<T>(IEventType eventType, System.Action<T> handler) where T : IEventData
        {
            LocalEvents.Observe<T>(eventType)
                .Subscribe(handler)
                .AddTo(this);
        }

        // Вызов глобального события
        public void PublishGlobalEvent<T>(IEventType eventType, T data) where T : IEventData
        {
            GlobalEventBus.Instance.Publish(eventType, data);
        }

        // Вызов локального события
        public void PublishLocalEvent<T>(IEventType eventType, T data) where T : IEventData
        {
            LocalEvents.Publish(eventType, data);
        }

        public void AddModule(ModuleBase module)
        {
            if (module == null)
            {
                Debug.LogError($"Попытка добавить null модуль в {transform.name}");
                return;
            }

            if (Modules.Contains(module)) return;

            Modules.Add(module);

            module.Setup(this);
        }

        private void InitializeModules()
        {
            for (int i = 0; i < Modules.Count; i++)
            {
                Modules[i].Initialize();
            }
        }

        public void RemoveModule(ModuleBase module) => Modules.Remove(module);

        public T GetModule<T>() where T : class
        {
            foreach (var module in Modules)
            {
                if (module is T result)
                {
                    return result;
                }
            }
            
            return new NotImplementedException($"такого модуля нет в списке модулей у {transform.name}") as T;
        }

        public T GetCachedComponent<T>() where T : Component
        {
            if(_cachedComponents == null) _cachedComponents = new Dictionary<Type, Component>();

            if (_cachedComponents.TryGetValue(typeof(T), out var component))
            {
                return component as T;
            }
            
            // Если компонент еще не кэширован, получаем и кэшируем
            var newComponent = GetComponent<T>();
            if (newComponent != null)
            {
                _cachedComponents[typeof(T)] = newComponent;
            }
            return newComponent;
        }

        [ContextMenu("Показать подули в консоле")]
        public void LogModules()
        {
            // Удаляем пустые модули из списка
            Modules.RemoveAll(module => module == null);
            
            Debug.Log($"У объектка {transform.name} {Modules.Count} модулей");
            foreach (var item in Modules)
            {
                Debug.Log(item.GetType().Name);
            }
        }
        
        [ContextMenu("Вывести в консоль локальные события")]
        public void LogLocalEvents()
        {
            Debug.Log($"У объектка {transform.name} локальные события ");
            LocalEvents.ShowAllEvents();
        }

        [ContextMenu("Вывести в консоль Глобальные события")]
        public void LogGlobalEvents()
        {
            Debug.Log($"У объектка {transform.name} глобальные события ");
            GlobalEventBus.Instance.ShowAllEvents();
        }

        [ContextMenu("Провести поиск модулей на объекте")]
        private void FindModules()
        {
            Modules.Clear();
            Modules.AddRange(GetComponents<ModuleBase>());
            Modules.ForEach(t=>t.Setup(this));
        }
    }
}