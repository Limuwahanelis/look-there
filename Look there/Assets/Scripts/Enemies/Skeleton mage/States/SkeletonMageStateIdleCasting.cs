using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMageStateIdleCasting : EnemyState
{
    public static Type StateType { get => typeof(SkeletonMageStateIdleCasting); }
    private SkeletonMageContext _context;
    private float _boneAttackTime;
    private int _spawnIndex = 0;
    public SkeletonMageStateIdleCasting(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        _boneAttackTime += Time.deltaTime;
        if(_boneAttackTime>_context.boneMissileCooldown)
        {
            _context.boneSpawner.SpawnBone(_spawnIndex, _context.playerTransform,_context.boneSpeed+ UnityEngine.Random.Range(-4,4));
            _spawnIndex++;
            if (_spawnIndex > 1) _spawnIndex = 0;
            _boneAttackTime = 0;
        }
    }

    public override void SetUpState(EnemyContext context)
    {
        base.SetUpState(context);
        _context = (SkeletonMageContext)context;
        _context.animMan.PlayAnimation("Idle cast");
        _context.boneSpawner.InitialSpawn(0);
        _context.boneSpawner.InitialSpawn(1);
    }

    public override void InterruptState()
    {
     
    }
}