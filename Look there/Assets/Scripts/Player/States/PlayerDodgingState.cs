using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerState
{
    public static Type StateType { get => typeof(PlayerDodgingState); }
    private float _time;
    private float _dodgeTime;
    public PlayerDodgingState(GetState function) : base(function)
    {
        
    }

    public override void Update()
    {
        _time += Time.deltaTime;
        if (_time >= 0.5f)
        {
            _context.playerMovement.StopPlayer();
            ChangeState(PlayerIdleState.StateType);
            _context.playerDodge.SetEnemyCollider(true);
        }
    }

    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
        _dodgeTime = _context.animationManager.GetAnimationLength("Dodge");
        Logger.Log("doge start");
        _time = 0;
        Logger.Log(_time);
        _context.animationManager.PlayAnimation("Dodge");
        _context.playerMovement.Dodge();
        _context.playerDodge.SetEnemyCollider(false);
    }

    public override void InterruptState()
    {
     
    }
    public override void Dodge()
    {
        
    }
}