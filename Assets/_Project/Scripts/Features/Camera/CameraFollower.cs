using System;
using ArenaShooter.Features.Hero.Components;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Features.Camera
{
    public class CameraFollower : ILateTickable, IInitializable
    {
        private readonly UnityEngine.Camera _mainCamera;
        private readonly HeroView _heroView;

        private Vector3 _cameraOffset;

        public CameraFollower(HeroView heroView)
        {
            _heroView = heroView ?? throw new ArgumentNullException(nameof(heroView));
            _mainCamera = UnityEngine.Camera.main ?? throw new InvalidOperationException("[CameraFollower] Main Camera not found!");
        }

        public void Initialize()
        {
            _cameraOffset = _mainCamera.transform.position - _heroView.CameraAnchor.position;
        }

        public void LateTick()
        {
            if (ReferenceEquals(_heroView, null) || !_heroView) return;
            
            _mainCamera.transform.position = _heroView.CameraAnchor.position + _cameraOffset;
        }
    }
}