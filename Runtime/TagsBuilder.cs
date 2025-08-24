using System;
using System.Collections.Generic;
using UnityEngine;

namespace ModularEventArchitecture
{

    [CreateAssetMenu(fileName = "TagsBuilder", menuName = "TagsBuilder")] [Serializable]
    public class TagsBuilder : ScriptableObject
    {

        // Кэш для быстрого доступа к парам модуль/тег
        private Dictionary<string, ModuleTagPair> _moduleTagDict = new Dictionary<string, ModuleTagPair>();
        [Header("Список тегов (расширяется по кнопке)")]
        [ReadOnly] public List<string> HierarchyTags;

        [Header("Структура иерархии тегов")]
        [SerializeField] private HierarchyItemTag[] hierarchyBuilder;


        [Header("Список модулей и их совместимых тегов")]
        public List<ModuleTagPair> ModuleTagPairs = new List<ModuleTagPair>();

        // Обновление кэша при изменении списка
        private void UpdateModuleTagDict()
        {
            _moduleTagDict.Clear();
            foreach (var pair in ModuleTagPairs)
            {
                if (!string.IsNullOrEmpty(pair.ModuleReference))
                    _moduleTagDict[pair.ModuleReference] = pair;
            }
        }

        // Быстрый доступ к паре по имени модуля
        public ModuleTagPair GetModuleTagPair(string moduleName)
        {
            if (_moduleTagDict.Count != ModuleTagPairs.Count)
                UpdateModuleTagDict();
            return _moduleTagDict.TryGetValue(moduleName, out var pair) ? pair : null;
        }


        [Button("Построить иерархию тегов")]
        public void BuildHierarchy()
        {
            HierarchyTags.Clear();

            foreach (var hierarchyUnit in hierarchyBuilder)
            {
                foreach (var item in hierarchyUnit.HierarchyItemtype)
                {
                    HierarchyTags.Add(hierarchyUnit.NameHierarchy + "/" + item);
                }
            }
        }

        [Button("Перезаполнить модули")]
        public void FillModules()
        {
            // Получаем все типы-наследники ModuleBase
            var moduleTypes = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    var types = assembly.GetTypes();
                    foreach (var type in types)
                    {
                        if (typeof(ModuleBase).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                        {
                            moduleTypes.Add(type);
                        }
                    }
                }
                catch { /* некоторые сборки могут выбрасывать ошибки */ }
            }

            // Собираем имена актуальных модулей
            var actualNames = new HashSet<string>(moduleTypes.ConvertAll(type => type.Name));

            // Удаляем устаревшие модули
            ModuleTagPairs.RemoveAll(pair => !actualNames.Contains(pair.ModuleReference));

            // Собираем имена уже добавленных модулей
            var existingNames = new HashSet<string>(ModuleTagPairs.ConvertAll(pair => pair.ModuleReference));

            // Добавляем новые модули
            foreach (var type in moduleTypes)
            {
                if (existingNames.Contains(type.Name)) continue;

                ModuleTagPairs.Add(new ModuleTagPair
                {
                    ModuleReference = type.Name,
                    CompatibleTag = "/None" // или другой дефолтный тег
                });
            }
            UpdateModuleTagDict();
        }
        
    }

    [Serializable]
    public class HierarchyItemTag
    {
        public string NameHierarchy = "";
        public string[] HierarchyItemtype;
    }

    [Serializable]
    public class ModuleTagPair
    {
        [Tooltip("Класс модуля (наследник ModuleBase)")]
        [ReadOnly] public string ModuleReference;

        [Tooltip("Совместимый тег для этого модуля")]
        [Drop] public string CompatibleTag = "/None";
    }
}