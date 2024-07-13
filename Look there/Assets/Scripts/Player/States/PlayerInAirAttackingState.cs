using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirAttackingState : PlayerAttackState
{
    private bool _nextAttack;
    private bool _changeToSlam;
    private int _comboCounter = 1;
    private int _maxCombo = 3;
    private float _comboEndWindow;
    private float _comboStartWindow;
    public static Type StateType { get => typeof(PlayerInAirAttackingState); }
    public PlayerInAirAttackingState(GetState function) : base(function)
    {

    }

    public override void Update()
    {
        _time += Time.deltaTime;

        AttackCheck(PlayerCombat.AttackType.NORMAL);

        if (_nextAttack)
        {
            if (_time >= _comboStartWindow)
            {
                _time = 0;
                _comboCounter++;
                if (_comboCounter==_maxCombo)
                {
                    if (_changeToSlam)
                    {
                        ChangeState(PlayerAirSlammingState.StateType);
                        return;
                    }
                }
                _context.animationManager.PlayAnimation($"Air Attack{_comboCounter}");
                _animSpeed = _context.animationManager.GetAnimationSpeed("Air Attack" + _comboCounter, "Base Layer");
                _currentAttack = _context.combat.PlayerAirCombos.comboList[_comboCounter-1];
                _comboStartWindow = _currentAttack.AttackWindowStart / _animSpeed;
                _comboEndWindow = _currentAttack.AttackWindowEnd / _animSpeed;
                _attackDamageStartWindow = _currentAttack.AttackDamageWindowStart;
                _attackDamageEndWindow = _currentAttack.AttackDamageWindowEnd;
                _checkForDmg = true;
                _isDealingDmg = false;
                _nextAttack = false;
            }
        }
        else if (_time >= _comboEndWindow)
        {
            if(_comboCounter==_maxCombo)
            {
                if (_changeToSlam)
                {
                    ChangeState(PlayerAirSlammingState.StateType);
                    return;
                }
            }
            ChangeState(PlayerInAirState.StateType);
        }
        PerformInputCommand();
    }

    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
        _context.canPerformAirCombo = false;
        _changeToSlam = false;
        _nextAttack = false;
        _comboCounter = 1;
        _context.playerMovement.SetRB(false);
        _context.playerMovement.StopPlayer();

        _context.animationManager.PlayAnimation("Air Attack1");

        _animSpeed = _context.animationManager.GetAnimationSpeed("Air Attack" + _comboCounter, "Base Layer");
        _currentAttack = _context.combat.PlayerAirCombos.comboList[_comboCounter - 1];
        _comboStartWindow = _currentAttack.AttackWindowStart / _animSpeed;
        _comboEndWindow = _currentAttack.AttackWindowEnd / _animSpeed;
        _attackDamageStartWindow = _currentAttack.AttackDamageWindowStart;
        _attackDamageEndWindow = _currentAttack.AttackDamageWindowEnd;
        //_attackCor = _context.coroutineHolder.StartCoroutine(_context.combat.AttackCor(PlayerCombat.AttackType.NORMAL));
    }

    public override void Attack(PlayerCombat.AttackModifiers modifier)
    {
        if (_comboCounter < _maxCombo)
        {
            if (_comboCounter == _maxCombo - 1)
            {
                _nextAttack = true;
                if (modifier == PlayerCombat.AttackModifiers.DOWN_ARROW) _changeToSlam = true;
                else _changeToSlam = false;
            }
            else _nextAttack = true;
            Logger.Log("next");
        }
        else _nextAttack = false;

    }
    public override void InterruptState()
    {
        if(_attackCor!=null) _context.coroutineHolder.StopCoroutine(_attackCor);
        _context.playerMovement.SetRB(true);
        _attackCor = null;
    }
}