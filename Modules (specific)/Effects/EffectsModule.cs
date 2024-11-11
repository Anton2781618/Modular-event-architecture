using System;
using UnityEngine;

namespace ModularEventArchitecture
{
    using static Effect;
    [CompatibleUnit(typeof(UnitEntity))]
    public class EffectsModule : ModuleBase
    {    
        [SerializeField] private Effect[] _effectsArray = new Effect[64]; // Фиксированный массив для эффектов
        private int _effectsCount = 0; // Текущее количество активных эффектов
        
        protected override void Initialize()
        {
            base.Initialize();
            
            // Подписываемся на события эффектов
            LocalEvents.Subscribe<EffectEvent>(LocalEventBus.События.Состояния.Эффекты.Эффект_создан, ApplyEffect);

            LocalEvents.Subscribe<EffectEvent>(LocalEventBus.События.Состояния.Эффекты.Эффект_снять, RemoveEffect);
        }

        private void ApplyEffect(EffectEvent effectEvent)
        {
            var effect = effectEvent.Effect;
            
            // Проверяем, есть ли уже такой эффект
            int index = FindEffectIndex(effect.Name);
            
            if (index != -1)
            {
                // Обновляем существующий эффект
                ref Effect existingEffect = ref _effectsArray[index];
                
                if (effect.Duration > 0)
                {
                    existingEffect.Duration = effect.Duration;
                }
                
                existingEffect.Magnitude = effect.Magnitude;
            }
            else
            {
                // Добавляем новый эффект
                if (_effectsCount < _effectsArray.Length)
                {
                    _effectsArray[_effectsCount] = effect;
                    
                    // Вызываем события применения
                    ProcessEffectEvents(effect.OnApplyEvents);
                    
                    _effectsCount++;
                }
            }
        }

        //снять эффект событием
        private void RemoveEffect(EffectEvent effectEvent)
        {
            var effect = effectEvent.Effect;
            
            // Проверяем, есть ли уже такой эффект
            int index = FindEffectIndex(effect.Name);
            
            if (index != -1)
            {
                RemoveEffectAt(index);
            }
        }

        public override void UpdateMe()
        {
            for (int i = _effectsCount - 1; i >= 0; i--)
            {
                ref Effect effect = ref _effectsArray[i];
                
                // Обновляем время действия эффекта
                if (!effect.IsPermanent)
                {
                    effect.Duration -= Time.deltaTime;

                    effect.ElapsedTime += Time.deltaTime;
                    
                    // Проверяем закончился ли эффект
                    if (effect.Duration <= 0)
                    {
                        RemoveEffectAt(i);
                        continue;
                    }
                }

                // Проверяем тики эффекта
                if (effect.UpdateTickTimer(Time.deltaTime))
                {
                    ProcessEffectEvents(effect.OnTickEvents);
                }
            }
        }

        private void RemoveEffectAt(int index)
        {
            if (index < 0 || index >= _effectsCount) return;

            // Вызываем события удаления перед удалением эффекта
            ProcessEffectEvents(_effectsArray[index].OnRemoveEvents);
            
            // Сдвигаем все последующие элементы на одну позицию влево
            for (int i = index; i < _effectsCount - 1; i++)
            {
                _effectsArray[i] = _effectsArray[i + 1];
            }
            
            _effectsCount--;
        }

        private void ProcessEffectEvents(EffectEventData[] events)
        {
            if (events == null) return;
            
            foreach (var eventData in events)
            {
                // Публикуем событие с данными
                LocalEvents.Publish(eventData.EventId, eventData.GetEventData());
            }
        }

        private int FindEffectIndex(EffectName effectName)
        {
            for (int i = 0; i < _effectsCount; i++)
            {
                if (_effectsArray[i].Name == effectName)
                {
                    return i;
                }
            }
            return -1;
        }

        // Вспомогательные методы для работы с эффектами
        public bool HasEffect(EffectName effectName)
        {
            return FindEffectIndex(effectName) != -1;
        }

        public ref Effect GetEffect(EffectName effectName)
        {
            int index = FindEffectIndex(effectName);
            if (index != -1)
            {
                return ref _effectsArray[index];
            }
            throw new InvalidOperationException($"Effect {effectName} not found");
        }

        public void RemoveAllEffects()
        {
            for (int i = _effectsCount - 1; i >= 0; i--)
            {
                RemoveEffectAt(i);
            }
        }

        // Метод для доступа к эффектам как к коллекции
        public Span<Effect> GetActiveEffects()
        {
            return new Span<Effect>(_effectsArray, 0, _effectsCount);
        }
    }
}

[Serializable]
    public class EffectEvent : ModularEventArchitecture.BaseEvent
    {
        public ModularEventArchitecture.Effect Effect;
    }