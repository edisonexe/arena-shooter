using System;
using ArenaShooter.Configs.Enemies;
using ArenaShooter.Gameplay.Hero;
using ArenaShooter.Infrastructure.Pooling;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Gameplay.Enemies
{
    public class EnemyWaveSpawner : ITickable
    {
        private readonly WaveConfig _config;
        private readonly ObjectPool<Enemy> _enemyPool;
        private readonly EnemyManager _enemyManager;
        private readonly Transform _heroTransform;

        private int _currentWave = 1;
        private float _gameTimeTimer;
        private float _spawnCooldownTimer;
        
        private float _currentSpawnInterval;
        private int _secondsInCurrentWave;
        
        public event Action<int> OnWaveChanged;
        public event Action<float> OnGameTimeUpdated;

        public EnemyWaveSpawner(
            WaveConfig config, 
            ObjectPool<Enemy> enemyPool, 
            EnemyManager enemyManager,
            HeroView heroView)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _enemyPool = enemyPool ?? throw new ArgumentNullException(nameof(enemyPool));
            _enemyManager = enemyManager ?? throw new ArgumentNullException(nameof(enemyManager));
            _heroTransform = heroView ? heroView.transform : throw new ArgumentNullException(nameof(heroView));

            _currentSpawnInterval = _config.BaseSpawnInterval;
        }

        public void Tick()
        {
            float deltaTime = Time.deltaTime;
            _gameTimeTimer += deltaTime;
            _spawnCooldownTimer += deltaTime;
            
            OnGameTimeUpdated?.Invoke(_gameTimeTimer);
            
            if (_spawnCooldownTimer >= _currentSpawnInterval)
            {
                _spawnCooldownTimer = 0f;
                SpawnEnemyWavePack();
            }
            
            int totalSecondsPassed = Mathf.FloorToInt(_gameTimeTimer);
            int calculatedCurrentWaveSeconds = totalSecondsPassed - ((_currentWave - 1) * _config.WaveDurationSeconds);

            if (calculatedCurrentWaveSeconds >= _config.WaveDurationSeconds)
            {
                AdvanceWave();
            }
        }

        private void SpawnEnemyWavePack()
        {
            if (!_heroTransform) return;
            
            int enemiesToSpawn = _config.BaseEnemiesPerWave + (_currentWave * 2);

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                Enemy enemy = _enemyPool.Get();
                
                Vector2 randomCircle = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(12f, 18f);
                Vector3 spawnPosition = _heroTransform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

                enemy.transform.position = spawnPosition;
                
                float waveHpModifier = 1f + (_currentWave - 1) * 0.25f;
                enemy.Spawn();
                
                _enemyManager.AddEnemy(enemy); 
            }
        }

        private void AdvanceWave()
        {
            _currentWave++;

            _currentSpawnInterval = _config.BaseSpawnInterval - (_currentWave * _config.DifficultyScalingFactor);
            
            OnWaveChanged?.Invoke(_currentWave);
            Debug.LogWarning($"[WaveSpawner] Entered Wave {_currentWave}. Spawn Interval reduced to: {_currentSpawnInterval:F2}s");
        }
    }
}