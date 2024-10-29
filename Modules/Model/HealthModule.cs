using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[CompatibleUnit(typeof(GameEntity))]
[IncompatibleUnit(typeof(LevelManager))]
public sealed class HealthModule : ModuleBase, IStatus
{

    [Tools.Information("Этот модуль представляет из себя систему здоровья персонажа",Tools.InformationAttribute.InformationType.Info,false)]
    [Tooltip("Максимальное здоровье")]
    [SerializeField] private float _maxHealth = 100;

    [Tooltip("Текущее здоровье")]
    [SerializeField] private float _currentHealth = 100;

    [Tooltip("Части тела")]
    [SerializeField] private Rigidbody[] bodyParts;
    
    [Tooltip("Вектор удара")]
    private Vector3 vectorHit = Vector3.zero;

    [Tooltip("Сила отталкивания при смерти")]
    private float force = 100;

    private bool _isDead = false;
    private NavMeshAgent _agent;
    private Animator _animator;
    
    protected override void Initialize()
    {
        base.Initialize();
        
        if (!_agent) TryGetComponent(out _agent);
        
        if (!_agent) _agent = GetComponent<NavMeshAgent>();
        
        if (!_animator) _animator = GetComponent<Animator>();

        if (bodyParts.Length == 0) FindBodyParts();        
        
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
        vectorHit = data.HitDirection;
        Debug.Log(vectorHit);

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

        _animator.GetBoneTransform(HumanBodyBones.Head).GetComponent<Rigidbody>().AddForce(vectorHit * force, ForceMode.Impulse);

        GlobalEventBus.Instance.Publish(GlobalEventBus.События.Юнит_погиб, new DieEvent{ Unit = Character });
    }

    public bool IsDead() => _isDead;

    [ContextMenu("Найти все части тела")]
    public void FindBodyParts() => bodyParts = GetComponentsInChildren<Rigidbody>();

    [ContextMenu("Включить кинематику на всех частях тела")]
    public void SetOnKinematicBody() 
    {
        foreach (var item in bodyParts)
        {
            item.isKinematic = true;
        }
    }

    [ContextMenu("Выключить кинематику на всех частях тела")]
    public void SetOffKinematicBody()
    {
        foreach (var item in bodyParts)
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