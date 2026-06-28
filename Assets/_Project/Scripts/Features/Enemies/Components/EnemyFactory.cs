using System;
using System.Collections.Generic;
using ArenaShooter.Features.Enemies.Configs;
using ArenaShooter.Infrastructure.Pooling;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Features.Enemies.Components
{
    public class EnemyFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly Transform _poolsParent;
        private readonly int _initialCapacity;
        
        private readonly Dictionary<EnemyConfig, ClassPool<EnemyEntity>> _poolsMap = new(8);
        
        public EnemyFactory(IInstantiator instantiator, Transform poolsParent, int initialCapacity)
        {
            _instantiator = instantiator ?? throw new ArgumentNullException(nameof(instantiator));
            _poolsParent = poolsParent ?? throw new ArgumentNullException(nameof(poolsParent));
            _initialCapacity = initialCapacity;
        }

        public EnemyEntity Create(Vector3 spawnPosition, EnemyConfig enemyConfig)
        {
            if (enemyConfig == null) throw new ArgumentNullException(nameof(enemyConfig));
            
            ClassPool<EnemyEntity> pool = GetOrCreatePool(enemyConfig);

            EnemyEntity entity = pool.Get();
            entity.SpawnAt(spawnPosition);
            
            return entity;
        }

        public void Reclaim(EnemyEntity enemy)
        {
            if (enemy == null) return;

            if (_poolsMap.TryGetValue(enemy.Config, out ClassPool<EnemyEntity> pool))
            {
                pool.Return(enemy);
            }
        }

        private ClassPool<EnemyEntity> GetOrCreatePool(EnemyConfig config)
        {
            if (_poolsMap.TryGetValue(config, out ClassPool<EnemyEntity> existingPool))
            {
                return existingPool;
            }
            
            var newPool = new ClassPool<EnemyEntity>(() => CreateNewEntity(config), _initialCapacity);
            _poolsMap[config] = newPool;
            return newPool;
        }

        private EnemyEntity CreateNewEntity(EnemyConfig config)
        {
            EnemyView view = UnityEngine.Object.Instantiate(config.Prefab, _poolsParent);
            view.Initialize();
            
            return _instantiator.Instantiate<EnemyEntity>(new object[] { view, config });
        }
    }
}