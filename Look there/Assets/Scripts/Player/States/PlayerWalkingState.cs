using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkingState : PlayerState
{
    public static Type StateType { get => typeof(PlayerWalkingState); }
    public PlayerWalkingState(GetState function) : base(function)
    {
    }
    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
        _context.animationManager.PlayAnimation("Walk");
    }
    public override void Update()
    {

    }
    public override void Move(Vector2 direction)
    {
        if (direction.x == 0)
        {
            ChangeState(PlayerIdleState.StateType);
        }
        _context.playerMovement.Move(direction.x);
    }
    public override void Jump()
    {
        ChangeState(PlayerJumpingState.StateType);
    }
    public override void Dodge()
    {
        ChangeState(PlayerDodgingState.StateType);
    }
    public override void Attack()
    {
        ChangeState(PlayerAttackingState.StateType);
    }
    public override void InterruptState()
    {
     
    }
}