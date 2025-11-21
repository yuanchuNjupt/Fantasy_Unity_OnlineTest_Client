using System.Collections;
using System.Collections.Generic;
using GGG.Tool.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : Singleton<GameInputManager>
{
    private GameInputAction _gameInputAction;
    
    public Vector2 Movement => _gameInputAction.GameInput.Movement.ReadValue<Vector2>();
    public Vector2 CameraLook => _gameInputAction.GameInput.CameraLook.ReadValue<Vector2>();
    public bool Run => _gameInputAction.GameInput.Run.triggered;
    
    public bool Climb => _gameInputAction.GameInput.Climb.triggered;
    
    public bool Grab => _gameInputAction.GameInput.Grab.triggered;
    
    public bool LAttack => _gameInputAction.GameInput.LAttack.triggered;
    
    public bool RAttack => _gameInputAction.GameInput.RAttack.triggered;
    
    public bool TakeOut => _gameInputAction.GameInput.TakeOut.triggered;
    //按住
    public bool Parry => _gameInputAction.GameInput.Parry.phase == InputActionPhase.Performed;
    
    
    
    protected override void Awake()
    {
        if (_gameInputAction == null)
        {
            _gameInputAction = new GameInputAction();
        }
    }
    
    private void OnEnable()
    {
        _gameInputAction.Enable();
    }

    private void OnDisable()
    {
        _gameInputAction.Disable();
    }

}
