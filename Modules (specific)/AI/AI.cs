using UnityEngine;
using UnityEngine.AI;

namespace ModularEventArchitecture
{
    [CompatibleUnit(typeof(NPC))]
    public class AI : ModuleBase
    {
        [SerializeField] private bool _agressive = true;
        private Transform _myTransform {get; set;}
        private NavMeshAgent _agent {get; set;}
        private Transform _target;
        [SerializeField] private State _currentState = State.Idle;
        [SerializeField] private float _detectionRange = 10f;

        public enum State
        {
            Idle,
            Patrol,
            Chase,
            Attack
        }

        protected override void Initialize()
        {
            base.Initialize();
            
            if (!_agent) _agent = Entity.GetCachedComponent<NavMeshAgent>();
            
            if (!_myTransform) _myTransform = transform;
            
            _myTransform = transform;

            _target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public override void UpdateMe()
        {
            // Debug.Log("UpdateMe");
            
            switch (_currentState)
            {
                case State.Idle:
                    Idle();
                    break;
                // case State.Patrol:
                //     Patrol();
                //     break;
                case State.Chase:
                    Chase();
                    break;
                case State.Attack:
                    Attack();
                    break;
            }
        }

        private void Idle()
        {
            // Debug.Log("Idle " + agressive + " ! " + DetectPlayer());
            if (DetectPlayer() && _agressive)
            {
                LocalEvents.Publish<MoveToTargetEvent>(LocalEventBus.События.Команды.Движение.Двигаться_к_таргету, new MoveToTargetEvent { target = _target });

                _currentState = State.Chase;
            }
        }

        private void Patrol()
        {
            Vector3 randomDirection = Random.insideUnitSphere * 5f;
            randomDirection.y = 0;
            Vector3 targetPosition = _myTransform.transform.position + randomDirection;


            if (DetectPlayer() && _agressive)
            {
                _currentState = State.Chase;
            }
            else if (Random.value < 0.02f)
            {
                _currentState = State.Idle;
            }
        }

        private void Chase()
        {
            // Debug.Log("Chase");

            if (Vector3.Distance(_myTransform.position, _target.position) <= _agent.stoppingDistance)
            {
                LocalEvents.Publish(LocalEventBus.События.Команды.Бой.Атакавать_цель, new AttackEvent { Unit = _target.gameObject });
                _currentState = State.Attack;
            }
            else 
            if (!DetectPlayer())
            {
                LocalEvents.Publish(LocalEventBus.События.Команды.Бой.Перестать_сражаться, new StopFightEvent()); 

                LocalEvents.Publish(LocalEventBus.События.Команды.Движение.Остановиться, new BaseEvent());

                _currentState = State.Idle;
            }
        }


        private void Attack()
        {
            if (Vector3.Distance(_myTransform.position, _target.position) > _agent.stoppingDistance)
            {
                LocalEvents.Publish<MoveToTargetEvent>(LocalEventBus.События.Команды.Движение.Двигаться_к_таргету, new MoveToTargetEvent { target = _target });

                _currentState = State.Chase;
            }
        }

        private bool DetectPlayer()
        {
            return Vector3.Distance(_myTransform.position, _target.position) <= _detectionRange;
        }
    }
}