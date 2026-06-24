using System;
using ArenaShooter.Features.CombatVisuals;
using ArenaShooter.Infrastructure.Signals;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Features.Hero.Components
{
    public class HeroHealthSystem : IInitializable, IDisposable, IDamageable
    {
        private readonly HeroView _view;
        private readonly SignalBus _signalBus;
        private readonly HeroRuntimeStats _runtimeStats;

        private Action<DamageTakenSignal> _onDamageSignalCache;
        private Action<float, float> _onHealthStatsChangedCache;
        
        public event Action<float, float> OnHealthChanged;

        public HeroHealthSystem(
            HeroView view, 
            SignalBus signalBus, 
            HeroRuntimeStats runtimeStats)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
            _runtimeStats = runtimeStats ?? throw new ArgumentNullException(nameof(runtimeStats));
        }

        public void Initialize()
        {
            _onDamageSignalCache = OnDamageSignalReceived;
            _signalBus.Subscribe(_onDamageSignalCache);
            

            _onHealthStatsChangedCache = HandleHealthStatsChanged;
            _runtimeStats.OnHealthStatsChanged += _onHealthStatsChangedCache;
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe(_onDamageSignalCache);
            _runtimeStats.OnHealthStatsChanged -= _onHealthStatsChangedCache;
        }

        public void TakeDamage(float amount, Vector3 damageSourcePosition)
        {
            if (_runtimeStats.CurrentHealth <= 0f) return;

            float newHealth = Mathf.Max(_runtimeStats.CurrentHealth - amount, 0f);
            _runtimeStats.SetCurrentHealth(newHealth);

            if (_view.VisualTilt)
            {
                Vector3 hitDirection = (_view.Rigidbody.position - damageSourcePosition);
                hitDirection.y = 0f;

                if (hitDirection.sqrMagnitude > 0.001f)
                {
                    _view.VisualTilt.ApplyDirectionalTilt(hitDirection.normalized);
                }
            }

            if (_runtimeStats.CurrentHealth <= 0f)
            {
                Die();
            }
        }

        public void ResetHealth()
        {
            _runtimeStats.SetCurrentHealth(_runtimeStats.MaxHealth);
        }

        private void HandleHealthStatsChanged(float currentHealth, float maxHealth)
        {
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
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