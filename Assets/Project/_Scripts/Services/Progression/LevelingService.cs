using System;
using ArenaShooter.Configs;

namespace ArenaShooter.Services.Progression
{
    public class LevelingService
    {
        private readonly LevelingConfig _config;
        
        private int _currentLevel = 1;
        private int _currentXp;
        private int _xpToNextLevel;
        
        public event Action<int, int> OnXpChanged; // currentXp, xpToNextLevel
        public event Action<int> OnLevelChanged; // new lvl
        public event Action OnLevelUp; // ui action

        public int CurrentLevel => _currentLevel;
        public int CurrentXp => _currentXp;
        public int XpToNextLevel => _xpToNextLevel;
        public float ProgressNormalized => _xpToNextLevel > 0 ? (float)_currentXp / _xpToNextLevel : 0f;
        
        public LevelingService(LevelingConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
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
                UnityEngine.Debug.LogWarning($"[Leveling] LEVEL UP! Current Level: {_currentLevel}. Next target: {_xpToNextLevel} XP");
                OnLevelChanged?.Invoke(_currentLevel);
                OnLevelUp?.Invoke();
            }
            
            OnXpChanged?.Invoke(_currentXp, _xpToNextLevel);
        }
    }
}