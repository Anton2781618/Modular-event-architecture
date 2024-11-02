using System;
using System.Collections.Generic;
using UnityEngine;

[CompatibleUnit(typeof(Player))]
public sealed class PlayerMovementModule : ModuleBase
{
    private Animator _animator;
    private PlayerMovementSystem playerMovementSystem;
    private bool _isTiredEffect = false;
    private bool _isRopeConnect = false;
    private bool _isRopeTired = false;

    protected override void Initialize()
    {
        base.Initialize();
        
        if (!_animator) _animator = GetComponent<Animator>();

        if (playerMovementSystem == null) playerMovementSystem = new PlayerMovementSystem(_animator);

        LocalEvents.Subscribe<BaseEvent>(LocalEventBus.События.Состояния.Усталость, ApplayTiredEffect);

        LocalEvents.Subscribe<BaseEvent>(LocalEventBus.События.Команды.Веревка.Веревка_присоеденена, RopeConnected);        
        LocalEvents.Subscribe<BaseEvent>(LocalEventBus.События.Команды.Веревка.Веревка_отсоеденена, RopeDisconnected);

        LocalEvents.Subscribe<BaseEvent>(LocalEventBus.События.Команды.Веревка.Веревка_натянулась, RopeTiredOn);
        LocalEvents.Subscribe<BaseEvent>(LocalEventBus.События.Команды.Веревка.Веревка_раслабилась, RopeTiredOff);
    }
    
    private void RopeTiredOn(BaseEvent empty)
    {
        if (_isRopeTired) return;

        _isRopeTired = true;
    }
    private void RopeTiredOff(BaseEvent empty)
    {
        if (!_isRopeTired) return;

        _isRopeTired = false;
    }


    private void ApplayTiredEffect(BaseEvent effectEvent)
    {
        _isTiredEffect = effectEvent.Enabled;
    }

    public void RopeConnected(BaseEvent data) => _isRopeConnect = true;
    public void RopeDisconnected(BaseEvent data) => _isRopeConnect = false;


    public override void UpdateMe()
    {
        if (_isRopeConnect && playerMovementSystem.MoveVector.magnitude > 0 && _isRopeTired == true)
        {
            // LocalEvents.Publish(LocalEventBus.События.Состояния.Эффекты.Эффект_изменился, new EffectEvent{Effect = new Effect( name: Effect.EffectName.Rope, duration: 0)});
        }
        else
        if (Input.GetKey(KeyCode.LeftShift) && playerMovementSystem.MoveVector.magnitude > 0)
        {
            if (!_isTiredEffect) playerMovementSystem.Sprint = true;

            LocalEvents.Publish(LocalEventBus.События.Команды.Движение.Начать_спринт, new BaseEvent());
        }
        else
        if (playerMovementSystem.Sprint == true && (Input.GetKeyUp(KeyCode.LeftShift) || playerMovementSystem.MoveVector.magnitude == 0))
        {
            StopSprint();
        }

        Debug.Log("PlayerMovementModule UpdateMe!!!!!!!!!!!!!!!!!!!!");
        
        playerMovementSystem.UpdateMe();
    }

    //метод закончить спринт
    public void StopSprint()
    {
        playerMovementSystem.Sprint = false;

        LocalEvents.Publish(LocalEventBus.События.Команды.Движение.Закончить_спринт, new BaseEvent());
    }
}
