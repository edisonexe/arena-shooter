using System;
using ArenaShooter.Configs;
using ArenaShooter.Gameplay.Combat;
using UnityEngine;

namespace ArenaShooter.Gameplay.Enemies
{
    public class EnemyEntity : IDamageable
    {
        private readonly EnemyView _view;
        private readonly EnemyConfig _config;
        private readonly EnemyMover _mover;

        private float _currentHealth;

        public event Action<EnemyEntity> OnDespawnRequested;

        public EnemyConfig Config => _config;
        public EnemyView View => _view;
        public bool IsActive { get; private set; }
        
        public EnemyEntity(EnemyView view, EnemyConfig config, EnemyMover mover)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _mover = mover ?? throw new ArgumentNullException(nameof(mover));
            
            _mover.Configure(_view.Transform, _config.MoveSpeed);

            _currentHealth = _config.MaxHealth;
            IsActive = true;
        }

        public void TakeDamage(float amount, Vector3 damageSourcePosition)
        {
            if (!IsActive) return;

            _currentHealth -= amount;
            _view.ApplyVisualDamage(damageSourcePosition, 0.12f, 3);

            if (_currentHealth <= 0f)
            {
                Despawn();
            }
        }

        public void TickUpdate(Vector3 targetPosition, float deltaTime)
        {
            if (!IsActive) return;

            _view.TickVisuals(deltaTime);
            _mover.MoveTowards(targetPosition, deltaTime);
        }

        private void Despawn()
        {
            IsActive = false;
            OnDespawnRequested?.Invoke(this);
        }
    }
}