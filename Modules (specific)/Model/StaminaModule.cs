using UnityEngine;

namespace ModularEventArchitecture
{
    [CompatibleUnit(typeof(UnitEntity))]
    public class StaminaModule : ModuleBase
    {
        [Tooltip("Максимальная выносливость")]
        [SerializeField] private float _maxStamina = 100;
        
        [Tooltip("Текущая выносливость")]
        [SerializeField] private float _currentStamina = 100;

        [Tooltip("Регенерация выносливости")]
        [SerializeField] private float _regenerationStamina = 0.3f;

        [Tooltip("Шаблон эффекта усталости")]
        [SerializeField] private EffectTemplate _tiredEffectTemplate;

        private const int STAMINA_UPDATE_THRESHOLD = 5; // Порог для отправки события
        private float _lastReportedStamina;
        private bool _isSprinting = false;
        private bool _isNotAccumulate = false;
        private bool _isTiredEffect = false;

        protected override void Initialize()
        {
            base.Initialize();
            
            LocalEvents.Subscribe<BaseEvent>(LocalEventBus.События.Команды.Движение.Начать_спринт, StartSprint);
            
            LocalEvents.Subscribe<BaseEvent>(LocalEventBus.События.Команды.Движение.Закончить_спринт, StopSprint);

            LocalEvents.Subscribe<BaseEvent>(LocalEventBus.События.Состояния.Усталость, CheckEffect);
        }

        public override void UpdateMe() => UpdateStamina();

        private void UpdateStamina()
        {
            if (_isSprinting && !_isTiredEffect)
            {
                ChangeStamina(-1);
            }
            else
            {
                if(!_isNotAccumulate && _currentStamina < _maxStamina)
                {
                    ChangeStamina(_regenerationStamina);
                }
            }

            _isNotAccumulate = false;
        }

        private void CheckEffect(BaseEvent data)
        {
            _isTiredEffect = data.Enabled;
            
            // else if (data.Effect.Name == Effect.EffectName.Rope)
            // {
            //     _isNotAccumulate = true;
            //     ChangeStamina(-1);
            // } 
        }

        private void StartSprint(BaseEvent data)
        {
            if (!_isSprinting)
            {
                _isSprinting = true;
            }
        }

        private void StopSprint(BaseEvent data)
        {
            if (_isSprinting)
            {
                _isSprinting = false;
            }
        }

        private void ChangeStamina(float amount)
        {
            float oldStamina = _currentStamina;
        
            _currentStamina = Mathf.Clamp(_currentStamina + amount, 0, _maxStamina);
            
            // Отправляем событие только если изменение достигло порога или достигнут минимум/максимум
            if (Mathf.Abs(_currentStamina - _lastReportedStamina) >= STAMINA_UPDATE_THRESHOLD || /* _currentStamina == 0 || _currentStamina == _maxStamina || */ _currentStamina != oldStamina)
            {
                _lastReportedStamina = _currentStamina;

                LocalEvents.Publish(LocalEventBus.События.Выносливость_изменилась, new StaminaChangedEvent { CurrentStamina = _currentStamina, MaxStamina = _maxStamina});

                if (_currentStamina >= _maxStamina)
                {
                    //снять эффект усталости
                    LocalEvents.Publish(LocalEventBus.События.Состояния.Эффекты.Эффект_снять, new EffectEvent { Effect = _tiredEffectTemplate.Effects });
                }
                else        
                if (_currentStamina <= 0 && _isTiredEffect == false)
                {
                    // Создаем эффект который наложет состояние усталости
                    LocalEvents.Publish(LocalEventBus.События.Состояния.Эффекты.Эффект_создан, new EffectEvent { Effect = _tiredEffectTemplate.Effects });

                    LocalEvents.Publish(LocalEventBus.События.Команды.Движение.Закончить_спринт, new BaseEvent());
                }
            }
            
        }

        [ContextMenu("asdsdasds")]
        public void SetMaxStamina()
        {
            LocalEvents.Publish(LocalEventBus.События.Состояния.Эффекты.Эффект_создан, new EffectEvent { Effect = _tiredEffectTemplate.Effects });
        }
    }
}