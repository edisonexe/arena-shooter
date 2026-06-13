using ArenaShooter.Infrastructure.Pooling;
using UnityEngine;

namespace ArenaShooter.Gameplay.Weapons
{
    public class Bullet : MonoBehaviour, IPoolable<Bullet>
    {
        private Vector3 _direction;
        private float _speed;

        public bool IsActive { get; private set; }

        public void Initialize(Vector3 position, Vector3 direction, float speed)
        {
            transform.position = position;
            _direction = direction.normalized;
            _speed = speed;
        }

        public void Spawn()
        {
            IsActive = true;
        }

        public void Despawn()
        {
            IsActive = false;
        }

        public void TickUpdate(float deltaTime)
        {
            transform.Translate(_direction * (_speed * deltaTime), Space.World);
        }
    }
}