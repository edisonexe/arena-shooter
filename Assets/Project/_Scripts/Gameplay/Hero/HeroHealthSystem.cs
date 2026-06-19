using System;
using ArenaShooter.Infrastructure.Signals;
using ArenaShooter.Services.Progression;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Gameplay.Hero
{
    public class HeroHealthSystem : IInitializable, IDisposable
    {
        private readonly HeroView _view;
        private readonly SignalBus _signalBus;
        private readonly HeroRuntimeStats _runtimeStats;
        private readonly HeroStatsModifierService _modifierService;

        private Action<DamageTakenSignal> _onDamageSignalCache;
        private Action _onModifiersAppliedCache;
        
        public event Action<float> OnHealthChanged;

        public HeroHealthSystem(
            HeroView view, 
            SignalBus signalBus, 
            HeroRuntimeStats runtimeStats, 
            HeroStatsModifierService modifierService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
            _runtimeStats = runtimeStats ?? throw new ArgumentNullException(nameof(runtimeStats));
            _modifierService = modifierService ?? throw new ArgumentNullException(nameof(modifierService));
        }

        public void Initialize()
        {
            _onDamageSignalCache = OnDamageSignalReceived;
            _signalBus.Subscribe(_onDamageSignalCache);
            
            _onModifiersAppliedCache = ForceUpdateHealthVisual;
            _modifierService.OnModifiersApplied += _onModifiersAppliedCache;
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe(_onDamageSignalCache);
        }

        public void TakeDamage(float amount, Vector3 damageSourcePosition)
        {
            if (_runtimeStats.CurrentHealth <= 0f) return;

            _modifierService.ApplyDamage(amount);

            if (_view.VisualTilt)
            {
                Vector3 hitDirection = (_view.Rigidbody.position - damageSourcePosition);
                hitDirection.y = 0f;

                if (hitDirection.sqrMagnitude > 0.001f)
                {
                    _view.VisualTilt.ApplyDirectionalTilt(hitDirection.normalized);
                }
            }
            
            ForceUpdateHealthVisual();

            if (_runtimeStats.CurrentHealth <= 0f)
            {
                Die();
            }
        }

        public void ResetHealth()
        {
            _runtimeStats.SetCurrentHealth(_runtimeStats.MaxHealth);
            OnHealthChanged?.Invoke(1f);
        }
        
        public void ForceUpdateHealthVisual()
        {
            float normalizedHealth = _runtimeStats.MaxHealth > 0f ? _runtimeStats.CurrentHealth / _runtimeStats.MaxHealth : 0f;
            OnHealthChanged?.Invoke(normalizedHealth);
        }
        
        private void Die()
        {
            _signalBus.Fire(new PlayerDiedSignal());
            _view.gameObject.SetActive(false);
        }
        
        private void OnDamageSignalReceived(DamageTakenSignal signal)
        {
            TakeDamage(signal.Amount, signal.OriginPosition);
        }
    }
}