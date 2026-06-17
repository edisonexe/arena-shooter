using ArenaShooter.Configs;
using ArenaShooter.Gameplay.Combat;
using ArenaShooter.Infrastructure.Pooling;
using UnityEngine;

namespace ArenaShooter.Gameplay.Enemies
{
    [RequireComponent(typeof(MeshRenderer), typeof(Collider))]
    public class Enemy : MonoBehaviour, IPoolable<Enemy>, IDamageable
    {
        [SerializeField] private EnemyConfig _config;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Collider _collider;

        private float _currentHealth;
        
        public bool IsActive { get; private set; }
        public EnemyConfig Config => _config;

        public void Initialize(Vector3 position)
        {
            transform.position = position;
            _currentHealth = _config.MaxHealth;
        }
        
        public void Spawn()
        {
            IsActive = true;
            
            _meshRenderer.enabled = true;
            _collider.enabled = true;
        }

        public void Despawn()
        {
            IsActive = false;
            
            _meshRenderer.enabled = false;
            _collider.enabled = false;
        }

        public void TakeDamage(float amount)
        {
            if (!IsActive) return;

            _currentHealth -= amount;
            if (_currentHealth <= 0f)
            {
                Despawn();
            }
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
            
            if (!(_meshRenderer ??= GetComponent<MeshRenderer>())) 
                Debug.LogError($"[Enemy] MeshRenderer is missing on '{name}'!", this);
            
            if (!(_collider ??= GetComponent<Collider>())) 
                Debug.LogError($"[Enemy] Collider2D is missing on '{name}'!", this);
        }
    }
}