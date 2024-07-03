using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SkeletonStateWalk : EnemyState
{
    public static Type StateType { get => typeof(SkeletonStateWalk); }
    private SkeletonContext _context;
    EnemyMovement.enemyDirection _direction;
    public SkeletonStateWalk(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        if (_context.playerTransform.position.x > _context.enemyTransform.position.x) _direction = EnemyMovement.enemyDirection.RIGHT;
        else _direction = EnemyMovement.enemyDirection.LEFT;
        if (math.abs(_context.enemyTransform.position.x - _context.playerTransform.position.x) > _context.minPlayerDistance)
        {
            _context.enemyMovement.Move(((float)_direction));
        }
        else
        {
            _context.enemyMovement.Stop();
            ChangeState(SkeletonStateAttack.StateType);
        }
    }

    public override void SetUpState(EnemyContext context)
    {
        base.SetUpState(context);
        _context = (SkeletonContext)context;
        _context.animMan.PlayAnimation("Walk");
        
    }

    public override void InterruptState()
    {
     
    }
}