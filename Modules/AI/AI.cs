using UnityEngine;
using UnityEngine.AI;

namespace ModularEventArchitecture
{
    [CompatibleUnit(typeof(NPC))]
    public class AI : ModuleBase
    {
        [SerializeField] private bool agressive = true;
        public Transform MyTransform {get; set;}
        public NavMeshAgent Agent {get; set;}
        [SerializeField] private Transform target;
        [SerializeField] private State currentState = State.Idle;

        public float detectionRange = 10f;

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
            
            if (!Agent) Agent = GetComponent<NavMeshAgent>();
            if (!MyTransform) MyTransform = transform;
            
            MyTransform = transform;

            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public override void UpdateMe()
        {
            // Debug.Log("UpdateMe");
            
            switch (currentState)
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
            if (DetectPlayer() && agressive)
            {
                LocalEvents.Publish<MoveToTargetEvent>(LocalEventBus.События.Команды.Движение.Двигаться_к_таргету, new MoveToTargetEvent { target = target });

                currentState = State.Chase;
            }
        }

        private void Patrol()
        {
            Vector3 randomDirection = Random.insideUnitSphere * 5f;
            randomDirection.y = 0;
            Vector3 targetPosition = MyTransform.transform.position + randomDirection;


            if (DetectPlayer() && agressive)
            {
                currentState = State.Chase;
            }
            else if (Random.value < 0.02f)
            {
                currentState = State.Idle;
            }
        }

        private void Chase()
        {
            // Debug.Log("Chase");

            if (Vector3.Distance(MyTransform.position, target.position) <= Agent.stoppingDistance)
            {
                LocalEvents.Publish(LocalEventBus.События.Команды.Бой.Атакавать_цель, new AttackEvent { Unit = target.gameObject });
                currentState = State.Attack;
            }
            else 
            if (!DetectPlayer())
            {
                LocalEvents.Publish(LocalEventBus.События.Команды.Бой.Перестать_сражаться, new StopFightEvent()); 

                LocalEvents.Publish(LocalEventBus.События.Команды.Движение.Остановиться, new BaseEvent());

                currentState = State.Idle;
            }
        }


        private void Attack()
        {
            if (Vector3.Distance(MyTransform.position, target.position) > Agent.stoppingDistance)
            {
                LocalEvents.Publish<MoveToTargetEvent>(LocalEventBus.События.Команды.Движение.Двигаться_к_таргету, new MoveToTargetEvent { target = target });

                currentState = State.Chase;
            }
        }

        private bool DetectPlayer()
        {
            return Vector3.Distance(MyTransform.position, target.position) <= detectionRange;
        }
    }
}