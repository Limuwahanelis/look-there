using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerMovement : MonoBehaviour
{
    public enum playerDirection
    {
        LEFT = -1,
        SAME = 0,
        RIGHT = 1
    }
    public bool IsPlayerFalling { get=>_rb.velocity.y<0; }
    public Rigidbody2D PlayerRB =>_rb;
    public playerDirection newPlayerDirection;
    public playerDirection oldPlayerDirection;
    [SerializeField] float _playerSpeed;
    [SerializeField] float _jumpStrength;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] PlayerController _player;
    private float _previousDirection;
    private int _flipSide = 1;
    public void Move(float direction)
    {
        if (direction != 0)
        {
            oldPlayerDirection = newPlayerDirection;
            newPlayerDirection = (playerDirection)direction;
            _rb.velocity = new Vector3(direction * _playerSpeed, _rb.velocity.y, 0);
            if (direction > 0)
            {
                _flipSide = 1;
                _player.MainBody.transform.localScale = new Vector3(_flipSide, _player.MainBody.transform.localScale.y, _player.MainBody.transform.localScale.z);
            }
            if (direction < 0)
            {
                _flipSide = -1;
                _player.MainBody.transform.localScale = new Vector3(_flipSide, _player.MainBody.transform.localScale.y, _player.MainBody.transform.localScale.z);
            }

        }
        else
        {
            if (_previousDirection != 0)
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
                //StopPlayerOnXAxis();
            }
        }
        _previousDirection = direction;
    }
    public void StopPlayer(bool vertical=true,bool horizontal = true)
    {
        if (horizontal) _rb.velocity = new Vector2(0,_rb.velocity.y);
        if (vertical) _rb.velocity = new Vector2(_rb.velocity.x, 0);
    }
    public void Jump()
    {
        _rb.velocity = new Vector3(0, 0, 0);
        _rb.AddForce(new Vector2(0, _jumpStrength),ForceMode2D.Impulse);
    }
}
