#if UNITY_EDITOR
using ModularEventArchitecture;
using UnityEditor;
using UnityEngine;

namespace MyScripts.Architecture.Attributes.PopupDrawer
{
    [CustomPropertyDrawer(typeof(PopupAttribute))]
    public class PopupDrawer : PropertyDrawer
    {
        private TagsBuilder _asset;
        private TagsBuilder Asset
        {
            get
            {
                if (_asset == null)
                {
                    string[] guids = AssetDatabase.FindAssets("t:TagsBuilder");
                    if (guids.Length > 0)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                        _asset = AssetDatabase.LoadAssetAtPath<TagsBuilder>(path);
                    }
                }
                return _asset;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Получаем атрибут
            PopupAttribute popupAttribute = attribute as PopupAttribute;
            
            if (Asset != null && Asset.HierarchyItemsTypes != null)
            {
                // Получаем текущий индекс из property
                int currentIndex =  Asset.HierarchyItemsTypes.IndexOf(property.stringValue);
                if (currentIndex == -1) currentIndex = 0;
                
                // Отрисовываем popup и получаем новый индекс
                int newIndex = EditorGUI.Popup(position, label.text, currentIndex, Asset.HierarchyItemsTypes.ToArray());
                
                // Если индекс изменился, обновляем значение в property
                if (newIndex != currentIndex)
                {
                    property.stringValue = Asset.HierarchyItemsTypes[newIndex];
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
            
            EditorGUI.EndProperty();
        }
    }
}
#endif