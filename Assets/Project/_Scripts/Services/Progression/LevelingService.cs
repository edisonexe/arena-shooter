using System;
using ArenaShooter.Configs;
using ArenaShooter.Infrastructure.Signals;

namespace ArenaShooter.Services.Progression
{
    public class LevelingService
    {
        private readonly LevelingConfig _config;
        private readonly SignalBus _signalBus;
        
        private int _currentLevel = 1;
        private int _currentXp;
        private int _xpToNextLevel;
        
        public event Action<int, int> OnXpChanged; // currentXp, xpToNextLevel
        public event Action<int> OnLevelChanged; // new lvl

        public int CurrentLevel => _currentLevel;
        public int CurrentXp => _currentXp;
        public int XpToNextLevel => _xpToNextLevel;
        
        public LevelingService(LevelingConfig config, SignalBus signalBus)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
            _xpToNextLevel = _config.BaseXpToNextLevel;
        }
        
        public void AddXp(int amount)
        {
            if (amount <= 0) return;

            _currentXp += amount;
            bool levelUpOccurred = false;

            while (_currentXp >= _xpToNextLevel)
            {
                _currentXp -= _xpToNextLevel;
                _currentLevel++;
                _xpToNextLevel = UnityEngine.Mathf.RoundToInt(_xpToNextLevel * _config.XpMultiplier);
                levelUpOccurred = true;
            }

            if (levelUpOccurred)
            {
                OnLevelChanged?.Invoke(_currentLevel);
                
                _signalBus.Fire(new GameStatesSignals.LevelUpSignal());
            }
            
            OnXpChanged?.Invoke(_currentXp, _xpToNextLevel);
        }
    }
}