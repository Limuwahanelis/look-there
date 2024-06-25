using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingAttackState : PlayerAttackState
{
    private float _jumpAttackTime;
    
    public static Type StateType { get => typeof(PlayerJumpingAttackState); }
    public PlayerJumpingAttackState(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        _time += Time.deltaTime;
        AttackCheck(PlayerCombat.AttackType.JUMPING);
        if(_time > _jumpAttackTime)
        {
            _context.playerMovement.SetRB(true);
            ChangeState(PlayerInAirState.StateType);
        }
    }

    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
        _context.animationManager.PlayAnimation("Jump Attack");
        _animSpeed = _context.animationManager.GetAnimationSpeed("Jump Attack");
        _jumpAttackTime = _context.animationManager.GetAnimationLength("Jump Attack")/_animSpeed;
        _currentAttack = _context.combat.JumpAttack;
        _attackDamageStartWindow = _currentAttack.AttackDamageWindowStart;
        _attackDamageEndWindow = _currentAttack.AttackDamageWindowEnd;
        _context.playerMovement.StopPlayer();
        _context.playerMovement.SetRB(false);

        _time = 0;
    }

    public override void InterruptState()
    {
        _context.playerMovement.SetRB(true);
        if( _attackCor != null ) _context.coroutineHolder.StopCoroutine(_attackCor);
        _attackCor = null;
    }
}