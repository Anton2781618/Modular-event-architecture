#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ModularEventArchitecture
{
    [CustomPropertyDrawer(typeof(DropAttribute))]
    public class DropAttributeDrawer : PropertyDrawer
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
            DropAttribute popupAttribute = attribute as DropAttribute;
            
            if (Asset != null && Asset.HierarchyTags != null)
            {
                // Получаем текущий индекс из property
                int currentIndex =  Asset.HierarchyTags.IndexOf(property.stringValue);
                if (currentIndex == -1) currentIndex = 0;
                
                // Отрисовываем popup и получаем новый индекс
                int newIndex = EditorGUI.Popup(position, label.text, currentIndex, Asset.HierarchyTags.ToArray());
                
                // Если индекс изменился, обновляем значение в property
                if (newIndex != currentIndex)
                {
                    property.stringValue = Asset.HierarchyTags[newIndex];
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
            
            EditorGUI.EndProperty();
        }
    }
}
#endif