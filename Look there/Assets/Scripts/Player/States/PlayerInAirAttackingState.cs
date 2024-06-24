using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirAttackingState : PlayerState
{
    private int _comboCounter = 1;
    private float _time;
    private bool _nextAttack;
    private int _maxCombo = 3;
    private float _comboEndWindow;
    private float _comboStartWindow;
    private Coroutine _attackCor;
    public static Type StateType { get => typeof(PlayerInAirAttackingState); }
    public PlayerInAirAttackingState(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        _time += Time.deltaTime;
        if (_nextAttack)
        {
            if (_time >= _comboStartWindow)
            {
                _time = 0;
                _comboCounter++;
                if(_attackCor!=null) _context.coroutineHolder.StopCoroutine(_attackCor);

                _attackCor = null;
                _context.animationManager.PlayAnimation($"Air Attack{_comboCounter}");
                _comboStartWindow = _context.combat.PlayerAirCombos.comboList[_comboCounter - 1].AttackWindowStart / _context.animationManager.GetAnimationSpeed("Air Attack" + _comboCounter, "Base Layer");
                _comboEndWindow = _context.combat.PlayerAirCombos.comboList[_comboCounter - 1].AttackWindowEnd / _context.animationManager.GetAnimationSpeed("Air Attack" + _comboCounter, "Base Layer");
                _attackCor = _context.coroutineHolder.StartCoroutine(_context.combat.AttackCor(PlayerCombat.AttackType.NORMAL));
                _nextAttack = false;
            }
        }
        else if (_time >= _comboEndWindow)
        {
            ChangeState(PlayerInAirState.StateType);
        }
    }

    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
        _context.canPerformAirCombo = false;
        _attackCor = null;
        _context.playerMovement.SetRB(false);
        _context.playerMovement.StopPlayer();
        _comboCounter = 1;
        _time = 0;
        _nextAttack = false;
        _context.animationManager.PlayAnimation("Air Attack1");
        //_attackTime = _context.animationManager.GetAnimationLength("Attack1");
        _comboStartWindow = _context.combat.PlayerAirCombos.comboList[_comboCounter - 1].AttackWindowStart / _context.animationManager.GetAnimationSpeed("Air Attack" + _comboCounter, "Base Layer");
        _comboEndWindow = _context.combat.PlayerAirCombos.comboList[_comboCounter - 1].AttackWindowEnd / _context.animationManager.GetAnimationSpeed("Air Attack" + _comboCounter, "Base Layer");
        _attackCor = _context.coroutineHolder.StartCoroutine(_context.combat.AttackCor(PlayerCombat.AttackType.NORMAL));
    }

    public override void Attack(PlayerCombat.AttackModifiers modifier)
    {
        if (_comboCounter == _maxCombo) return;
        _nextAttack = true;
    }
    public override void InterruptState()
    {
        _context.coroutineHolder.StopCoroutine(_attackCor);
        _context.playerMovement.SetRB(true);
        _attackCor = null;
    }
}