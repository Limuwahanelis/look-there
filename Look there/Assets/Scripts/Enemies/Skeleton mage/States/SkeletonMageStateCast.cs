using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMageStateCast : EnemyState
{
    public static Type StateType { get => typeof(SkeletonMageStateCast); }
    private SkeletonMageContext _context;

    private float _time;
    private float _castAnimTime;
    private int _castNum = 2;
    public SkeletonMageStateCast(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        _time += Time.deltaTime;
        if(_time > _castAnimTime* _castNum) 
        {
            ChangeState(SkeletonMageStateIdleCasting.StateType);
        }
    }

    public override void SetUpState(EnemyContext context)
    {
        base.SetUpState(context);
        _context = (SkeletonMageContext)context;
        _context.animMan.PlayAnimation("Talk cast");
        _castAnimTime = _context.animMan.GetAnimationLength("Talk cast");
    }

    public override void InterruptState()
    {
     
    }
}