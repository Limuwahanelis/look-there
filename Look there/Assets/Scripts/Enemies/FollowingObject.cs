using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObject : MonoBehaviour
{
    [SerializeField] HealthSystem _healthSystem;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] float _pushForce;
    [SerializeField] float _maxSpeed;
    [SerializeField] float _force;
    [SerializeField] GameObject _objectToFollow;
    private void Awake()
    {
        _healthSystem.OnHitEvent += Push;
    }
    private void FixedUpdate()
    {
        if (_rb.velocity.magnitude < _maxSpeed)
        {
            _rb.AddForce((_objectToFollow.transform.position - transform.position).normalized * _force);
        }
    }
    private void Push(DamageInfo info)
    {
        _rb.AddForce((transform.position - info.dmgPosition).normalized * _pushForce,ForceMode2D.Impulse);
    }

}
