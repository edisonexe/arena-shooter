using System;
using ArenaShooter.Gameplay.Combat;

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

        public void TakeDamage(float amount)
        {
            _healthSystem.TakeDamage(amount);
        }
    }
}