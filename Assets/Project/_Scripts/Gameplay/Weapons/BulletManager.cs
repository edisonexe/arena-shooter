using System;
using System.Collections.Generic;
using ArenaShooter.Infrastructure.Pooling;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Gameplay.Weapons
{
    public class BulletManager : ITickable
    {
        private readonly ObjectPool<Bullet> _bulletPool;
        private readonly List<Bullet> _activeBullets = new List<Bullet>(128);

        public BulletManager(ObjectPool<Bullet> bulletPool)
        {
            _bulletPool = bulletPool ?? throw new ArgumentNullException(nameof(bulletPool), "[BulletManager] Pool cannot be null!");
        }

        public void Tick()
        {
            float deltaTime = Time.deltaTime;
            
            for (var i = _activeBullets.Count - 1; i >= 0; i--)
            {
                Bullet bullet = _activeBullets[i];

                if (!bullet.IsActive)
                {
                    _activeBullets.RemoveAt(i);
                    _bulletPool.Return(bullet);
                    continue;
                }

                bullet.TickUpdate(deltaTime);

                if (bullet.transform.position.sqrMagnitude > 2500f)
                {
                    bullet.Despawn();
                }
            }
        }

        public void FireBullet(Vector3 position, Vector3 direction, float speed)
        {
            Bullet bullet = _bulletPool.Get();
            bullet.Initialize(position, direction, speed);
            _activeBullets.Add(bullet);
        }
    }
}