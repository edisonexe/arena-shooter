using ArenaShooter.Infrastructure.Pooling;
using UnityEngine;

namespace ArenaShooter.Gameplay.Weapons
{
    public class Bullet : MonoBehaviour, IPoolable<Bullet>
    {
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

        public void Spawn() => IsActive = true;

        public void Despawn() => IsActive = false;

        public void TickUpdate(float deltaTime)
        {
            transform.Translate(_direction * (_speed * deltaTime), Space.World);
        }
    }
}