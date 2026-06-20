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

        private float _currentHealth;

        public event Action<EnemyEntity> OnDespawnRequested;

        public EnemyConfig Config => _config;
        public EnemyView View => _view;
        public bool IsActive { get; private set; }

        public EnemyEntity(EnemyView view, EnemyConfig config)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _config = config ?? throw new ArgumentNullException(nameof(config));

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

            Transform viewTransform = _view.Transform;
            Vector3 currentPosition = viewTransform.position;
            Vector3 direction = targetPosition - currentPosition;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                direction.Normalize();
                viewTransform.forward = direction;
                viewTransform.Translate(direction * (_config.MoveSpeed * deltaTime), Space.World);
            }
        }

        private void Despawn()
        {
            IsActive = false;
            OnDespawnRequested?.Invoke(this);
        }
    }
}