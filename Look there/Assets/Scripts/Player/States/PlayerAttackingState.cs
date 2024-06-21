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
    public static Type StateType { get => typeof(PlayerAttackingState); }
    public PlayerAttackingState(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        _time += Time.deltaTime;
        //if (_time>=_attackTime)
        //{
        //    if(_nextAttack)
        //    {
        //        _time = 0;
        //        _comboCounter++;
        //        if(_comboCounter>_maxCombo)
        //        {
        //            _comboCounter = 1;
        //        }
        //        _attackTime = _context.animationManager.GetAnimationLength($"Attack{_comboCounter}");
        //        _context.animationManager.PlayAnimation($"Attack{_comboCounter}");
        //        _nextAttack = false;
        //    }
        //    else
        //    {
        //        ChangeState(PlayerIdleState.StateType);
        //    }
        //}
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
        _context.playerMovement.StopPlayer();
        _comboCounter = 1;
        _time = 0;
        _nextAttack = false;
        _context.animationManager.PlayAnimation("Attack1");
        //_attackTime = _context.animationManager.GetAnimationLength("Attack1");
        _comboStartWindow = _context.combat.PlayerCombos.comboList[_comboCounter - 1].AttackWindowStart / _context.animationManager.GetAnimationSpeed("Attack" + _comboCounter, "Base Layer");
        _comboEndWindow = _context.combat.PlayerCombos.comboList[_comboCounter - 1].AttackWindowEnd / _context.animationManager.GetAnimationSpeed("Attack" + _comboCounter, "Base Layer");
    }
    public override void Dodge()
    {
        ChangeState(PlayerDodgingState.StateType);
    }
    public override void Attack()
    {
    //    if (_context.animationManager.GetAnimationCurrentDuration("Attack" + _comboCounter, "Base Layer") >= _context.combat.PlayerCombos.comboList[_comboCounter - 1].AttackWindowStart / _context.animationManager.GetAnimationSpeed("Attack" + _comboCounter, "Base Layer")
    //&& _context.animationManager.GetAnimationCurrentDuration("Attack" + _comboCounter, "Base Layer") <= _context.combat.PlayerCombos.comboList[_comboCounter - 1].AttackWindowEnd / _context.animationManager.GetAnimationSpeed("Attack" + _comboCounter, "Base Layer"))
    //    {
            _nextAttack = true;
        //}
       
    }
    public void PerformNextAttackInCombo(in PlayerAttackingState attackState)
    {
        //if (_comboAttackCounter > _comboList1.comboList.Count)
        //{
        //    return;
        //}
        if (_context.animationManager.GetAnimationCurrentDuration("Attack " + _comboCounter, "Combat") >= _context.combat.PlayerCombos.comboList[_comboCounter - 1].AttackWindowStart / _context.animationManager.GetAnimationSpeed("Attack " + _comboCounter, "Combat")
            && _context.animationManager.GetAnimationCurrentDuration("Attack " + _comboCounter, "Combat") <= _context.combat.PlayerCombos.comboList[_comboCounter - 1].AttackWindowEnd / _context.animationManager.GetAnimationSpeed("Attack " + _comboCounter, "Combat"))
        {
            //_player.animManager.PlayAnimation("Attack " + _comboAttackCounter);
            //_player.anim.SetTrigger("Attack");
            //_comboAttackCounter++;
            //attackState.StartWaitingFoNextAttack("Attack " + _comboAttackCounter);
        }
    }
    //public float GetCurrentAttackWindowEndRaw()
    //{
    //    return _comboList1.comboList[_comboAttackCounter - 1].nextAttackWindowEnd;
    //}
    public override void InterruptState()
    {
     
    }
}