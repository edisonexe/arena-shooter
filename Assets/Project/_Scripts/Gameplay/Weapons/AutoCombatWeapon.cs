using System;
using ArenaShooter.Configs;
using ArenaShooter.Gameplay.Enemies;
using UnityEngine;

namespace ArenaShooter.Gameplay.Weapons
{
    public class AutoCombatWeapon
    {
        private readonly BulletManager _bulletManager;
        private readonly EnemyManager _enemyManager;
        private readonly HeroConfig _config;
        
        private float _fireTimer;
        private const float MaxTargetDetectionRadius = 30f;

        public AutoCombatWeapon(BulletManager bulletManager, EnemyManager enemyManager, HeroConfig config)
        {
            _bulletManager = bulletManager ?? throw new ArgumentNullException(nameof(bulletManager));
            _enemyManager = enemyManager ?? throw new ArgumentNullException(nameof(enemyManager));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            
            _fireTimer = _config.FireRate;
        }

        public void TickWeapon(Vector3 shooterPosition, Transform firePoint, Action<Vector3> onTargetLocked)
        {
            Enemy target = _enemyManager.GetClosestEnemy(shooterPosition, MaxTargetDetectionRadius);

            if (target)
            {
                Vector3 targetPosition = target.transform.position;

                onTargetLocked?.Invoke(targetPosition);
                
                _fireTimer += Time.deltaTime;
                if (_fireTimer >= _config.FireRate)
                {
                    _fireTimer = 0f;
                    ExecuteShoot(firePoint, targetPosition);
                }
            }
            else
            {
                _fireTimer = _config.FireRate;
            }
        }

        private void ExecuteShoot(Transform firePoint, Vector3 targetPosition)
        {
            Vector3 firePointPosition = firePoint.position;
            Vector3 shootDirection = targetPosition - firePointPosition;
            shootDirection.y = 0f; 

            _bulletManager.FireBullet(firePointPosition, shootDirection, 20f);
        }
    }
}