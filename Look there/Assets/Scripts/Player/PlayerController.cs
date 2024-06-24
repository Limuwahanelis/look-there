using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Debug"), SerializeField] bool _printState;
    public bool IsAlive => _isAlive;
    public PlayerState CurrentPlayerState => _currentPlayerState;
    public GameObject MainBody=>_mainBody;
    [SerializeField] Animator _anim;
    [SerializeField] GameObject _mainBody;
    [SerializeField] AnimationManager _playerAnimationManager;
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] PlayerChecks _playerChecks;
    [SerializeField] PlayerCombat _playerCombat;
    private PlayerState _currentPlayerState;
    private PlayerContext _context;
    private Dictionary<Type, PlayerState> playerStates = new Dictionary<Type, PlayerState>();
    private bool _isAlive=true;
    void Start()
    {
        List<Type> states = AppDomain.CurrentDomain.GetAssemblies().SelectMany(domainAssembly => domainAssembly.GetTypes())
            .Where(type => typeof(PlayerState).IsAssignableFrom(type) && !type.IsAbstract).ToArray().ToList();

        _context = new PlayerContext
        {
            ChangePlayerState = ChangeState,
            animationManager = _playerAnimationManager,
            playerMovement = _playerMovement,
            WaitAndPerformFunction = WaitAndExecuteFunction,
            coroutineHolder = this,
            checks = _playerChecks,
            combat = _playerCombat,
            canPerformAirCombo = true,
        };
        PlayerState.GetState getState = GetState;
        foreach (Type state in states)
        {
            playerStates.Add(state, (PlayerState)Activator.CreateInstance(state, getState));
        }
        PlayerState newState = GetState(typeof(PlayerIdleState));
        newState.SetUpState(_context);
        _currentPlayerState = newState;
    }

    public PlayerState GetState(Type state)
    {
        return playerStates[state];
    }
    void Update()
    {
        _currentPlayerState.Update();
    }
    private void FixedUpdate()
    {
        _currentPlayerState.FixedUpdate();
    }
    public void ChangeState(PlayerState newState)
    {
        if (_printState) Debug.Log(newState.GetType());
        _currentPlayerState.InterruptState();
        _currentPlayerState = newState;
    }
    public Coroutine WaitAndExecuteFunction(float timeToWait, Action function)
    {
        return StartCoroutine(DelyedFunction( timeToWait, function));
    }
    public IEnumerator DelyedFunction(float timeToWait, Action function)
    {
        yield return new WaitForSeconds(timeToWait);
        function();
    }
}
