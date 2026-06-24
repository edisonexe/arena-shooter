using System;
using ArenaShooter.Features.Enemies.Components;
using ArenaShooter.Features.Hero.Components;
using ArenaShooter.Features.Weapons.Configs;
using UnityEngine;

namespace ArenaShooter.Features.Weapons.Components
{
    public class AutoCombatWeapon
    {
        private readonly WeaponConfig _weaponConfig;
        private readonly BulletManager _bulletManager;
        private readonly EnemyManager _enemyManager;
        private readonly HeroRuntimeStats _runtimeStats;
        
        private float _fireTimer;

        public AutoCombatWeapon(
            WeaponConfig weaponConfig, 
            BulletManager bulletManager, 
            EnemyManager enemyManager, 
            HeroRuntimeStats runtimeStats)
        {
            _weaponConfig = weaponConfig ?? throw new ArgumentNullException(nameof(weaponConfig));
            _bulletManager = bulletManager ?? throw new ArgumentNullException(nameof(bulletManager));
            _enemyManager = enemyManager ?? throw new ArgumentNullException(nameof(enemyManager));
            _runtimeStats = runtimeStats ?? throw new ArgumentNullException(nameof(runtimeStats));
            
            _fireTimer = _runtimeStats.WeaponCooldown;
        }

        public void TickWeapon(Vector3 shooterPosition, Transform firePoint, Action<Vector3> onTargetLocked)
        {
            EnemyEntity target = _enemyManager.GetClosestEnemy(shooterPosition, _weaponConfig.FireRadius);

            if (target != null && target.IsActive)
            {
                Vector3 targetPosition = target.View.Transform.position;
                onTargetLocked?.Invoke(targetPosition);

                _fireTimer += Time.deltaTime;
                
                if (_fireTimer >= _runtimeStats.WeaponCooldown)
                {
                    _fireTimer = 0f;
                    ExecuteShoot(firePoint, targetPosition);
                }
            }
            else
            {
                _fireTimer = _runtimeStats.WeaponCooldown;
            }
        }

        private void ExecuteShoot(Transform firePoint, Vector3 targetPosition)
        {
            Vector3 firePointPosition = firePoint.position;
            Vector3 shootDirection = targetPosition - firePointPosition;
            shootDirection.y = 0f; 

            _bulletManager.FireBullet(
                firePointPosition, 
                shootDirection.normalized, 
                _weaponConfig.BulletSpeed, 
                _runtimeStats.BulletDamage
            );
        }
    }
}