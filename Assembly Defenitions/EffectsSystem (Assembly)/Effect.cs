using System;
using UnityEngine;

[Serializable]
public struct Effect
{
    
    [Header("Основные свойства")]

    [Tooltip("Иконка эффекта")]
    public Sprite icon;
    
    [TextArea(3, 5)] [Tooltip("Описание эффекта")]
    public string description;
    [Tooltip("Название эффекта")]
    public EffectName Name;
    
    // Параметры времени
    [Tooltip("Длительность эффекта в секундах")]
    public float Duration;

    [Tooltip("как часто сробатывают эффекты установленые в OnTickEvents(например, урон каждую секунду)")]
    public float TickRate;   // Как часто срабатывает эффект (для DOT эффектов)
    
    [HideInInspector] [Tooltip("Прошедшее время с начала действия эффекта")]
    public float ElapsedTime;

    [Tooltip("Этот флаг делает эффект постоянным, который не исчезает по истечении времени, а снимается вручную")]
    public bool IsPermanent;
    
    // Параметры силы эффекта
    [Tooltip("Сила эффекта (например, замедление на 50%)")]
    public float Magnitude;  // Сила эффекта (например, замедление на 50%)
    
    // События эффекта
    public EffectEventData[] OnApplyEvents;
    public EffectEventData[] OnTickEvents;
    public EffectEventData[] OnRemoveEvents;
    
    public float tickTimer;
    
    // Источник эффекта (может быть полезно для снятия всех эффектов от конкретного источника)
    public object Source { get; set; }

    public enum EffectName
    {
        Tired,
        Stunned,
        Poisoned,
        Burning,
        Rope
    }


    // Обновление таймера тика
    public bool UpdateTickTimer(float deltaTime)
    {
        
        if (TickRate <= 0) return false;
        
        tickTimer += deltaTime;
        
        if (tickTimer >= TickRate)
        {
            tickTimer = 0;
            return true;
        }
        return false;
    }
}
