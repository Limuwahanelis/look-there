using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContext
{
    public Action<PlayerState> ChangePlayerState;
    public Func<float, Action,Coroutine> WaitAndPerformFunction;
    public MonoBehaviour coroutineHolder;
    public AnimationManager animationManager;
    public PlayerMovement playerMovement;
    public PlayerChecks checks;
    public PlayerCombat combat;
}