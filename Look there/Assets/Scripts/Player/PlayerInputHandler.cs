using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] PlayerController _player;
    [SerializeField] InputActionAsset _controls;
    [SerializeField] PlayerWarpSkill _warpSkill;
    //private PlayerInteract _playerInteract;
    private bool isDownArrowPressed;
    private Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<PlayerController>();
        //_playerInteract = GetComponent<PlayerInteract>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.IsAlive)
        {
            if (!GlobalSettings.IsGamePaused)
            {
                _player.CurrentPlayerState.Move(direction);

            }
        }
    }
    private void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (GlobalSettings.IsGamePaused) return;
        //if (direction * _player.mainBody.transform.localScale.x > 0 && isDownArrowPressed) _player.currentState.Slide();
         _player.CurrentPlayerState.Jump();
    }
    void OnVertical(InputValue value)
    {
        direction = value.Get<Vector2>();
        Debug.Log(direction);
    }

    private void OnAttack(InputValue value)
    {
        if (GlobalSettings.IsGamePaused) return;
        if(direction.y>0) _player.CurrentPlayerState.Attack(PlayerCombat.AttackModifiers.UP_ARROW);
        if(direction.y<0) _player.CurrentPlayerState.Attack(PlayerCombat.AttackModifiers.DOWN_ARROW);
        else _player.CurrentPlayerState.Attack();
    }

    private void OnDownArrowModifier(InputValue value)
    {
        if (GlobalSettings.IsGamePaused) return;
        isDownArrowPressed = value.Get<float>() == 1 ? true : false;
    }
    private void OnInteract(InputValue value)
    {
        if (GlobalSettings.IsGamePaused) return;
        //_playerInteract.InteractWithObject();
    }
    private void OnWarp(InputValue value)
    {
        if (GlobalSettings.IsGamePaused) return;
        _player.CurrentPlayerState.Dodge();
    }
}
