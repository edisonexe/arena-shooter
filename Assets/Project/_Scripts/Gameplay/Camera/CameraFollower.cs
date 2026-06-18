using System;
using ArenaShooter.Gameplay.Hero;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Gameplay.Camera
{
    public class CameraFollower : IFixedTickable, IInitializable
    {
        private readonly UnityEngine.Camera _mainCamera;
        private readonly HeroView _heroView;

        private Vector3 _cameraOffset;
        private Vector3 _currentVelocity;
        private Vector3 _smoothedPosition;
        
        private const float SmoothTime = 0.05f; 

        public CameraFollower(HeroView heroView)
        {
            _heroView = heroView ?? throw new ArgumentNullException(nameof(heroView));
            _mainCamera = UnityEngine.Camera.main ?? throw new InvalidOperationException("[CameraFollower] Main Camera not found!");
        }

        public void Initialize()
        {
            _cameraOffset = _mainCamera.transform.position - _heroView.transform.position;
            _smoothedPosition = _mainCamera.transform.position;
        }

        public void FixedTick()
        {
            if (!_heroView) return;
            
            Vector3 targetPosition = _heroView.Rigidbody.position + _cameraOffset;
            
            _smoothedPosition = Vector3.SmoothDamp(
                _smoothedPosition,
                targetPosition,
                ref _currentVelocity,
                SmoothTime,
                float.PositiveInfinity,
                Time.fixedDeltaTime
            );

            _mainCamera.transform.position = _smoothedPosition;
        }
    }
}