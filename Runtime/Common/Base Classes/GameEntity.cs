using System;
using System.Collections.Generic;
using UnityEngine;

namespace ModularEventArchitecture
{
    public abstract class GameEntity : MonoEventBus
    {
        [HideInInspector] public List<ModuleBase> modules = new List<ModuleBase>();
        // Кэшируем часто используемые компоненты
        private Dictionary<Type, Component> cachedComponents;

        public virtual void UpdateMe()
        {
            foreach (var module in modules)
            {
                if (module == null) throw new NullReferenceException($"Обнаружена пустая ячейка в списке модульей! На объекте {transform.name}");
                    
                module.UpdateMe();
            }
        }

        protected override void OnEnable()
        {
            InitializeModules();

            base.OnEnable();

            GlobalEventBus.Instance.Publish(BasicActionsTypes.Commands.Unit_Created, new CreateUnitEvent {Unit = this});
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            GlobalEventBus.Instance.Publish(BasicActionsTypes.Commands.Unit_Die, new DieEvent { Unit = this });
        }

        public virtual void Deactivate() => gameObject.SetActive(false);
        public virtual void Reactivate() => gameObject.SetActive(true);
        public virtual void Use(){}

        public void AddModule(ModuleBase module)
        {
            if (module == null)
            {
                Debug.LogError($"Попытка добавить null модуль в {transform.name}");
                return;
            }

            if (modules.Contains(module)) return;

            modules.Add(module);

            module.Setup(this);

            // module.SetLocalEventBus(LocalEvents);
        }

        public void InitializeModules()
        {
            for (int i = 0; i < modules.Count; i++)
            {
                modules[i].Initialize();
            }
        }

        public void RemoveModule(ModuleBase module) => modules.Remove(module);

        public T GetModule<T>() where T : class
        {
            foreach (var module in modules)
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
            if(cachedComponents == null) cachedComponents = new Dictionary<Type, Component>();

            if (cachedComponents.TryGetValue(typeof(T), out var component))
            {
                return component as T;
            }
            
            // Если компонент еще не кэширован, получаем и кэшируем
            var newComponent = GetComponent<T>();
            if (newComponent != null)
            {
                cachedComponents[typeof(T)] = newComponent;
            }
            return newComponent;
        }

        [ContextMenu("Показать подули в консоле")]
        public void LogModules()
        {
            // Удаляем пустые модули из списка
            modules.RemoveAll(module => module == null);
            
            Debug.Log($"У объектка {transform.name} {modules.Count} модулей");
            foreach (var item in modules)
            {
                Debug.Log(item.GetType().Name);
            }
        }
        
        [ContextMenu("Вывести в консоль локальные события")]
        public void LogLocalEvents()
        {
            Debug.Log($"У объектка {transform.name} локальные события ");
            LocalEvents?.ShowAllEvents();
        }

        [ContextMenu("Вывести в консоль Глобальные события")]
        public void LogGlobalEvents()
        {
            Debug.Log($"У объектка {transform.name} глобальные события ");
            GlobalEventBus.Instance.ShowAllEvents();
        }
    }
}