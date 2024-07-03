using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum enemyDirection
    {
        LEFT = -1,
        SAME = 0,
        RIGHT = 1
    }
    public bool IsPlayerFalling { get => _rb.velocity.y < 0; }
    public Rigidbody2D RB => _rb;
    public int FlipSide => _flipSide;
    private enemyDirection _newDirection;
    private enemyDirection _oldDirection;
    [SerializeField] float _speed;
    [SerializeField] float _jumpStrength;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] EnemyController _enemy;
    private float _previousDirection;
    private int _flipSide = 1;
    public void Move(float direction)
    {
        if (direction != 0)
        {
            _oldDirection = _newDirection;
            _newDirection = (enemyDirection)direction;
            _rb.velocity = new Vector3(direction * _speed, _rb.velocity.y, 0);
            if (direction > 0)
            {
                _flipSide = 1;
                _enemy.MainBody.transform.localScale = new Vector3(_flipSide, _enemy.MainBody.transform.localScale.y, _enemy.MainBody.transform.localScale.z);
            }
            if (direction < 0)
            {
                _flipSide = -1;
                _enemy.MainBody.transform.localScale = new Vector3(_flipSide, _enemy.MainBody.transform.localScale.y, _enemy.MainBody.transform.localScale.z);
            }

        }
        else
        {
            if (_previousDirection != 0)
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
            }
        }
        _previousDirection = direction;
    }
    public void Stop(bool vertical = true, bool horizontal = true)
    {
        if (horizontal) _rb.velocity = new Vector2(0, _rb.velocity.y);
        if (vertical) _rb.velocity = new Vector2(_rb.velocity.x, 0);
    }
    public void Jump()
    {
        _rb.velocity = new Vector3(0, 0, 0);
        _rb.AddForce(new Vector2(0, _jumpStrength), ForceMode2D.Impulse);
    }
    public void MoveRB(Vector2 pos)
    {
        _rb.MovePosition(pos);
    }
    public void Push(Vector3 PushForce, IPusher playerPusher)
    {
        Stop();
        _rb.AddForce(PushForce, ForceMode2D.Impulse);

    }
    public void SetRB(bool isdynamic)
    {
        if (isdynamic) _rb.bodyType = RigidbodyType2D.Dynamic;
        else _rb.bodyType = RigidbodyType2D.Kinematic;
    }
    public void SetRBVelocity(Vector2 velocity)
    {
        _rb.velocity = velocity;
    }
    public void Push(enemyDirection pushDirection, Vector3 PushForce, IPusher playerPusher)
    {
        Stop();
        if (pushDirection == enemyDirection.RIGHT)
        {
            PushForce = new Vector3(Mathf.Abs(PushForce.x), PushForce.y, PushForce.z);
        }
        else
        {
            PushForce = new Vector3(-Mathf.Abs(PushForce.x), PushForce.y, PushForce.z);
        }
        _rb.AddForce(PushForce, ForceMode2D.Impulse);

    }
}
