#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(ModuleBase), true)]
public class ModulesEditor : Editor
{
    public VisualTreeAsset treeAsset;
    
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        treeAsset.CloneTree(root);

        // Add default inspector properties
        InspectorElement.FillDefaultInspector(root, serializedObject, this);

        return root;
    }
}
#endif