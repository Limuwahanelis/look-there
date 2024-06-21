using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerAttackingState : PlayerState
{
    private int _comboCounter=1;
    private float _time;
    private bool _nextAttack;
    private int _maxCombo = 3;
    private float _comboEndWindow;
    private float _comboStartWindow;
    private Coroutine _attackCor;
    public static Type StateType { get => typeof(PlayerAttackingState); }
    public PlayerAttackingState(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        _time += Time.deltaTime;
        if (_nextAttack)
        {
            if(_time >= _comboStartWindow)
            {
                _time = 0;
                _comboCounter++;
                if (_comboCounter > _maxCombo)
                {
                    _comboCounter = 1;
                }
                _context.coroutineHolder.StopCoroutine(_attackCor);
                _attackCor = null;
                _context.animationManager.PlayAnimation($"Attack{_comboCounter}");
                _comboStartWindow = _context.combat.PlayerCombos.comboList[_comboCounter - 1].AttackWindowStart / _context.animationManager.GetAnimationSpeed("Attack" + _comboCounter, "Base Layer");
                _comboEndWindow = _context.combat.PlayerCombos.comboList[_comboCounter - 1].AttackWindowEnd / _context.animationManager.GetAnimationSpeed("Attack" + _comboCounter, "Base Layer");
                _attackCor=_context.coroutineHolder.StartCoroutine(_context.combat.AttackCor());
                _nextAttack = false;
            }
        }
        else if(_time>= _comboEndWindow)
        {
            ChangeState(PlayerIdleState.StateType);
        }
    }

    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
        _attackCor = null;
        _context.playerMovement.StopPlayer();
        _comboCounter = 1;
        _time = 0;
        _nextAttack = false;
        _context.animationManager.PlayAnimation("Attack1");
        //_attackTime = _context.animationManager.GetAnimationLength("Attack1");
        _comboStartWindow = _context.combat.PlayerCombos.comboList[_comboCounter - 1].AttackWindowStart / _context.animationManager.GetAnimationSpeed("Attack" + _comboCounter, "Base Layer");
        _comboEndWindow = _context.combat.PlayerCombos.comboList[_comboCounter - 1].AttackWindowEnd / _context.animationManager.GetAnimationSpeed("Attack" + _comboCounter, "Base Layer");
        _attackCor = _context.coroutineHolder.StartCoroutine(_context.combat.AttackCor());
    }
    public override void Dodge()
    {
        ChangeState(PlayerDodgingState.StateType);
    }
    public override void Attack()
    {
            _nextAttack = true;
    }
    public override void InterruptState()
    {
        _context.coroutineHolder.StopCoroutine(_attackCor);
        _attackCor = null;
    }
}