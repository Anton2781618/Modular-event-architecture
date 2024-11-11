using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace ModularEventArchitecture
{
    [CompatibleUnit(typeof(NPC))]
    public sealed class CombatModule : ModuleBase
    {
        private NavMeshAgent _agent;
        private Animator _animator;
        [SerializeField] private GameObject _target;
        [SerializeField] private Weapon _weapon;

        private CombatSystem combatSystem;

        protected override void Initialize()
        {
            base.Initialize();
            
            if (!_agent) _agent = Entity.GetCachedComponent<NavMeshAgent>();

            if (!_animator) _animator = Entity.GetCachedComponent<Animator>();

            combatSystem = new CombatSystem(_animator, _agent);

            LocalEvents.Subscribe<AttackEvent>(LocalEventBus.События.Команды.Бой.Атакавать_цель, SetTarget);

            LocalEvents.Subscribe<StopFightEvent>(LocalEventBus.События.Команды.Бой.Перестать_сражаться, StopFight);
        }

        private void AttackTrget() => combatSystem.Attack(_target);

        public override void UpdateMe()
        {
            if (_target) AttackTrget();
        }

        private void SetTarget(AttackEvent obj) => _target = obj.Unit;
        private void StopFight(StopFightEvent obj) => _target = null;

        public void SetHitBoolOFF()
        {
            if(_weapon)_weapon.SetHitBoolOFF();
        }
        
    }
}