using System;
using ArenaShooter.Gameplay.Combat;
using UnityEngine;

namespace ArenaShooter.Gameplay.Hero
{
    public class HeroEntity : IDamageable
    {
        private readonly HeroHealthSystem _healthSystem;

        public event Action<float> OnHealthChanged
        {
            add => _healthSystem.OnHealthChanged += value;
            remove => _healthSystem.OnHealthChanged -= value;
        }

        public HeroEntity(HeroHealthSystem healthSystem)
        {
            _healthSystem = healthSystem ?? throw new ArgumentNullException(nameof(healthSystem));
        }

        public void TakeDamage(float amount, Vector3 damageSourcePosition)
        {
            _healthSystem.TakeDamage(amount, damageSourcePosition);
        }
    }
}