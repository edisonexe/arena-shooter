using System;
using ArenaShooter.Infrastructure.Signals;
using Zenject;

namespace ArenaShooter.Services.Progression
{
    public class LevelingService : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;

        private int _currentLevel = 1;
        private int _currentXp = 0;
        private int _xpToNextLevel = 100;
        
        public event Action<int, float> OnXpChanged;

        public LevelingService(SignalBus signalBus)
        {
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
        }

        public void Initialize()
        {
            _signalBus.Subscribe<EnemyKilledSignal>(OnEnemyKilled);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<EnemyKilledSignal>(OnEnemyKilled);
        }

        private void OnEnemyKilled(EnemyKilledSignal signal)
        {
            AddXp(signal.XpValue);
        }

        private void AddXp(int amount)
        {
            _currentXp += amount;
            
            while (_currentXp >= _xpToNextLevel)
            {
                _currentXp -= _xpToNextLevel;
                LevelUp();
            }

            float progressNormalized = (float)_currentXp / _xpToNextLevel;
            OnXpChanged?.Invoke(_currentLevel, progressNormalized);
        }

        private void LevelUp()
        {
            _currentLevel++;
            
            _xpToNextLevel = UnityEngine.Mathf.RoundToInt(_xpToNextLevel * 1.2f);

            UnityEngine.Debug.LogWarning($"[Leveling] LEVEL UP! Current Level: {_currentLevel}. Next target: {_xpToNextLevel} XP");
        }
    }
}