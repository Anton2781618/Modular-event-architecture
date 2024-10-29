using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CompatibleUnit(typeof(NPC))]
public sealed class CombatModule : ModuleBase
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject target;
    [SerializeField] private Weapon weapon;

    private CombatSystem combatSystem;

    protected override void Initialize()
    {
        if (!_agent) _agent = GetComponent<NavMeshAgent>();
        if (!_animator) _animator = GetComponent<Animator>();

        combatSystem = new CombatSystem(_animator, _agent);

        LocalEvents.Subscribe<AttackEvent>(LocalEventBus.События.Команды.Бой.Атакавать_цель, SetTarget);

        LocalEvents.Subscribe<StopFightEvent>(LocalEventBus.События.Команды.Бой.Перестать_сражаться, StopFight);
    }

    private void AttackTrget() => combatSystem.Attack(target);

    public override void UpdateMe()
    {
        Debug.Log("UpdateMe");
        if (target) AttackTrget();
    }

    private void SetTarget(AttackEvent obj) => target = obj.Unit;
    private void StopFight(StopFightEvent obj) => target = null;

    public void SetHitBoolOFF()
    {
        Debug.Log("SetHitBoolOFF");
        if(weapon)weapon.SetHitBoolOFF();
    }
    
}
