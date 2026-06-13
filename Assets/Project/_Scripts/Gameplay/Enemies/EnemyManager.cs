using System;
using System.Collections.Generic;
using ArenaShooter.Gameplay.Hero;
using ArenaShooter.Infrastructure.Pooling;
using ArenaShooter.Infrastructure.Signals;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Gameplay.Enemies
{
    public class EnemyManager : ITickable
    {
        private readonly ObjectPool<Enemy> _enemyPool;
        private readonly HeroView _heroView;
        private readonly List<Enemy> _activeEnemies = new(256);

        private readonly SignalBus _signalBus;

        private float _spawnTimer;
        private readonly float _spawnInterval = 0.3f;

        private float _damageTimer;
        private const float DamageInterval = 0.5f;
        private const float AttackRadiusSqr = 1.0f;

        public EnemyManager(ObjectPool<Enemy> enemyPool, HeroView heroView, SignalBus signalBus)
        {
            _enemyPool = enemyPool ?? throw new ArgumentNullException(nameof(enemyPool));
            _heroView = heroView ?? throw new ArgumentNullException(nameof(heroView));
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
        }

        public void Tick()
        {
            float deltaTime = Time.deltaTime;
            Vector3 heroPosition = _heroView.transform.position;

            _damageTimer += deltaTime;
            bool canAttackThisFrame = _damageTimer >= DamageInterval;
            if (canAttackThisFrame) _damageTimer = 0f;

            for (int i = _activeEnemies.Count - 1; i >= 0; i--)
            {
                Enemy enemy = _activeEnemies[i];

                if (!enemy.IsActive)
                {
                    _signalBus.Fire(new EnemyKilledSignal(enemy.transform.position, enemy.Config.XpValue));
                    
                    _activeEnemies.RemoveAt(i);
                    _enemyPool.Return(enemy);
                    continue;
                }

                enemy.TickUpdate(heroPosition, deltaTime);

                if (canAttackThisFrame)
                {
                    float sqrDistanceToHero = (enemy.transform.position - heroPosition).sqrMagnitude;
                    if (sqrDistanceToHero <= AttackRadiusSqr)
                    {
                        _signalBus.Fire(new DamageTakenSignal(enemy.Config.Damage));
                    }
                }
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