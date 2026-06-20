using System;
using ArenaShooter.Configs.Enemies;
using ArenaShooter.Gameplay.Hero;
using ArenaShooter.Infrastructure.Reset;
using ArenaShooter.Infrastructure.Signals;
using ArenaShooter.Services.Gameplay;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Gameplay.Enemies
{
    public class EnemyWaveSpawner : ITickable, IInitializable, IDisposable, IResettable
    {
        private readonly WaveConfig _config;
        private readonly EnemyFactory _enemyFactory;
        private readonly EnemyManager _enemyManager;
        private readonly Transform _heroTransform;
        private readonly SignalBus _signalBus;
        private readonly ArenaBoundsService _arenaBoundsService;
        private readonly MatchDurationSystem _durationSystem;
        
        private int _currentWaveIndex = 0;
        private float _spawnCooldownTimer;
        private bool _isSpawningActive = true;

        private const float SPAWN_RAD = 20f;
        private Action<PlayerDiedSignal> _onPlayerDiedCache;
        
        public event Action<int> OnWaveChanged;

        public EnemyWaveSpawner(
            WaveConfig config, 
            EnemyFactory enemyFactory, 
            EnemyManager enemyManager,
            HeroView heroView,
            SignalBus signalBus,
            ArenaBoundsService arenaBoundsService,
            MatchDurationSystem durationSystem)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _enemyFactory = enemyFactory ?? throw new ArgumentNullException(nameof(enemyFactory));
            _enemyManager = enemyManager ?? throw new ArgumentNullException(nameof(enemyManager));
            _heroTransform = heroView ? heroView.transform : throw new ArgumentNullException(nameof(heroView));
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
            _durationSystem = durationSystem ?? throw new ArgumentNullException(nameof(durationSystem));
            _arenaBoundsService = arenaBoundsService ?? throw new ArgumentNullException(nameof(arenaBoundsService));
        }

        public void Initialize()
        {
            _onPlayerDiedCache = HandlePlayerDied;
            _signalBus.Subscribe(_onPlayerDiedCache);
            
            OnWaveChanged?.Invoke(_currentWaveIndex + 1);
        }
        
        public void Tick()
        {
            if (!_isSpawningActive || _config.Waves.Count == 0) return;
            if (_currentWaveIndex >= _config.Waves.Count) return;

            WaveSetup currentWaveSetup = _config.Waves[_currentWaveIndex];
            float deltaTime = Time.deltaTime;
            
            _spawnCooldownTimer += deltaTime;
            if (_spawnCooldownTimer >= currentWaveSetup.SpawnInterval)
            {
                _spawnCooldownTimer = 0f;
                SpawnCurrentWavePack(currentWaveSetup);
            }
            
            float currentMatchTime = _durationSystem.ElapsedTime;
            int totalSecondsPassed = Mathf.FloorToInt(currentMatchTime);
            
            int accumulatedTimeBeforeCurrent = 0;
            for (int i = 0; i < _currentWaveIndex; i++)
            {
                accumulatedTimeBeforeCurrent += _config.Waves[i].WaveDurationSeconds;
            }

            int secondsInCurrentWave = totalSecondsPassed - accumulatedTimeBeforeCurrent;

            if (secondsInCurrentWave >= currentWaveSetup.WaveDurationSeconds)
            {
                AdvanceWave();
            }
        }

        public void ResetState()
        {
            _currentWaveIndex = 0;
            _spawnCooldownTimer = 0f;
            _isSpawningActive = true;
            OnWaveChanged?.Invoke(_currentWaveIndex + 1);
        }
        
        private void SpawnCurrentWavePack(in WaveSetup currentWaveSetup)
        {
            if (!_heroTransform) return;
            
            for (int e = 0; e < currentWaveSetup.EnemiesToSpawn.Count; e++)
            {
                EnemySpawnSetup spawnSetup = currentWaveSetup.EnemiesToSpawn[e];
                
                for (int i = 0; i < spawnSetup.Count; i++)
                {
                    Vector2 randomOffset = UnityEngine.Random.insideUnitCircle.normalized * SPAWN_RAD;
                    Vector3 spawnPosition = _heroTransform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);
                    spawnPosition.y = _heroTransform.position.y;
                    
                    spawnPosition = _arenaBoundsService.ClampToArena(spawnPosition);
                    
                    EnemyEntity enemy = _enemyFactory.Create(spawnPosition, spawnSetup.EnemyConfig);
                    _enemyManager.AddEnemy(enemy); 
                }
            }
        }

        private void AdvanceWave()
        {
            if (_currentWaveIndex + 1 >= _config.Waves.Count) return;

            _currentWaveIndex++;
            _spawnCooldownTimer = 0f;
            
            OnWaveChanged?.Invoke(_currentWaveIndex + 1);
        }
        
        private void HandlePlayerDied(PlayerDiedSignal signal) => _isSpawningActive = false;
        
        public void Dispose() => _signalBus.Unsubscribe(_onPlayerDiedCache);
    }
}