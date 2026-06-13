using System;
using ArenaShooter.Configs;
using UnityEngine;

namespace ArenaShooter.Gameplay.Hero
{
    public class HeroMover
    {
        private readonly Rigidbody _rigidbody;
        private readonly HeroConfig _config;

        public HeroMover(HeroView heroView, HeroConfig config)
        {
            if (!heroView) throw new ArgumentNullException(nameof(heroView), "[HeroMover] HeroView cannot be null!");
            
            _rigidbody = heroView.Rigidbody ?? throw new ArgumentNullException(nameof(heroView.Rigidbody), "[HeroMover] Rigidbody is missing on HeroView!");
            _config = config ?? throw new ArgumentNullException(nameof(config), "[HeroMover] HeroConfig cannot be null!");
        }

        public void Move(Vector2 direction, float fixedDeltaTime)
        {
            Vector3 movement = new Vector3(direction.x, 0f, direction.y) * (_config.MoveSpeed * fixedDeltaTime);
            
            _rigidbody.MovePosition(_rigidbody.position + movement);
        }

        public void RotateTowards(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - _rigidbody.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                _rigidbody.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}