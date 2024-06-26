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
    [SerializeField] float _comboWindowTime;
    private float _time;
    private bool _isStillCombo = false;
    private bool _isCombCorOn = false;
    private void Awake()
    {
        _healthSystem.OnHitEvent += Push;
        
    }
    private void FixedUpdate()
    {
        if (_isCombCorOn) return;
        transform.up = _objectToFollow.transform.position - transform.position;
        if (_rb.velocity.magnitude < _maxSpeed)
        {
            _rb.AddForce((_objectToFollow.transform.position - transform.position).normalized * _force);
        }
    }
    IEnumerator ComboCor(DamageInfo info)
    {
        _isCombCorOn = true;
        while (_time < _comboWindowTime)
        {
            _time += Time.deltaTime;
            yield return null;
            if (_isStillCombo)
            {
                _time = 0;
                _isStillCombo = false;
            }
        }
        _time = 0;
        _rb.AddForce((transform.position - info.dmgPosition).normalized * _pushForce, ForceMode2D.Impulse);
        _isCombCorOn = false;
    }
    private void Push(DamageInfo info)
    {
        if (!_isCombCorOn)
        {
            _rb.velocity = Vector2.zero;
            StartCoroutine(ComboCor(info));
        }
        else
        {
            _isStillCombo = true;
        }
        
    }

}
