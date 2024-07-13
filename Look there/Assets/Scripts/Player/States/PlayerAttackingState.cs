using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerAttackState
{
    private int _comboCounter=1;
    private bool _nextAttack;
    private int _maxCombo = 3;
    private float _comboEndWindow;
    private float _comboStartWindow;
    private bool _performJumpingAttack = false;
    public static Type StateType { get => typeof(PlayerAttackingState); }
    public PlayerAttackingState(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        _time += Time.deltaTime;
        PerformInputCommand();
        AttackCheck(PlayerCombat.AttackType.NORMAL);

        if (_nextAttack)
        {
            if(_time >= _comboStartWindow)
            {
                _time = 0;
                _comboCounter++;
                if (_comboCounter > _maxCombo)
                {
                    if (_performJumpingAttack) 
                    {
                        ChangeState(PlayerIdleState.StateType);
                        return;
                    }

                    
                    _comboCounter = 1;
                }
                _context.animationManager.PlayAnimation($"Attack{_comboCounter}");
                _currentAttack = _context.combat.PlayerCombos.comboList[_comboCounter - 1];
                _animSpeed = _context.animationManager.GetAnimationSpeed("Attack" + _comboCounter, "Base Layer");
                _comboStartWindow = _currentAttack.AttackWindowStart / _animSpeed;
                _comboEndWindow = _currentAttack.AttackWindowEnd / _animSpeed;
                _nextAttack = false;
                _isDealingDmg = false;
                _checkForDmg = true;
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
        _checkForDmg = true;
        _context.playerMovement.StopPlayer();
        _context.animationManager.PlayAnimation("Attack1");
        _animSpeed = _context.animationManager.GetAnimationSpeed("Attack" + _comboCounter, "Base Layer");
        _currentAttack = _context.combat.PlayerCombos.comboList[_comboCounter - 1];
        _comboStartWindow = _currentAttack.AttackWindowStart / _animSpeed;
        _comboEndWindow = _currentAttack.AttackWindowEnd / _animSpeed;
        _attackDamageStartWindow = _currentAttack.AttackDamageWindowStart / _animSpeed;
        _attackDamageEndWindow = _currentAttack.AttackDamageWindowEnd / _animSpeed;
    }
    public override void Dodge()
    {
        ChangeState(PlayerDodgingState.StateType);
    }
    public override void Attack(PlayerCombat.AttackModifiers modifier)
    {
        if (modifier == PlayerCombat.AttackModifiers.UP_ARROW) _performJumpingAttack = true;
        _nextAttack = true;
    }
    public override void InterruptState()
    {
        if(_attackCor!=null) _context.coroutineHolder.StopCoroutine(_attackCor);
        _attackCor = null;
    }
}