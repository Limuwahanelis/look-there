using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateAttack : EnemyState
{
    public static Type StateType { get => typeof(SkeletonStateAttack); }
    private SkeletonContext _context;

    protected float _attackDamageStartWindow;
    protected float _attackDamageEndWindow;
    protected float _animSpeed;
    protected bool _isDealingDmg;
    protected bool _checkForDmg = true;
    protected float _time;
    protected ComboAttack _currentAttack;
    protected Coroutine _attackCor;

    private int _comboCounter = 1;
    private bool _nextAttack;
    private int _maxCombo = 2;
    private float[] _attacksAnimLengths= new float[2];
    public SkeletonStateAttack(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        _time += Time.deltaTime;

        if (_comboCounter == 1) AttackCheck(SkeletonCombat.AttackType.SLASH);
        else AttackCheck(SkeletonCombat.AttackType.STAB);

        if (Math.Abs(_context.playerTransform.position.x - _context.enemyTransform.position.x) < _context.maxPlayerDistance) _nextAttack = true;
        else _nextAttack = false;

        if (_nextAttack)
        {
            if (_time >= _attacksAnimLengths[_comboCounter-1] + _context.combat.AttacksDelays[_comboCounter-1])
            {
                _time = 0;
                _comboCounter++;
                if (_comboCounter > _maxCombo)
                {
                    _comboCounter = 1;
                }
                _context.animMan.PlayAnimation($"Attack{_comboCounter}");
                _currentAttack = _context.combat.SkeletonCombos.comboList[_comboCounter - 1];
                _animSpeed = _context.animMan.GetAnimationSpeed("Attack" + _comboCounter, "Base Layer");
                _nextAttack = false;
                _isDealingDmg = false;
                _checkForDmg = true;
            }
        }
        else if (_time >= _attacksAnimLengths[_comboCounter - 1] + _context.combat.AttacksDelays[_comboCounter - 1])
        {
            _comboCounter++;
            if (_comboCounter > _maxCombo)
            {
                _comboCounter = 1;
            }
            ChangeState(SkeletonStateIdle.StateType);
        }
    }

    public override void SetUpState(EnemyContext context)
    {
        base.SetUpState(context);
        _context = (SkeletonContext)context;

        //_comboCounter = 1;
        _isDealingDmg = false;
        _time = 0;
        _nextAttack = false;
        _attackCor = null;
        _checkForDmg = true;
        _context.animMan.PlayAnimation("Attack" + _comboCounter);
        _animSpeed = _context.animMan.GetAnimationSpeed("Attack" + _comboCounter, "Base Layer");
        _currentAttack = _context.combat.SkeletonCombos.comboList[_comboCounter - 1];
        for(int i=0;i<_comboCounter; i++)
        {
            _attacksAnimLengths[i]=_context.animMan.GetAnimationLength("Attack"+_comboCounter)/ _context.animMan.GetAnimationSpeed("Attack" + _comboCounter);
        }
    }

    public virtual void AttackCheck(SkeletonCombat.AttackType attackType)
    {
        if (_checkForDmg)
        {
            if (!_isDealingDmg)
            {
                if (_time > _attackDamageStartWindow)
                {
                    _attackCor = _context.coroutineHolder.StartCoroutine(_context.combat.AttackCor(attackType));
                    _isDealingDmg = true;
                }
            }
            else
            {
                if (_time > _attackDamageEndWindow)
                {
                    _context.coroutineHolder.StopCoroutine(_attackCor);
                    _attackCor = null;
                    _checkForDmg = false;
                }
            }
        }
    }
    public override void InterruptState()
    {
     
    }
}