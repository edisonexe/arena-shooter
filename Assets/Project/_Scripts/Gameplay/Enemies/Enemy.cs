using ArenaShooter.Configs;
using ArenaShooter.Gameplay.Combat;
using ArenaShooter.Infrastructure.Pooling;
using UnityEngine;

namespace ArenaShooter.Gameplay.Enemies
{
    [RequireComponent(typeof(Collider), typeof(DamageVisualTilt), typeof(HitFlashVisual))]
    public class Enemy : MonoBehaviour, IPoolable<Enemy>, IDamageable
    {
        [SerializeField] private EnemyConfig _config;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Collider _collider;
        [SerializeField] private DamageVisualTilt _visualTilt;
        [SerializeField] private HitFlashVisual _hitFlash;
        
        private float _currentHealth;
        
        public bool IsActive { get; private set; }
        public EnemyConfig Config => _config;

        public void Initialize(Vector3 position)
        {
            transform.position = position;
            _currentHealth = _config.MaxHealth;
            
            if (_hitFlash) _hitFlash.Initialize();
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

        public void TakeDamage(float amount, Vector3 damageSourcePosition)
        {
            if (!IsActive) return;

            _currentHealth -= amount;

            if (_visualTilt)
            {
                Vector3 hitDirection = (transform.position - damageSourcePosition);
                hitDirection.y = 0f;
                
                if (hitDirection.sqrMagnitude > 0.001f)
                {
                    _visualTilt.ApplyDirectionalTilt(hitDirection.normalized);
                }
            };
            
            if (_hitFlash) _hitFlash.PlayFlash(0.12f);
            
            if (_currentHealth <= 0f) Despawn();
        }
        
        public void TickUpdate(Vector3 targetPosition, float deltaTime)
        {
            _visualTilt.TickTilt(deltaTime);
            
            if (_hitFlash) _hitFlash.TickFlash(deltaTime);
            
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
            
            if (!_meshRenderer) Debug.LogError($"[Enemy] MeshRenderer is missing on '{name}'!", this);
            
            if (!(_collider ??= GetComponent<Collider>())) 
                Debug.LogError($"[Enemy] Collider is missing on '{name}'!", this);
            
            if (!(_visualTilt ??= GetComponent<DamageVisualTilt>())) 
                Debug.LogError($"[Enemy] DamageVisualTilt is missing on '{name}'!", this);
            
            if (!(_hitFlash ??= GetComponent<HitFlashVisual>())) Debug.LogError($"[Enemy] HitFlashVisual is missing!", this);
        }
    }
}