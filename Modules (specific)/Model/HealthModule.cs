using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace ModularEventArchitecture
{
    [CompatibleUnit(typeof(UnitEntity))]
    public sealed class HealthModule : ModuleBase, IStatus
    {

        [Tools.Information("Этот модуль представляет из себя систему здоровья персонажа",Tools.InformationAttribute.InformationType.Info,false)]
        [Tooltip("Максимальное здоровье")]
        [SerializeField] private float _maxHealth = 100;

        [Tooltip("Текущее здоровье")]
        [SerializeField] private float _currentHealth = 100;

        [Tooltip("Части тела")]
        [SerializeField] private Rigidbody[] _bodyParts;
        
        [Tooltip("Вектор удара")]
        private Vector3 _vectorHit = Vector3.zero;

        [Tooltip("Сила отталкивания при смерти")]
        private float _force = 100;

        private bool _isDead = false;
        private NavMeshAgent _agent;
        private Animator _animator;
        
        protected override void Initialize()
        {
            base.Initialize();
            
            if (!_agent) Entity.GetCachedComponent<NavMeshAgent>();
            
            if (!_animator) _animator = Entity.GetCachedComponent<Animator>();
            
            LocalEvents.Subscribe<DamageEvent>(LocalEventBus.События.Получить_урон , TakeDamage);

            LocalEvents.Subscribe<DieEvent>(LocalEventBus.События.Юнит_погиб , Die);

            LocalEvents.Subscribe<StunEvent>(LocalEventBus.События.Оглушен , Unconscious);

        }

        public override void UpdateMe()
        {
        }

        //метод потерять сознание
        public void Unconscious(StunEvent data)
        {
            if (data.isStun) StartCoroutine(RegainConsciousness(data.Time));
        }

        //курутина на 20 секунд восстановления сознания
        private IEnumerator RegainConsciousness(float time = 20)
        {
            yield return new WaitForSeconds(time);

            LocalEvents.Publish(LocalEventBus.События.Оглушен, new StunEvent {isStun = false, Time = 0});
        }

        private void TakeDamage(DamageEvent data)
        {
            _vectorHit = data.HitDirection;
            Debug.Log(_vectorHit);

            ChangeHealth(-data.Damage);
        }

        public void ChangeHealth(float value)
        {
            _currentHealth += value;

            LocalEvents.Publish(LocalEventBus.События.Здоровье_изменилась, new HealthChangedEvent { CurrentHealth = _currentHealth, MaxHealth = _maxHealth });

            if (_currentHealth <= 0)
            {
                LocalEvents.Publish<DieEvent>(LocalEventBus.События.Юнит_погиб, new DieEvent());

                Die(new DieEvent());
            }
        }    
        
        public void Die(DieEvent data)
        {
            _isDead = true;

            _animator.enabled = false;
            
            if(_agent) _agent.enabled = false;

            SetOffKinematicBody();

            _animator.GetBoneTransform(HumanBodyBones.Head).GetComponent<Rigidbody>().AddForce(_vectorHit * _force, ForceMode.Impulse);

            GlobalEventBus.Instance.Publish(GlobalEventBus.События.Юнит_погиб, new DieEvent{ Unit = Entity });
        }

        public bool IsDead() => _isDead;

        [ContextMenu("Найти все части тела")]
        [Tools.Button("Найти все части тела")]
        public void FindBodyParts() => _bodyParts = GetComponentsInChildren<Rigidbody>();

        [ContextMenu("Включить кинематику на всех частях тела")]
        public void SetOnKinematicBody() 
        {
            foreach (var item in _bodyParts)
            {
                item.isKinematic = true;
            }
        }

        [ContextMenu("Выключить кинематику на всех частях тела")]
        public void SetOffKinematicBody()
        {
            if(_bodyParts == null || _bodyParts.Length == 0) FindBodyParts();
            
            foreach (var item in _bodyParts)
            {
                item.isKinematic = false;
                item.useGravity = true;
            }
        }    
    }

    internal interface IStatus
    {
        public bool IsDead();
    }
}