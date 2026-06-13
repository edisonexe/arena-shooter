using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ArenaShooter.Services.Input
{
    public class NewInputService : IInputService, IInitializable, IDisposable
    {
        private GameInput _gameInput;
        private Vector2 _axis;

        public Vector2 Axis => _axis;

        public void Initialize()
        {
            _gameInput = new GameInput();
            _gameInput.Player.Enable();
            
            _gameInput.Player.Move.performed += OnMovePerformed;
            _gameInput.Player.Move.canceled += OnMoveCanceled;
        }

        public void Dispose()
        {
            if (_gameInput != null)
            {
                _gameInput.Player.Move.performed -= OnMovePerformed;
                _gameInput.Player.Move.canceled -= OnMoveCanceled;
                
                _gameInput.Player.Disable();
                _gameInput.Dispose();
            }
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            _axis = context.ReadValue<Vector2>();
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            _axis = Vector2.zero;
        }
    }
}