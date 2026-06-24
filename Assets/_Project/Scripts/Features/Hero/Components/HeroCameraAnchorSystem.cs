using System;
using ArenaShooter.Features.Camera.Configs;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Features.Hero.Components
{
    public class HeroCameraAnchorSystem : ITickable
    {
        private readonly HeroView _view;
        private readonly CameraConfig _cameraConfig;

        public HeroCameraAnchorSystem(HeroView view, CameraConfig cameraConfig)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _cameraConfig = cameraConfig ?? throw new ArgumentNullException(nameof(cameraConfig));
        }

        public void Tick()
        {
            if (ReferenceEquals(_view.CameraAnchor, null) || !_view.CameraAnchor) return;

            _view.CameraAnchor.localPosition = Vector3.Lerp(
                _view.CameraAnchor.localPosition,
                Vector3.zero,
                _cameraConfig.AnchorLerpSpeed * Time.deltaTime
            );
        }
    }
}