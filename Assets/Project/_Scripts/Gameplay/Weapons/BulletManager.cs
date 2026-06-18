using System;
using System.Collections.Generic;
using ArenaShooter.Infrastructure.Pooling;
using ArenaShooter.Infrastructure.Reset;
using ArenaShooter.Services.Combat;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Gameplay.Weapons
{
    public class BulletManager : ITickable, IResettable
    {
        private readonly ObjectPool<Bullet> _bulletPool;
        private readonly SpatialCollisionService _collisionService;
        private readonly List<Bullet> _activeBullets = new(128);
        
        public BulletManager(ObjectPool<Bullet> bulletPool, SpatialCollisionService collisionService)
        {
            _bulletPool = bulletPool ?? throw new ArgumentNullException(nameof(bulletPool));
            _collisionService = collisionService ?? throw new ArgumentNullException(nameof(collisionService));
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

                if (_collisionService.CheckBulletHit(bullet.transform.position, bullet.Damage))
                {
                    _activeBullets.RemoveAt(i);
                    _bulletPool.Return(bullet);
                    continue;
                }
                
                if (bullet.transform.position.sqrMagnitude > 2500f)
                {
                    _activeBullets.RemoveAt(i);
                    _bulletPool.Return(bullet);
                }
            }
        }

        public void FireBullet(Vector3 position, Vector3 direction, float speed, float damage)
        {
            Bullet bullet = _bulletPool.Get();
            bullet.Initialize(position, direction, speed, damage);
            _activeBullets.Add(bullet);
        }
        
        public void ResetState()
        {
            for (int i = _activeBullets.Count - 1; i >= 0; i--)
            {
                _bulletPool.Return(_activeBullets[i]);
            }
            _activeBullets.Clear();
        }
    }
}