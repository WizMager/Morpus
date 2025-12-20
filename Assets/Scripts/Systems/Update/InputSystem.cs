using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class InputSystem : ISystem
{
    private InputActions _inputActions;
    private Filter _playerFilter;
    private Stash<MoveDirectionComponent> _moveDirectionStash;

    private bool _isMoveActive;
    
    public World World { get; set;}

    public void OnAwake()
    {
        _playerFilter = World.Filter.With<PlayerComponent>().Build();
        _moveDirectionStash = World.GetStash<MoveDirectionComponent>();
        
        _inputActions = new InputActions();
        _inputActions.Enable();

        _inputActions.Movement.Move.performed += OnMoveEnable;
        _inputActions.Movement.Move.canceled += OnMoveDisable;
    }

    private void OnMoveEnable(InputAction.CallbackContext obj)
    {
        if (_isMoveActive)
            return;
        
        _isMoveActive = true;
    }

    private void OnMoveDisable(InputAction.CallbackContext obj)
    {
        if (!_isMoveActive)
            return;
        
        _isMoveActive = false;
        
        _moveDirectionStash.Set(_playerFilter.First(), new MoveDirectionComponent
        {
            direction = Vector2.zero
        });
    }

    public void OnUpdate(float deltaTime) 
    {
        if (!_isMoveActive)
            return;
        
        _moveDirectionStash.Set(_playerFilter.First(), new MoveDirectionComponent
        {
            direction = _inputActions.Movement.Move.ReadValue<Vector2>()
        });
    }

    public void Dispose()
    {
        _inputActions.Disable();
#if UNITY_EDITOR
        return; //Disable error in editor
#endif
        _inputActions.Dispose();
    }
}