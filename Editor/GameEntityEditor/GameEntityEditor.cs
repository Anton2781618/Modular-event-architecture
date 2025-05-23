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
        //-------------------------------------------------------------------------------------

        public VisualTreeAsset treeAsset;

        private Button _ADDModule;
        private VisualElement _modulesContainer;
        private bool _isMenuOpen = false;
        private GameEntity _targetEntity;

        private List<Type> availableModules;
        //!-------------------------------------------------------------------------------------


        protected virtual void OnEnable()
        {
            try
            {
                _targetEntity = target as GameEntity;

                var moduleTypes = new HashSet<Type>();

                // Поиск в Assembly-CSharp
                var assemblyCSharp = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(assembly => assembly.GetName().Name == "Assembly-CSharp");

                if (assemblyCSharp != null)
                {
                    var csharpModules = assemblyCSharp.GetTypes()
                        .Where(t => typeof(ModuleBase).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
                    foreach (var type in csharpModules)
                    {
                        moduleTypes.Add(type);
                    }
                }

                // Поиск в текущей сборке ModularEventArchitecture
                var currentAssembly = Assembly.GetAssembly(typeof(ModuleBase));
                if (currentAssembly != null)
                {
                    var assemblyModules = currentAssembly.GetTypes()
                        .Where(t => typeof(ModuleBase).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
                    foreach (var type in assemblyModules)
                    {
                        moduleTypes.Add(type);
                    }
                }

                // Фильтруем модули с учетом обоих атрибутов
                availableModules = moduleTypes.Where(moduleType =>
                {
                    try
                    {
                        // Проверяем исключения
                        var incompatibleAttributes = moduleType.GetCustomAttributes<IncompatibleUnitAttribute>();
                        if (incompatibleAttributes == null || incompatibleAttributes.Any(attr => attr.UnitType.IsAssignableFrom(_targetEntity.GetType())))
                        {
                            return false; // Модуль исключен для данного типа
                        }

                        // Проверяем совместимость
                        var compatibleAttributes = moduleType.GetCustomAttributes<CompatibleUnitAttribute>();
                        
                        // Если нет атрибутов совместимости, считаем модуль совместимым
                        if (compatibleAttributes == null || !compatibleAttributes.Any())
                        {
                            return true;
                        }

                        return compatibleAttributes.Any(attr => attr.UnitType.IsAssignableFrom(_targetEntity.GetType()));
                    }
                    catch (NullReferenceException)
                    {
                        // Если произошла ошибка при проверке атрибутов, считаем модуль совместимым
                        Debug.LogWarning($"Null reference while checking attributes for module {moduleType.Name}");
                        return true;
                    }
                }).ToList();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error in GameEntityEditor.OnEnable: {e}");
                availableModules = new List<Type>();
            }
        }

        public override VisualElement CreateInspectorGUI()
        {
            try
            {
                VisualElement root = new VisualElement();

                if (treeAsset != null)
                {
                    treeAsset.CloneTree(root);

                    _ADDModule = root.Q<Button>("Button_ADDMondule");
                    _modulesContainer = root.Q<VisualElement>("ModulesContainer");

                    if (_ADDModule != null)
                    {
                        _ADDModule.clicked += ToggleModuleMenu;
                    }
                }
                else
                {
                    Debug.LogWarning("GameEntityEditor: treeAsset is null");
                }

                // Add default inspector properties
                InspectorElement.FillDefaultInspector(root, serializedObject, this);

                return root;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error in GameEntityEditor.CreateInspectorGUI: {e}");
                return new VisualElement();
            }
        }

        private void ToggleModuleMenu()
        {
            _isMenuOpen = !_isMenuOpen;
            
            if (_isMenuOpen)
            {
                if (_modulesContainer != null)
                {
                    _modulesContainer.style.display = DisplayStyle.Flex;
                    CreateModuleButtons();
                }
            }
            else
            {
                if (_modulesContainer != null)
                {
                    _modulesContainer.style.display = DisplayStyle.None;
                    _modulesContainer.Clear();
                }
            }
        }

        private void CreateModuleButtons()
        {
            if (_modulesContainer == null) return;

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

            // newModule.Setup(_targetEntity);

            _targetEntity.AddModule(newModule);

            // После добавления модуля, закрываем меню
            ToggleModuleMenu();
            
            // Обновляем инспектор
            EditorUtility.SetDirty(target);
        }
    }
}
#endif
