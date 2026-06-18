using System;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Gameplay.Hero
{
    public class HeroRotationSystem : ITickable
    {
        private readonly HeroView _view;
        
        private const float RotSpeed = 15f;
        private Vector3 _lookTargetPosition;
        private bool _hasLookTarget;

        public HeroRotationSystem(HeroView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void SetLookTarget(Vector3 targetPosition)
        {
            _lookTargetPosition = targetPosition;
            _hasLookTarget = true;
        }

        public void Tick()
        {
            if (_view.VisualTilt) _view.VisualTilt.TickTilt(Time.deltaTime);
            
            if (!_hasLookTarget || !_view.VisualRoot) return;

            Vector3 direction = _lookTargetPosition - _view.Rigidbody.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                _view.VisualRoot.rotation = Quaternion.Slerp(
                    _view.VisualRoot.rotation, 
                    targetRotation, 
                    RotSpeed * Time.deltaTime
                );
            }

            _hasLookTarget = false;
        }
    }
}