using System;
using ArenaShooter.Configs.Enemies;
using ArenaShooter.Gameplay.Hero;
using ArenaShooter.Infrastructure.Pooling;
using ArenaShooter.Infrastructure.Signals;
using ArenaShooter.Services.Gameplay;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Gameplay.Enemies
{
    public class EnemyWaveSpawner : ITickable, IInitializable, IDisposable
    {
        private readonly WaveConfig _config;
        private readonly ObjectPool<Enemy> _enemyPool;
        private readonly EnemyManager _enemyManager;
        private readonly Transform _heroTransform;
        private readonly SignalBus _signalBus;
        private readonly ArenaBoundsService _arenaBoundsService;
        private readonly MatchDurationSystem _durationSystem;
        
        private int _currentWave = 1;
        
        private float _spawnCooldownTimer;
        private float _currentSpawnInterval;
        private bool _isSpawningActive = true;

        private const float SPAWN_RAD = 20f;
        
        private Action<PlayerDiedSignal> _onPlayerDiedCache;
        
        public event Action<int> OnWaveChanged;

        public EnemyWaveSpawner(
            WaveConfig config, 
            ObjectPool<Enemy> enemyPool, 
            EnemyManager enemyManager,
            HeroView heroView,
            SignalBus signalBus,
            ArenaBoundsService arenaBoundsService,
            MatchDurationSystem durationSystem)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _enemyPool = enemyPool ?? throw new ArgumentNullException(nameof(enemyPool));
            _enemyManager = enemyManager ?? throw new ArgumentNullException(nameof(enemyManager));
            _heroTransform = heroView ? heroView.transform : throw new ArgumentNullException(nameof(heroView));
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
            _durationSystem = durationSystem ?? throw new ArgumentNullException(nameof(durationSystem));
            _arenaBoundsService = arenaBoundsService ?? throw new ArgumentNullException(nameof(arenaBoundsService));
            
            _currentSpawnInterval = _config.BaseSpawnInterval;
        }

        public void Initialize()
        {
            _onPlayerDiedCache = HandlePlayerDied;
            _signalBus.Subscribe(_onPlayerDiedCache);
        }
        
        public void Tick()
        {
            if (!_isSpawningActive) return;
            
            float deltaTime = Time.deltaTime;
            _spawnCooldownTimer += deltaTime;
            
            if (_spawnCooldownTimer >= _currentSpawnInterval)
            {
                _spawnCooldownTimer = 0f;
                SpawnEnemyWavePack();
            }
            
            float currentMatchTime = _durationSystem.ElapsedTime;
            int totalSecondsPassed = Mathf.FloorToInt(currentMatchTime);
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
                
                Vector2 randomOffset = UnityEngine.Random.insideUnitCircle.normalized * SPAWN_RAD;
                Vector3 spawnPosition = _heroTransform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);

                spawnPosition = _arenaBoundsService.ClampToArena(spawnPosition);
                
                enemy.Initialize(spawnPosition);

                enemy.Spawn();
                
                _enemyManager.AddEnemy(enemy); 
            }
        }

        private void AdvanceWave()
        {
            _currentWave++;

            _currentSpawnInterval = _config.BaseSpawnInterval - (_currentWave * _config.DifficultyScalingFactor);
            
            OnWaveChanged?.Invoke(_currentWave);
        }
        
        private void HandlePlayerDied(PlayerDiedSignal signal)
        {
            _isSpawningActive = false;
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe(_onPlayerDiedCache);
        }
    }
}