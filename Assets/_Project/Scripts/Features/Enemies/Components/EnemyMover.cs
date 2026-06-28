using System;
using UnityEngine;

namespace ArenaShooter.Features.Enemies.Components
{
    public class EnemyMover
    {
        private Rigidbody _rigidbody;
        private float _moveSpeed;
        
        public void Configure(Rigidbody rigidbody, float moveSpeed)
        {
            _rigidbody = rigidbody ?? throw new ArgumentNullException(nameof(rigidbody));
            _moveSpeed = moveSpeed;
        }

        public void MoveTowards(Vector3 targetPosition)
        {
            if (ReferenceEquals(_rigidbody, null) || !_rigidbody || _rigidbody.isKinematic) return;

            Vector3 currentPosition = _rigidbody.position;
            Vector3 direction = targetPosition - currentPosition;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                Vector3 moveDirection = direction.normalized;
                
                _rigidbody.transform.forward = moveDirection;
                
                _rigidbody.linearVelocity = moveDirection * _moveSpeed;
            }
            else
            {
                _rigidbody.linearVelocity = Vector3.zero;
            }
        }

        public void Stop()
        {
            if (_rigidbody && !_rigidbody.isKinematic)
            {
                _rigidbody.linearVelocity = Vector3.zero;
            }
        }
    }
}