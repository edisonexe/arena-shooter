using ArenaShooter.Infrastructure.Pooling;
using UnityEngine;

namespace ArenaShooter.Features.Weapons.Components
{
    [RequireComponent(typeof(MeshRenderer), typeof(Collider))]
    public class Bullet : MonoBehaviour, IPoolable
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Collider _collider;
        
        private Vector3 _direction;
        private float _speed;

        public float Damage { get; private set; }
        public bool IsActive { get; private set; }

        public void Initialize(Vector3 position, Vector3 direction, float speed, float damage)
        {
            transform.position = position;
            _direction = direction.normalized;
            _speed = speed;
            Damage = damage;
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

        public void TickUpdate(float deltaTime)
        {
            transform.Translate(_direction * (_speed * deltaTime), Space.World);
        }

        private void OnValidate()
        {
            if (!(_meshRenderer ??= GetComponent<MeshRenderer>())) 
                Debug.LogError($"[Bullet] MeshRenderer is missing on '{name}'!", this);
            
            if (!(_collider ??= GetComponent<Collider>())) 
                Debug.LogError($"[Bullet] Collider2D is missing on '{name}'!", this);
        }
    }
}