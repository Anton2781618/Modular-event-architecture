using System;
using UnityEngine;
using UnityEngine.AI;

namespace ModularEventArchitecture
{
    [CompatibleUnit(typeof(NPC))]
    public sealed class NpcMovementModule : ModuleBase
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;
        private NPCMovementSystem _nPCMovementSystem;
        private MovementTarget _currentTarget;

        private bool isStun = false;

        protected override void Initialize()
        {
            base.Initialize();
            
            if (!_agent) _agent = GetComponent<NavMeshAgent>();
            
            if (!_animator) _animator = GetComponent<Animator>();
            
            
            _nPCMovementSystem = new NPCMovementSystem(_animator, _agent);

            
            LocalEvents.Subscribe<MoveToTargetEvent>(LocalEventBus.События.Команды.Движение.Двигаться_к_таргету , SetTarget);
            
            LocalEvents.Subscribe<MoveToPointEvent>(LocalEventBus.События.Команды.Движение.Двигаться_к_точке , SetPoint);
            
            LocalEvents.Subscribe<BaseEvent>(LocalEventBus.События.Команды.Движение.Остановиться , StopMoving);

            LocalEvents.Subscribe<StunEvent>(LocalEventBus.События.Оглушен , Unconscious);
        }

        private void Unconscious(StunEvent @event) => isStun = @event.isStun;



        public override void UpdateMe()
        {
            if (_currentTarget == null || isStun) return;
            
            _nPCMovementSystem.Move(_currentTarget.GetPosition());
        }

        private void SetPoint(MoveToPointEvent data)
        {
            _currentTarget = new PointTarget(data.Point);
        }

        private void SetTarget(MoveToTargetEvent data)
        {
            _currentTarget = new TransformTarget(data.target);
        }

        private void StopMoving(BaseEvent data) => _currentTarget = null;


        //!--------------------------вспомогательные классы---------------------

        public abstract class MovementTarget
        {
            public abstract Vector3 GetPosition();
        }

        public class PointTarget : MovementTarget
        {
            private Vector3 _point;

            public PointTarget(Vector3 point)
            {
                _point = point;
            }

            public override Vector3 GetPosition()
            {
                return _point;
            }
        }

        public class TransformTarget : MovementTarget
        {
            private Transform _transform;

            public TransformTarget(Transform transform)
            {
                _transform = transform;
            }

            public override Vector3 GetPosition()
            {
                return _transform.position;
            }
        }
        //!----------------------------------------------------------------
    }
}