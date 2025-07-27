using System;
using UnityEngine;

namespace ModularEventArchitecture
{
    /// Интерфейс для данных событий
    [Serializable]
    public class EventBase : IEventData 
    {
    }

    [Serializable]
    public struct AttackEvent : IEventData
    {
        public GameObject Unit;
    }

    [Serializable]
    public struct StopFightEvent : IEventData { }

    [Serializable]
    public struct MoveToPointEvent : IEventData
    {
        [SerializeField] private Vector3 point;
        public Vector3 Point { get => point; set => point = value; }
    }

    [Serializable]
    public struct MoveToTargetEvent : IEventData
    {
        [SerializeField] private Transform targetTransform;
        public Transform target { get => targetTransform; set => targetTransform = value; }
    }

    [Serializable]
    public struct DamageEvent : IEventData
    {
        [SerializeField] private float damage;
        [SerializeField] private Vector3 hitDirection;
        
        public float Damage { get => damage; set => damage = value; }
        public Vector3 HitDirection { get => hitDirection; set => hitDirection = value; }
    }

    [Serializable]
    public struct StunEvent : IEventData
    {
        [SerializeField] private bool stunState;
        [SerializeField] private float stunTime;

        public bool isStun { get => stunState; set => stunState = value; }
        public float Time { get => stunTime; set => stunTime = value; }
    }

    [Serializable]
    public struct StaminaChangedEvent : IEventData 
    {
        public float MaxStamina;
        public float CurrentStamina;
    }

    [Serializable]
    public struct HealthChangedEvent : IEventData 
    {
        public float MaxHealth;
        public float CurrentHealth;
    }
}