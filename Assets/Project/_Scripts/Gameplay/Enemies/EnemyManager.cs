using System;
using System.Collections.Generic;
using ArenaShooter.Gameplay.Hero;
using ArenaShooter.Infrastructure.Pooling;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Gameplay.Enemies
{
    public class EnemyManager : ITickable
    {
        private readonly ObjectPool<Enemy> _enemyPool;
        private readonly HeroView _heroView;
        private readonly List<Enemy> _activeEnemies = new List<Enemy>(256);

        private float _spawnTimer;
        private readonly float _spawnInterval = 2f;

        public EnemyManager(ObjectPool<Enemy> enemyPool, HeroView heroView)
        {
            _enemyPool = enemyPool ?? throw new ArgumentNullException(nameof(enemyPool), "[EnemyManager] Pool cannot be null!");
            _heroView = heroView ?? throw new ArgumentNullException(nameof(heroView), "[EnemyManager] HeroView cannot be null!");
        }

        public void Tick()
        {
            float deltaTime = Time.deltaTime;
            
            Vector3 heroPosition = _heroView.transform.position;
            
            for (int i = _activeEnemies.Count - 1; i >= 0; i--)
            {
                Enemy enemy = _activeEnemies[i];

                if (!enemy.IsActive)
                {
                    _activeEnemies.RemoveAt(i);
                    _enemyPool.Return(enemy);
                    continue;
                }

                enemy.TickUpdate(heroPosition, deltaTime);
            }
            
            _spawnTimer += deltaTime;
            if (_spawnTimer >= _spawnInterval)
            {
                _spawnTimer = 0f;
                SpawnEnemyWave();
            }
        }

        public Enemy GetClosestEnemy(Vector3 position, float maxRadius)
        {
            Enemy closestEnemy = null;
            float closestSqrDistance = maxRadius * maxRadius;

            for (int i = 0; i < _activeEnemies.Count; i++)
            {
                Enemy enemy = _activeEnemies[i];
                
                if (!enemy.IsActive) continue;

                float sqrDistance = (enemy.transform.position - position).sqrMagnitude;
                if (sqrDistance < closestSqrDistance)
                {
                    closestSqrDistance = sqrDistance;
                    closestEnemy = enemy;
                }
            }

            return closestEnemy;
        }
        
        private void SpawnEnemyWave()
        {
            Vector2 randomPoint = UnityEngine.Random.insideUnitCircle.normalized * 15f;
            Vector3 spawnPosition = _heroView.transform.position + new Vector3(randomPoint.x, 0f, randomPoint.y);

            Enemy enemy = _enemyPool.Get();
            enemy.Initialize(spawnPosition);
            _activeEnemies.Add(enemy);
        }
    }
}