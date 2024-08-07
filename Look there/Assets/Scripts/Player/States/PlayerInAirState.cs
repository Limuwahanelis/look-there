using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    public static Type StateType { get => typeof(PlayerInAirState); }
    private bool _isFalling;
    private bool _jumpOnLanding;
    public PlayerInAirState(GetState function) : base(function)
    {
    }
    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
    }
    public override void Update()
    {
        PerformInputCommand();
        if(_context.playerMovement.IsPlayerFalling)
        {
            if (!_isFalling)
            {
                _context.animationManager.PlayAnimation("Fall");
                _isFalling = true;
            }
        }
        if(_context.checks.IsOnGround && math.abs( _context.playerMovement.PlayerRB.velocity.y) < 0.0004) 
        {
            if (_jumpOnLanding) ChangeState(PlayerJumpingState.StateType);
            else ChangeState(PlayerIdleState.StateType);
        }
        
    }
    public override void Jump()
    {
        _jumpOnLanding = true;
    }
    public override void Move(Vector2 direction)
    {
        _context.playerMovement.Move(direction.x);
    }
    public override void Attack(PlayerCombat.AttackModifiers modifier)
    {
        if (modifier == PlayerCombat.AttackModifiers.DOWN_ARROW)
        {
            ChangeState(PlayerAirSlammingState.StateType);
            return;
        }
        if(_context.canPerformAirCombo) ChangeState(PlayerInAirAttackingState.StateType);
    }
    public override void UndoComand()
    {
        _jumpOnLanding = false;
    }
    public override void InterruptState()
    {
        _isFalling = false;
        _jumpOnLanding = false;
    }
}