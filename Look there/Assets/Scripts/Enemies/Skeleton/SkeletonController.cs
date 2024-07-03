using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkeletonController :EnemyController
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] SkeletonCombat _combat;
    [SerializeField] EnemyMovement _enemyMovement;
    [SerializeField] float _maxPlayerDistance;
    [SerializeField] float _minPlayerDistance;
    [SerializeField] float _speed;
    private SkeletonContext _context;
    // Start is called before the first frame update
    void Start()
    {
        List<Type> states = AppDomain.CurrentDomain.GetAssemblies().SelectMany(domainAssembly => domainAssembly.GetTypes())
    .Where(type => typeof(EnemyState).IsAssignableFrom(type) && !type.IsAbstract).ToArray().ToList();

        EnemyState.GetState getState = GetState;
        foreach (Type state in states)
        {
            _enemyStates.Add(state, (EnemyState)Activator.CreateInstance(state, getState));
        }

        _context = new SkeletonContext
        {
            ChangeEnemyState = ChangeState,
            animMan = _enemyAnimationManager,
            enemyHitState = GetState(typeof(SkeletonStateHit)),
            playerTransform = _playerTransform,
            enemyTransform = transform,
            rb = _rb,
            coroutineHolder = this,
            combat = _combat,
            minPlayerDistance = _minPlayerDistance,
            maxPlayerDistance = _maxPlayerDistance,
            enemyMovement = _enemyMovement

        };

        EnemyState newState = GetState(typeof(SkeletonStateIdle));
        newState.SetUpState(_context);
        _currentEnemyState = newState;

        _healthSystem.OnHitEvent += _currentEnemyState.Hit;
    }

    // Update is called once per frame
    void Update()
    {
        _currentEnemyState?.Update();
    }
    private void OnDestroy()
    {
        _healthSystem.OnHitEvent -= _currentEnemyState.Hit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + Vector3.right * _maxPlayerDistance, transform.position + Vector3.right * _maxPlayerDistance + Vector3.down*2);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.right * _minPlayerDistance, transform.position + Vector3.right * _minPlayerDistance + Vector3.down * 2);
    }
}
