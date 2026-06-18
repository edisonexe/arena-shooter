using System;
using ArenaShooter.Infrastructure.Signals;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace ArenaShooter.UI.HUD
{
    public class VolumeVignetteVisual : IInitializable, IDisposable, ITickable
    {
        private readonly Volume _globalVolume;
        private readonly SignalBus _signalBus;
        
        private Vignette _vignette;
        private float _currentIntensity;
        private const float FadeSpeed = 4f;
        private const float MaxIntensity = 0.45f;
        
        private Action<DamageTakenSignal> _onDamageSignalCache;

        public VolumeVignetteVisual(SignalBus signalBus, Volume globalVolume)
        {
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
            _globalVolume = globalVolume ?? throw new ArgumentNullException(nameof(globalVolume));
        }

        public void Initialize()
        {
            if (!_globalVolume.profile.TryGet(out _vignette))
            {
                Debug.LogError("[VolumeVignetteVisual] Vignette override missing in Volume Profile!");
                return;
            }

            _vignette.intensity.overrideState = true;
            _vignette.intensity.value = 0f;

            _onDamageSignalCache = HandlePlayerDamage;
            _signalBus.Subscribe(_onDamageSignalCache);
        }

        public void Dispose()
        {
            _signalBus?.Unsubscribe(_onDamageSignalCache);
            
            if (_vignette != null)
            {
                _vignette.intensity.value = 0f;
            }
        }

        public void Tick()
        {
            if (_vignette == null || _currentIntensity <= 0f) return;

            _currentIntensity -= FadeSpeed * Time.deltaTime;

            if (_currentIntensity < 0f)
            {
                _currentIntensity = 0f;
            }

            _vignette.intensity.value = _currentIntensity;
        }

        private void HandlePlayerDamage(DamageTakenSignal signal)
        {
            _currentIntensity = MaxIntensity;
        }
    }
}