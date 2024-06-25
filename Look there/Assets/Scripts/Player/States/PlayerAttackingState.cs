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
    private float _attackDamageStartWindow;
    private float _attackDamageEndWindow;
    private float _animSpeed;
    private bool _isDealingDmg;
    private bool _chackForDmg = true;
    private ComboAttack _currentAttack;
    private Coroutine _attackCor;
    public static Type StateType { get => typeof(PlayerAttackingState); }
    public PlayerAttackingState(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        _time += Time.deltaTime;
        if (_chackForDmg)
        {
            if (!_isDealingDmg)
            {
                if (_time > _attackDamageStartWindow)
                {
                    _attackCor = _context.coroutineHolder.StartCoroutine(_context.combat.AttackCor(PlayerCombat.AttackType.NORMAL));
                    _isDealingDmg = true;
                }
            }
            else
            {
                if (_time > _attackDamageEndWindow)
                {
                    _context.coroutineHolder.StopCoroutine(_attackCor);
                    Logger.Log("End att");
                    _attackCor = null;
                    _chackForDmg = false;
                }
            }
        }

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
                _context.animationManager.PlayAnimation($"Attack{_comboCounter}");
                _comboStartWindow = _context.combat.PlayerCombos.comboList[_comboCounter - 1].AttackWindowStart / _context.animationManager.GetAnimationSpeed("Attack" + _comboCounter, "Base Layer");
                _comboEndWindow = _context.combat.PlayerCombos.comboList[_comboCounter - 1].AttackWindowEnd / _context.animationManager.GetAnimationSpeed("Attack" + _comboCounter, "Base Layer");
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

        _comboCounter = 1;
        _isDealingDmg = false;
        _time = 0;
        _nextAttack = false;
        _attackCor = null;
        _chackForDmg = true;
        _context.playerMovement.StopPlayer();
        _context.animationManager.PlayAnimation("Attack1");
        _animSpeed = _context.animationManager.GetAnimationSpeed("Attack" + _comboCounter, "Base Layer");
        _currentAttack = _context.combat.PlayerCombos.comboList[_comboCounter - 1];
        _comboStartWindow = _currentAttack.AttackWindowStart / _animSpeed;
        _comboEndWindow = _currentAttack.AttackWindowEnd / _animSpeed;
        _attackDamageStartWindow = _currentAttack.AttackDamageWindowStart / _animSpeed;
        _attackDamageEndWindow = _currentAttack.AttackDamageWindowEnd / _animSpeed;
        //_attackCor = _context.coroutineHolder.StartCoroutine(_context.combat.AttackCor(PlayerCombat.AttackType.NORMAL));
    }
    public override void Dodge()
    {
        ChangeState(PlayerDodgingState.StateType);
    }
    public override void Attack(PlayerCombat.AttackModifiers modifier)
    {
            _nextAttack = true;
    }
    public override void InterruptState()
    {
        if(_attackCor!=null) _context.coroutineHolder.StopCoroutine(_attackCor);
        _attackCor = null;
    }
}