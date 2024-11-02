using System;
using UnityEngine;

[Serializable]
public struct EffectEventData
{
    [Tooltip("Тип события")]
    [SerializeField] public LocalEventType eventType;
    [SerializeReference] private IEventData eventData;
    public int EventId => (int)eventType;
    public IEventData GetEventData() => eventData;
    public LocalEventType EventType => eventType;
}

// Структура, напрямую связанная с LocalEventBus.События
[Serializable]
public enum LocalEventType
{
    [InspectorName("Получение урона")]
    ПолучитьУрон = LocalEventBus.События.Получить_урон,
    
    [InspectorName("Оглушение")]
    Оглушение = LocalEventBus.События.Оглушен,
    
    [InspectorName("Изменение выносливости")]
    ИзменениеВыносливости = LocalEventBus.События.Выносливость_изменилась,
    
    [InspectorName("Изменение здоровья")]
    ИзменениеЗдоровья = LocalEventBus.События.Здоровье_изменилась,
    
    // Движение
    [InspectorName("Двигаться к точке")]
    ДвигатьсяКТочке = LocalEventBus.События.Команды.Движение.Двигаться_к_точке,
    
    [InspectorName("Двигаться к цели")]
    ДвигатьсяКЦели = LocalEventBus.События.Команды.Движение.Двигаться_к_таргету,
    
    [InspectorName("Остановиться")]
    Остановиться = LocalEventBus.События.Команды.Движение.Остановиться,
    
    [InspectorName("Начать спринт")]
    НачатьСпринт = LocalEventBus.События.Команды.Движение.Начать_спринт,
    
    [InspectorName("Закончить спринт")]
    ЗакончитьСпринт = LocalEventBus.События.Команды.Движение.Закончить_спринт,
    
    // Бой
    [InspectorName("Атаковать цель")]
    АтаковатьЦель = LocalEventBus.События.Команды.Бой.Атакавать_цель,
    
    [InspectorName("Прекратить бой")]
    ПрекратитьБой = LocalEventBus.События.Команды.Бой.Перестать_сражаться,
    
    // UI
    [InspectorName("Обновить UI")]
    ОбновитьUI = LocalEventBus.События.Команды.UI.Обновить_UI,
    
    // Эффекты
    [InspectorName("Эффект создан")]
    ЭффектСоздан = LocalEventBus.События.Состояния.Эффекты.Эффект_создан,
    
    [InspectorName("Усталость")]
    Усталость = LocalEventBus.События.Состояния.Усталость
}
