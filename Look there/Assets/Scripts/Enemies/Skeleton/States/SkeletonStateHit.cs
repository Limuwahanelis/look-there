using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateHit : EnemyState
{
    public static Type StateType { get => typeof(SkeletonStateHit); }
    private SkeletonContext _context;

    private float _animtime;
    private float _time=0;
    public SkeletonStateHit(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        _time += Time.deltaTime;
        if(_time > _animtime)
        {
            ChangeState(SkeletonStateIdle.StateType);
        }
    }

    public override void SetUpState(EnemyContext context)
    {
        base.SetUpState(context);
        _context = (SkeletonContext)context;

        _context.animMan.PlayAnimation("Hit");
        _animtime = _context.animMan.GetAnimationLength("Hit");
        _time = 0;
    }

    public override void InterruptState()
    {
     
    }
}