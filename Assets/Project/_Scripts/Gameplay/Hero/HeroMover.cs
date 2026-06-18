using System;
using UnityEngine;

namespace ArenaShooter.Gameplay.Hero
{
    public class HeroMover
    {
        private readonly Rigidbody _rigidbody;
        private readonly HeroRuntimeStats _stats;
        
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
    }
}