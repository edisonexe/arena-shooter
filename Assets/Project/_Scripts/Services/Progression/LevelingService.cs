using System;
using ArenaShooter.Infrastructure.Signals;
using Zenject;

namespace ArenaShooter.Services.Progression
{
    public class LevelingService
    {
        private int _currentLevel = 1;
        private int _currentXp = 0;
        private int _xpToNextLevel = 100;
        
        public event Action<int, float> OnXpChanged;
        public event Action OnLevelUp;

        public void AddXp(int amount)
        {
            if (amount <= 0) return;
            
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
            
            OnLevelUp?.Invoke();
        }
    }
}