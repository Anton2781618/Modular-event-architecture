using System;
using System.Collections.Generic;
using MyScripts.Architecture.Attributes.PopupDrawer;
using UnityEngine;

namespace ModularEventArchitecture
{

    [CreateAssetMenu(fileName = "TagsBuilder", menuName = "TagsBuilder")] [Serializable]
    public class TagsBuilder : ScriptableObject
    {
        [Header("Список тегов (расширяемый)")]
        public List<string> HierarchyItemsTypes;
        public HierarchyItem[] hierarchyItems;


        [Header("Список модулей и их совместимых тегов")]
        public List<ModuleTagPair> ModuleTagPairs = new List<ModuleTagPair>();


        [Tools.Button("Построить иерархию тегов")]
        public void BuildHierarchy()
        {
            HierarchyItemsTypes.Clear();

            foreach (var hierarchyUnit in hierarchyItems)
            {
                foreach (var item in hierarchyUnit.HierarchyItemtype)
                {
                    HierarchyItemsTypes.Add(hierarchyUnit.NameHierarchy + "/" + item);
                }
            }
        }

        [Tools.Button("Дозаполнить модули")]
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
        }
        
    }

    [Serializable]
    public class HierarchyItem
    {
        public string NameHierarchy = "";
        public string[] HierarchyItemtype;
    }

    [Serializable]
    public class ModuleTagPair
    {
        [Tooltip("Класс модуля (наследник ModuleBase)")]
        public string ModuleReference;

        [Tooltip("Совместимый тег для этого модуля")]
        [Popup] public string CompatibleTag = "/None";
    }
}