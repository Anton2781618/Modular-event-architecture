#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace ModularEventArchitecture
{
    [CustomEditor(typeof(GameEntity), true)]
    public class GameEntityEditor : Editor
    {
        public VisualTreeAsset treeAsset;

        private Button _ADDModule;
        private VisualElement _modulesContainer;
        private bool _isMenuOpen = false;
        private GameEntity _targetEntity;

        private List<Type> availableModules;

        private void OnEnable()
        {
            _targetEntity = target as GameEntity;

            // Получаем все типы модулей
            var allModuleTypes = Assembly.GetAssembly(typeof(ModuleBase)).GetTypes().Where(t => typeof(ModuleBase).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            // Фильтруем модули с учетом обоих атрибутов
            availableModules = allModuleTypes.Where(moduleType =>
            {
                // Проверяем исключения
                var incompatibleAttributes = moduleType.GetCustomAttributes<IncompatibleUnitAttribute>();
                if (incompatibleAttributes.Any(attr => attr.UnitType.IsAssignableFrom(_targetEntity.GetType())))
                {
                    return false; // Модуль исключен для данного типа
                }

                // Проверяем совместимость
                var compatibleAttributes = moduleType.GetCustomAttributes<CompatibleUnitAttribute>();
                
                // Если нет атрибутов совместимости, считаем модуль совместимым
                if (!compatibleAttributes.Any())
                {
                    return true;
                }

                return compatibleAttributes.Any(attr => attr.UnitType.IsAssignableFrom(_targetEntity.GetType()));
            }).ToList();
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            // Add the script field at the top
            // var scriptField = new PropertyField(serializedObject.FindProperty("m_Script"));
            // scriptField.SetEnabled(false);
            // root.Add(scriptField);

            treeAsset.CloneTree(root);

            _ADDModule = root.Q<Button>("Button_ADDMondule");
            _modulesContainer = root.Q<VisualElement>("ModulesContainer");

            _ADDModule.clicked += ToggleModuleMenu;

            // Add default inspector properties
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            return root;
        }

        private void ToggleModuleMenu()
        {
            _isMenuOpen = !_isMenuOpen;
            
            if (_isMenuOpen)
            {
                _modulesContainer.style.display = DisplayStyle.Flex;
                CreateModuleButtons();
            }
            else
            {
                _modulesContainer.style.display = DisplayStyle.None;
                _modulesContainer.Clear();
            }
        }

        private void CreateModuleButtons()
        {
            _modulesContainer.Clear();

            foreach (var moduleType in availableModules)
            {
                if (_targetEntity.GetComponent(moduleType) != null) continue;
                
                string moduleName = moduleType.Name;
                
                Button moduleButton = new Button(() => AddModule(moduleType)) { text = moduleName };
                
                _modulesContainer.Add(moduleButton);
            }
        }

        private void AddModule(Type moduleType)
        {
            var newModule = _targetEntity.gameObject.AddComponent(moduleType) as ModuleBase;

            newModule.SetCharacter(_targetEntity);

            _targetEntity.AddModule(newModule);

            // После добавления модуля, закрываем меню
            ToggleModuleMenu();
            
            // Обновляем инспектор
            EditorUtility.SetDirty(target);
        }
    }
}
#endif