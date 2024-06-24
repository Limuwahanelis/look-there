using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingAttackState : PlayerState
{
    private float _jumpAttackTime;
    private float _time;
    private Coroutine _attackCor;
    public static Type StateType { get => typeof(PlayerJumpingAttackState); }
    public PlayerJumpingAttackState(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        _time += Time.deltaTime;
        if(_time > _jumpAttackTime)
        {
            _context.playerMovement.SetRB(true);
            _context.coroutineHolder.StopCoroutine(_attackCor);
            _attackCor = null;
            ChangeState(PlayerInAirState.StateType);
        }
    }

    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
        _context.animationManager.PlayAnimation("Jump Attack");
        _jumpAttackTime = _context.animationManager.GetAnimationLength("Jump Attack");
        _context.playerMovement.StopPlayer();
        _context.playerMovement.SetRB(false);
        _attackCor=_context.coroutineHolder.StartCoroutine(_context.combat.AttackCor(PlayerCombat.AttackType.JUMPING));
        _time = 0;
    }

    public override void InterruptState()
    {
        _context.playerMovement.SetRB(true);
        if( _attackCor != null ) _context.coroutineHolder.StopCoroutine(_attackCor);
        _attackCor = null;
    }
}