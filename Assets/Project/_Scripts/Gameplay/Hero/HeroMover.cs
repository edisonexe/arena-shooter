using System;
using ArenaShooter.Configs;
using ArenaShooter.Services.Progression;
using UnityEngine;

namespace ArenaShooter.Gameplay.Hero
{
    public class HeroMover
    {
        private readonly Rigidbody _rigidbody;
        private readonly HeroRuntimeStats _stats;
        
        private const float ROT_SPEED = 15f;
        
        public HeroMover(Rigidbody rigidbody, HeroRuntimeStats stats)
        {
            _rigidbody = rigidbody ?? throw new ArgumentNullException(nameof(rigidbody));
            _stats = stats ?? throw new ArgumentNullException(nameof(stats));
        }

        public void Move(Vector2 direction, float fixedDeltaTime)
        {
            Vector3 movement = new Vector3(direction.x, 0f, direction.y) * (_stats.MoveSpeed * fixedDeltaTime);
            
            _rigidbody.MovePosition(_rigidbody.position + movement);
        }

        public void RotateTowards(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - _rigidbody.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                
                Quaternion smoothedRotation = Quaternion.Slerp(
                    _rigidbody.rotation, 
                    targetRotation, 
                    ROT_SPEED * Time.deltaTime
                );
                _rigidbody.MoveRotation(smoothedRotation);
            }
        }
    }
}