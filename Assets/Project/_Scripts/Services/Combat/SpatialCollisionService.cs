using System;
using ArenaShooter.Gameplay.Enemies;
using UnityEngine;

namespace ArenaShooter.Services.Combat
{
    public class SpatialCollisionService
    {
        private readonly EnemyManager _enemyManager;
        private const float HitRadius = 0.8f;

        public SpatialCollisionService(EnemyManager enemyManager)
        {
            _enemyManager = enemyManager ?? throw new ArgumentNullException(nameof(enemyManager));
        }

        public bool CheckBulletHit(Vector3 bulletPosition, float damage)
        {
            EnemyEntity closestEnemy = _enemyManager.GetClosestEnemy(bulletPosition, HitRadius);

            if (closestEnemy != null)
            {
                closestEnemy.TakeDamage(damage, bulletPosition);
                return true;
            }

            return false;
        }
    }
}