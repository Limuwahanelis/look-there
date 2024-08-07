using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateIdle : EnemyState
{
    public static Type StateType { get => typeof(SkeletonStateIdle); }
    private SkeletonContext _context;

    private int _idleCycles = 1;
    private float _idleTime;
    private float _time;
    public SkeletonStateIdle(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        _time += Time.deltaTime;
        if(_time > _idleTime) 
        {
           ChangeState(SkeletonStateWalk.StateType);
        }
    }

    public override void SetUpState(EnemyContext context)
    {
        base.SetUpState(context);
        _context = (SkeletonContext)context;
        _context.animMan.PlayAnimation("Idle");
        _time = 0;
        _idleTime = _context.animMan.GetAnimationLength("Idle");
    }

    public override void InterruptState()
    {
     
    }
}