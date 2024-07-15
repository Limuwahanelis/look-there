using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMageStateMoveCasting : EnemyState
{
    public static Type StateType { get => typeof(SkeletonMageStateMoveCasting); }
    private SkeletonMageContext _context;

    private Vector2 _moveVector;
    private bool _isMovingFromPlayer;
    private bool _isMovingToPlayer;
    private int _spawnIndex = 0;
    public SkeletonMageStateMoveCasting(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        _context.boneAttackTime += Time.deltaTime;
        if (_context.boneAttackTime > _context.boneMissileCooldown)
        {
            _context.boneSpawner.SpawnBone(_spawnIndex, _context.playerTransform, _context.boneSpeed.value + UnityEngine.Random.Range(-0.25f, 0.25f));
            _spawnIndex++;
            if (_spawnIndex > 1) _spawnIndex = 0;
            _context.boneAttackTime = 0;
        }
        if (_isMovingFromPlayer)
        {
            _moveVector = (_context.enemyTransform.position - _context.playerTransform.position).normalized;
            
            if (Vector2.Distance(_context.enemyTransform.position, _context.playerTransform.position)>_context.playerDetectionRange)
            {
                ChangeState(SkeletonMageStateIdleCasting.StateType);
                return;
            }
        }
        if(_isMovingToPlayer)
        {
            _moveVector = (_context.playerTransform.position-_context.enemyTransform.position ).normalized;
            if (Vector2.Distance(_context.enemyTransform.position, _context.playerTransform.position) < _context.playerDetectionRange)
            {
                ChangeState(SkeletonMageStateIdleCasting.StateType);
                return;
            }
        }
        if (Math.Abs(_moveVector.x) > 0.25) _context.animMan.PlayAnimation("Move cast");
        else _context.animMan.PlayAnimation("Idle cast");
        _context.enemyMovement.Move(_moveVector);
    }

    public override void SetUpState( EnemyContext context)
    {
        base.SetUpState( context);
        _context = (SkeletonMageContext)context;

        if (Vector2.Distance(_context.enemyTransform.position, _context.playerTransform.position) > _context.moveToPlayerDistance) _isMovingToPlayer = true;
        if (Vector2.Distance(_context.enemyTransform.position, _context.playerTransform.position) < _context.moveFromPlayerDistance) _isMovingFromPlayer = true;
    }

    public override void InterruptState()
    {
        _isMovingToPlayer = false;
        _isMovingFromPlayer = false;
    }
}