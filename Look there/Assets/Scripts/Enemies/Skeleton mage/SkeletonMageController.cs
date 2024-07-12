using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkeletonMageController : EnemyController
{
    [Header("Skeleton mage"), SerializeField] float _boneCooldown;
    [SerializeField] BoneMissileSpawner _boneSpawner;
    [SerializeField] float _playerDetectionRange;
    [SerializeField] float _boneBaseSpeed;
    protected SkeletonMageContext _context;

    void Start()
    {
        List<Type> states = AppDomain.CurrentDomain.GetAssemblies().SelectMany(domainAssembly => domainAssembly.GetTypes())
    .Where(type => typeof(EnemyState).IsAssignableFrom(type) && !type.IsAbstract).ToArray().ToList();

        _context = new SkeletonMageContext
        {
            ChangeEnemyState = ChangeState,
            animMan = _enemyAnimationManager,
            boneSpawner= _boneSpawner,
            playerTransform=_playerTransform,
            enemyTransform=transform,
            boneMissileCooldown = _boneCooldown,
            playerDetectionRange = _playerDetectionRange,
            boneSpeed = _boneBaseSpeed,
        };
        EnemyState.GetState getState = GetState;
        foreach (Type state in states)
        {
            _enemyStates.Add(state, (EnemyState)Activator.CreateInstance(state, getState));
        }
        EnemyState newState = GetState(typeof(SkeletonMageStateIdle));
        newState.SetUpState(_context);
        _currentEnemyState = newState;
    }

    void Update()
    {
        _currentEnemyState?.Update();
    }
}
