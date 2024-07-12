using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMageStateIdle : EnemyState
{
    public static Type StateType { get => typeof(SkeletonMageStateIdle); }
    private SkeletonMageContext _context;
    public SkeletonMageStateIdle(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        if(Vector2.Distance(_context.enemyTransform.position,_context.playerTransform.position)<_context.playerDetectionRange)
        {
            ChangeState(SkeletonMageStateCast.StateType);
        }
    }

    public override void SetUpState(EnemyContext context)
    {
        base.SetUpState(context);
        _context = (SkeletonMageContext)context;

        _context.animMan.PlayAnimation("Idle");
    }

    public override void InterruptState()
    {
     
    }
}