using System;
using System.Collections.Generic;
using ArenaShooter.Configs;
using ArenaShooter.Infrastructure.Pooling;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Gameplay.Enemies
{
    public class EnemyFactory
    {
        private readonly DiContainer _container;
        private readonly Transform _poolsParent;
        private readonly Dictionary<EnemyView, ObjectPool<EnemyView>> _poolsMap = new(8);
        private readonly int _initialCapacity;
        
        public EnemyFactory(DiContainer container, Transform poolsParent, int initialCapacity)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _poolsParent = poolsParent ?? throw new ArgumentNullException(nameof(poolsParent));
            _initialCapacity = initialCapacity;
        }

        public EnemyEntity Create(Vector3 spawnPosition, EnemyConfig enemyConfig)
        {
            if (enemyConfig == null) throw new ArgumentNullException(nameof(enemyConfig));
            if (enemyConfig.Prefab == null) throw new MissingReferenceException(enemyConfig.name);
            
            ObjectPool<EnemyView> pool = GetOrCreatePool(enemyConfig.Prefab);

            EnemyView view = pool.Get();
            view.Transform.position = spawnPosition;
            view.Initialize();
            view.Spawn();
            
            return _container.Instantiate<EnemyEntity>(new object[] { view, enemyConfig });
        }

        public void Reclaim(EnemyEntity enemy)
        {
            if (enemy == null) return;

            enemy.View.Despawn();
            
            if (_poolsMap.TryGetValue(enemy.Config.Prefab, out ObjectPool<EnemyView> pool))
            {
                pool.Return(enemy.View);
            }
        }

        private ObjectPool<EnemyView> GetOrCreatePool(EnemyView prefab)
        {
            if (_poolsMap.TryGetValue(prefab, out ObjectPool<EnemyView> existingPool))
            {
                return existingPool;
            }
            
            var newPool = new ObjectPool<EnemyView>(prefab, _poolsParent, _initialCapacity);
            _poolsMap[prefab] = newPool;
            return newPool;
        }
    }
}