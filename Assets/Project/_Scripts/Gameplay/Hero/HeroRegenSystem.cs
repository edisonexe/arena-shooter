using System;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Gameplay.Hero
{
    public class HeroRegenSystem : ITickable
    {
        private readonly HeroRuntimeStats _runtimeStats;
        private readonly HeroHealthSystem _healthSystem;

        public HeroRegenSystem(HeroRuntimeStats runtimeStats, HeroHealthSystem healthSystem)
        {
            _runtimeStats = runtimeStats ?? throw new ArgumentNullException(nameof(runtimeStats));
            _healthSystem = healthSystem ?? throw new ArgumentNullException(nameof(healthSystem));
        }
        
        public void Tick()
        {
            if (_runtimeStats.CurrentHealth <= 0f) return;
            if (_runtimeStats.CurrentHealth >= _runtimeStats.MaxHealth) return;
            if (_runtimeStats.HealthRegen <= 0f) return;

            float addedHealth = _runtimeStats.HealthRegen * Time.deltaTime;
            float newHealth = Mathf.Min(_runtimeStats.CurrentHealth + addedHealth, _runtimeStats.MaxHealth);
            _runtimeStats.SetCurrentHealth(newHealth);
            
            _healthSystem.ForceUpdateHealthVisual();
        }
    }
}