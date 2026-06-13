using System;
using ArenaShooter.Configs;
using ArenaShooter.Infrastructure.Pooling;
using UnityEngine;

namespace ArenaShooter.Gameplay.Enemies
{
    public class Enemy : MonoBehaviour, IPoolable<Enemy>
    {
        [SerializeField] private EnemyConfig _config;

        public bool IsActive { get; private set; }
        public EnemyConfig Config => _config;

        public void Initialize(Vector3 position)
        {
            transform.position = position;
        }

        public void Spawn()
        {
            IsActive = true;
        }

        public void Despawn()
        {
            IsActive = false;
        }

        public void TickUpdate(Vector3 targetPosition, float deltaTime)
        {
            Vector3 currentPosition = transform.position;
            Vector3 direction = targetPosition - currentPosition;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                direction.Normalize();
                
                transform.forward = direction;
                
                transform.Translate(direction * (_config.MoveSpeed * deltaTime), Space.World);
            }
        }

        private void OnValidate()
        {
            if (!_config) Debug.LogError("[Enemy] EnemyConfig is missing!", this);
        }
    }
}