using System;
using System.Collections;
using UnityEngine;

public abstract class PlayerState
{
    public delegate PlayerState GetState(Type state);
    protected PlayerContext _context;
    protected GetState _getType;
    
    public PlayerState(GetState function)
    {
        _getType = function;
    }
    public virtual void SetUpState(PlayerContext context)
    {
        _context = context;
    }
   
    public abstract void Update();
    public virtual void FixedUpdate() { }
    public virtual void Move(Vector2 direction) { }
    public virtual void Jump() { }
    public virtual void Attack() { }
    public virtual void Dodge(){ }
    public abstract void InterruptState();
    public void ChangeState(Type newStateType)
    {
        PlayerState state = _getType(newStateType);
        _context.ChangePlayerState(state);
        state.SetUpState(_context);
    }
}
