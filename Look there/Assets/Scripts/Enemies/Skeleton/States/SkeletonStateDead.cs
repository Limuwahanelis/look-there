using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateDead : EnemyState
{
    public static Type StateType { get => typeof(SkeletonStateDead); }
    private SkeletonContext _context;
    public SkeletonStateDead(GetState function) : base(function)
    {
    }

    public override void Update()
    {

    }

    public override void SetUpState(EnemyContext context)
    {
        base.SetUpState(context);
        _context = (SkeletonContext)context;

        _context.animMan.PlayAnimation("Death");
    }
    public override void Hit(DamageInfo damageInfo)
    {
        
    }
    public override void InterruptState()
    {
     
    }
}