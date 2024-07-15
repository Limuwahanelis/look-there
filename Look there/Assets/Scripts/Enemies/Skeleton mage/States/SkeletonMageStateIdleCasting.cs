using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMageStateIdleCasting : EnemyState
{
    public static Type StateType { get => typeof(SkeletonMageStateIdleCasting); }
    private SkeletonMageContext _context;
    private int _spawnIndex = 0;
    private bool _spawnedFirstBones;
    public SkeletonMageStateIdleCasting(GetState function) : base(function)
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
        if (Vector2.Distance(_context.enemyTransform.position,_context.playerTransform.position)<_context.moveFromPlayerDistance)
        {
            ChangeState(SkeletonMageStateMoveCasting.StateType);
            return;
        }
        if (Vector2.Distance(_context.enemyTransform.position, _context.playerTransform.position) > _context.moveToPlayerDistance)
        {
            ChangeState(SkeletonMageStateMoveCasting.StateType);
            return;
        }
    }

    public override void SetUpState( EnemyContext context)
    {
        base.SetUpState( context);
        _context = (SkeletonMageContext)context;
        _context.animMan.PlayAnimation("Idle cast");
        if (_spawnedFirstBones) return;
        _context.rotatingObjectsSpawner.SpawnObject(0, _context.enemyTransform);
        _context.rotatingObjectsSpawner.SpawnObject(1, _context.enemyTransform);
        _spawnedFirstBones = true;
    }

    public override void InterruptState()
    {
     
    }
}