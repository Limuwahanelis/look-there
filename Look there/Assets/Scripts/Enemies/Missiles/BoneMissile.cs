using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneMissile : MonoBehaviour,IDamagable
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _speed;
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] Collider2D _col;
    private Transform _enemyTrans;
    private float _angle=0;
    private Vector2 _direction;
    private Vector2 _originPoint;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetRotation(float startAngle)
    {
        _angle = startAngle;
    }
    public void SetOwner(Transform tran)
    {
        _enemyTrans = tran;
    }
    public void SetDirectionAndSpeed(Vector2 target,float speed) 
    {
        _direction = (target - _rb.position).normalized;
        _speed = speed;
    }
    private void Update()
    {
        _angle += _rotationSpeed * Time.deltaTime;
        if (_angle > 360) _angle = 0;

        transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
        //_rb.MoveRotation(_angle);
        //_rb.rotation = _angle;
        if (_direction == Vector2.zero) return;
        _rb.MovePosition((_rb.position + _direction* _speed * Time.deltaTime));
    }

    public void TakeDamage(DamageInfo info)
    {
        if (_enemyTrans == null) _direction = -_direction;
        else 
        {
            Vector2 tmp = _enemyTrans.transform.position;
            _direction = (tmp - _rb.position).normalized;
        }
        _rb.includeLayers = _enemyLayer;
        _col.includeLayers = _enemyLayer;
        _col.excludeLayers = 0;
    }

    public void Kill()
    {
        
    }
}
