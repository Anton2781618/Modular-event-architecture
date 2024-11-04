using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ModularEventArchitecture
{
    [CustomPropertyDrawer(typeof(EffectEventData))]
    public class EffectEventDataDrawer : PropertyDrawer
    {
        // Словарь для хранения предыдущих значений для каждого свойства
        private static Dictionary<string, LocalEventType> previousValues = new Dictionary<string, LocalEventType>();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            // Получаем ссылки на свойства
            var eventType = property.FindPropertyRelative("eventType");
            var eventData = property.FindPropertyRelative("eventData");

            // Уникальный ключ для этого свойства
            string propertyKey = property.propertyPath;

            // Рассчитываем высоту для строки
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            
            // Получаем текущее значение типа события напрямую как enum
            var currentEventType = (LocalEventType)System.Enum.ToObject(typeof(LocalEventType), eventType.intValue);
            
            // Получаем предыдущее значение или используем текущее, если его нет
            if (!previousValues.ContainsKey(propertyKey))
            {
                previousValues[propertyKey] = currentEventType;
            }

            // Рисуем поле выбора типа события
            Rect eventTypeRect = new Rect(position.x, position.y, position.width, lineHeight);
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(eventTypeRect, eventType);
            bool eventTypeChanged = EditorGUI.EndChangeCheck();

            // Если тип события изменился
            if (eventTypeChanged)
            {
                currentEventType = (LocalEventType)System.Enum.ToObject(typeof(LocalEventType), eventType.intValue);
                
                var newEventData = CreateEventData(currentEventType);
                eventData.managedReferenceValue = newEventData;
                
                previousValues[propertyKey] = currentEventType;
                
                // Помечаем свойство как измененное
                property.serializedObject.ApplyModifiedProperties();
            }

            // Если есть данные события, отображаем их
            if (eventData.managedReferenceValue != null)
            {
                Rect dataFieldRect = new Rect(position.x, position.y + lineHeight + spacing, position.width, EditorGUI.GetPropertyHeight(eventData));
                
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(dataFieldRect, eventData, true);
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var eventData = property.FindPropertyRelative("eventData");
            float baseHeight = EditorGUIUtility.singleLineHeight;

            if (eventData.managedReferenceValue != null)
            {
                baseHeight += EditorGUIUtility.standardVerticalSpacing + 
                            EditorGUI.GetPropertyHeight(eventData);
            }

            return baseHeight;
        }

        private IEventData CreateEventData(LocalEventType eventType)
        {
            return eventType switch
            {
                LocalEventType.ПолучитьУрон => new DamageEvent(),
                LocalEventType.Оглушение => new StunEvent(),
                LocalEventType.ДвигатьсяКТочке => new MoveToPointEvent(),
                LocalEventType.ДвигатьсяКЦели => new MoveToTargetEvent(),
                LocalEventType.ИзменениеВыносливости => new StaminaChangedEvent(),
                LocalEventType.ИзменениеЗдоровья => new HealthChangedEvent(),
                LocalEventType.ЭффектСоздан => new EffectEvent(),
                LocalEventType.АтаковатьЦель => new AttackEvent(),
                LocalEventType.ПрекратитьБой => new StopFightEvent(),
                _ => new BaseEvent()
            };
        }
    }
}