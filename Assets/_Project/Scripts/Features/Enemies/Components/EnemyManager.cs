using System;
using System.Collections.Generic;
using ArenaShooter.Features.Hero.Components;
using ArenaShooter.Infrastructure.Reset;
using ArenaShooter.Infrastructure.Signals;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Features.Enemies.Components
{
    public class EnemyManager : ITickable, IFixedTickable, IResettable
    {
        private readonly EnemyFactory _enemyFactory;
        private readonly HeroView _heroView;
        private readonly SignalBus _signalBus;

        private readonly List<EnemyEntity> _activeEnemies = new(256);
        private readonly List<EnemyEntity> _despawnBuffer = new(16);
        
        private float _damageTimer;
        private const float DAMAGE_INTERVAL = 0.5f;
        
        public List<EnemyEntity> ActiveEnemies => _activeEnemies;
        
        public EnemyManager(EnemyFactory enemyFactory, HeroView heroView, SignalBus signalBus)
        {
            _enemyFactory = enemyFactory ?? throw new ArgumentNullException(nameof(enemyFactory));
            _heroView = heroView ?? throw new ArgumentNullException(nameof(heroView));
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
        }

        public void Tick()
        {
            float deltaTime = Time.deltaTime;
            Vector3 heroPosition = _heroView.Rigidbody.position;

            _damageTimer += deltaTime;
            bool canAttackThisFrame = _damageTimer >= DAMAGE_INTERVAL;
            if (canAttackThisFrame) _damageTimer = 0f;

            _despawnBuffer.Clear();
            
            for (int i = 0; i < _activeEnemies.Count; i++)
            {
                EnemyEntity enemy = _activeEnemies[i];

                if (!enemy.IsActive)
                {
                    _despawnBuffer.Add(enemy);
                    continue;
                }
                
                enemy.TickVisuals(deltaTime);
                
                if (canAttackThisFrame)
                {
                    Vector3 enemyPosition = enemy.View.Transform.position;
                    enemyPosition.y = heroPosition.y;
    
                    float sqrDistanceToHero = (enemyPosition - heroPosition).sqrMagnitude;
                    if (sqrDistanceToHero <= enemy.Config.AttackRadiusSqr)
                    {
                        _signalBus.Fire(new DamageTakenSignal(enemy.Config.Damage, enemy.View.Transform.position));
                    }
                }
            }
            
            for (int i = 0; i < _despawnBuffer.Count; i++)
            {
                ProcessEnemyDeath(_despawnBuffer[i]);
            }
        }
        
        public void FixedTick()
        {
            if (ReferenceEquals(_heroView, null) || !_heroView) return;
            
            Vector3 heroPosition = _heroView.transform.position;
            
            int count = _activeEnemies.Count;
            for (int i = 0; i < count; i++)
            {
                EnemyEntity enemy = _activeEnemies[i];
                if (enemy == null || !enemy.IsActive) continue;

                enemy.FixedTickUpdate(heroPosition);
            }
        }
        
        public void AddEnemy(EnemyEntity enemy)
        {
            if (enemy == null) return;
            _activeEnemies.Add(enemy);
        }
        
        public EnemyEntity GetClosestEnemy(Vector3 position, float maxRadius)
        {
            EnemyEntity closestEnemy = null;
            float closestSqrDistance = maxRadius * maxRadius;

            for (var i = 0; i < _activeEnemies.Count; i++)
            {
                EnemyEntity enemy = _activeEnemies[i];
                if (!enemy.IsActive) continue;

                Vector3 enemyPosition = enemy.View.Transform.position;
                enemyPosition.y = position.y;
                
                float sqrDistance = (enemyPosition - position).sqrMagnitude;
                if (sqrDistance < closestSqrDistance)
                {
                    closestSqrDistance = sqrDistance;
                    closestEnemy = enemy;
                }
            }
            return closestEnemy;
        }

        private void ProcessEnemyDeath(EnemyEntity enemy)
        {
            _signalBus.Fire(new EnemyKilledSignal(enemy.View.Transform.position, enemy.Config.XpValue));
            _activeEnemies.Remove(enemy);
            _enemyFactory.Reclaim(enemy);
        }
        
        public void ResetState()
        {
            for (int i = _activeEnemies.Count - 1; i >= 0; i--)
            {
                _enemyFactory.Reclaim(_activeEnemies[i]);
            }
            _activeEnemies.Clear();
        }
    }
}