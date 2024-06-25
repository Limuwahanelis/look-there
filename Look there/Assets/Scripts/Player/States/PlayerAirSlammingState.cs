using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirSlammingState : PlayerState
{
    bool _hasLanded = false;
    int _slamStep = 1;
    float _time = 0;
    float _animTime=0;
    public static Type StateType { get => typeof(PlayerAirSlammingState); }
    public PlayerAirSlammingState(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        if (_slamStep == 1)
        {
            if(_time> 0.08333333333333333f)
            {
                _context.playerMovement.MoveRB(new Vector2(_context.playerMovement.PlayerRB.position.x, _context.playerMovement.PlayerRB.position.y - _context.combat.SlamSpeed * Time.deltaTime));
                //Debug.Break();
            }
            if (_time > _animTime)
            {
                _context.animationManager.PlayAnimation("Air Slam Loop");
                _slamStep++;
            }
            if (_context.checks.IsOnGround||_context.checks.IsRay)
            {
                _context.playerMovement.StopPlayer();
                _context.playerMovement.SetRB(true);
                _context.animationManager.PlayAnimation("Air Slam Finish");
                _animTime = _context.animationManager.GetAnimationLength("Air Slam Finish");
                _slamStep=3;
                _time = 0;
            }
            _time += Time.deltaTime;
        }
        if (_slamStep == 2)
        {
            _context.playerMovement.MoveRB(new Vector2(_context.playerMovement.PlayerRB.position.x, _context.playerMovement.PlayerRB.position.y - _context.combat.SlamSpeed * Time.deltaTime));
            if (_context.checks.IsOnGround)
            {
                _context.playerMovement.StopPlayer();
                _context.playerMovement.SetRB(true);
                //Debug.Break();
                Logger.Log("Stop");
                _context.animationManager.PlayAnimation("Air Slam Finish");
                _animTime = _context.animationManager.GetAnimationLength("Air Slam Finish");
                _slamStep++;
                _time = 0;
            }
        }
        if (_slamStep == 3)
        {
            if (_time >= _animTime)
            {
                _context.collisions.SetPlayerFloorCollider(true);
                ChangeState(PlayerIdleState.StateType);
            }
            _time += Time.deltaTime;
        }
    }

    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
        _context.animationManager.PlayAnimation("Air Slam Start");
        _animTime = _context.animationManager.GetAnimationLength("Air Slam Start");
        _slamStep = 1;
        _context.playerMovement.SetRB(false);
        _context.playerMovement.StopPlayer();
        _context.collisions.SetPlayerFloorCollider(false);
        //_context.playerMovement.SetRBVelocity(new Vector2(0, -_context.combat.SlamSpeed));

        _time = 0;
    }

    public override void InterruptState()
    {
        _context.playerMovement.SetRB(true);
        _context.collisions.SetPlayerFloorCollider(true);
    }
}