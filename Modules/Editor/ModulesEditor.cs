#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Linq;
using Tools;
using System.Drawing;

[CustomEditor(typeof(ModuleBase), true)]
public class ModulesEditor : Editor
{
    public VisualTreeAsset treeAsset;
    private VisualElement _buttonsContainer;
    
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        treeAsset.CloneTree(root);

        _buttonsContainer = root.Q<VisualElement>("ButtonsContainer");

        // Add default inspector properties
        InspectorElement.FillDefaultInspector(root, serializedObject, this);

        // Add buttons for methods with ButtonAttribute
        var methods = target.GetType().GetMethods().Where(m => m.GetParameters().Length == 0 && m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0);

        foreach (var method in methods)
        {
            var buttonAttribute = (ButtonAttribute)method.GetCustomAttributes(typeof(ButtonAttribute), true)[0];

            // Create button
            var button = new Button(() =>
            {
                foreach (var targetObject in targets)
                {
                    method.Invoke(targetObject, null);
                }
            })
            {
                // Use custom button name if provided, otherwise use method name
                text = buttonAttribute.buttonName ?? ObjectNames.NicifyVariableName(method.Name)
            };

            // Set button enabled state
            bool isEnabled = buttonAttribute.mode == ButtonMode.AlwaysEnabled || 
            (EditorApplication.isPlaying ? buttonAttribute.mode == ButtonMode.EnabledInPlayMode : buttonAttribute.mode == ButtonMode.DisabledInPlayMode);
            
            button.SetEnabled(isEnabled);

            // Add some margin
            button.style.marginTop = 5;
            button.style.marginBottom = 5;

            _buttonsContainer.Add(button);
            // root.Add(button);
        }

        return root;
    }
}
#endif
