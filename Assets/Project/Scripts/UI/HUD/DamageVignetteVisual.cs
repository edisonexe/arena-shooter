using System;
using ArenaShooter.Infrastructure.Signals;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace ArenaShooter.UI.HUD
{
    public class DamageVignetteVisual : IInitializable, IDisposable, ITickable
    {
        private readonly Volume _globalVolume;
        private readonly SignalBus _signalBus;
        
        private Vignette _vignette;
        private float _currentIntensity;
        private const float FADE_SPEED = 4f;
        private const float MAX_INTENS = 0.45f;
        
        private Action<DamageTakenSignal> _onDamageSignalCache;

        public DamageVignetteVisual(SignalBus signalBus, Volume globalVolume)
        {
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
            _globalVolume = globalVolume ?? throw new ArgumentNullException(nameof(globalVolume));
        }

        public void Initialize()
        {
            if (_globalVolume == null) return;
            
            VolumeProfile runtimeProfile = _globalVolume.profile;
            
            if (!runtimeProfile.TryGet(out _vignette))
            {
                Debug.LogError("[DamageVignetteVisual] Vignette override missing in Volume Profile!", _globalVolume);
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
            
            if (_globalVolume != null && !ReferenceEquals(_globalVolume, null))
            {
                _vignette.intensity.value = 0f;
            }
        }

        public void Tick()
        {
            if (_vignette == null || _currentIntensity <= 0f) return;

            _currentIntensity -= FADE_SPEED * Time.deltaTime;

            if (_currentIntensity < 0f)
            {
                _currentIntensity = 0f;
            }

            _vignette.intensity.value = _currentIntensity;
        }

        private void HandlePlayerDamage(DamageTakenSignal signal)
        {
            _currentIntensity = MAX_INTENS;
        }
    }
}