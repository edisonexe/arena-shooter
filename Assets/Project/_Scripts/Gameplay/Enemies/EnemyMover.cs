using UnityEngine;

namespace ArenaShooter.Gameplay.Enemies
{
    public class EnemyMover
    {
        private Transform _transform;
        private float _moveSpeed;
        
        public void Configure(Transform transform, float moveSpeed)
        {
            _transform = transform;
            _moveSpeed = moveSpeed;
        }

        public void MoveTowards(Vector3 targetPosition, float deltaTime)
        {
            if (_transform == null) return;

            Vector3 currentPosition = _transform.position;
            Vector3 direction = targetPosition - currentPosition;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                direction.Normalize();
                _transform.forward = direction;
                _transform.Translate(direction * (_moveSpeed * deltaTime), Space.World);
            }
        }
    }
}