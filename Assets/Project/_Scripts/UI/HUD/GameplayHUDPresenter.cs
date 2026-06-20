using System;
using ArenaShooter.Gameplay.Hero;
using ArenaShooter.Infrastructure.Reset;
using ArenaShooter.Services.Progression;
using UnityEngine;
using Zenject;

namespace ArenaShooter.UI.HUD
{
    public class GameplayHUDPresenter : IInitializable, IDisposable, IResettable
    {
        private readonly GameplayHUDView _view;
        private readonly HeroHealthSystem _healthSystem;
        private readonly HeroRuntimeStats _runtimeStats;
        private readonly LevelingService _levelingService;
        
        public GameplayHUDPresenter(
            GameplayHUDView view, 
            HeroHealthSystem healthSystem, 
            HeroRuntimeStats runtimeStats,
            LevelingService levelingService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _healthSystem = healthSystem ?? throw new ArgumentNullException(nameof(healthSystem));
            _runtimeStats = runtimeStats ?? throw new ArgumentNullException(nameof(runtimeStats));
            _levelingService = levelingService ?? throw new ArgumentNullException(nameof(levelingService));
        }

        public void Initialize()
        {
            _healthSystem.OnHealthChanged += OnPlayerHealthChanged;
            _levelingService.OnXpChanged += OnXpChanged;
            _levelingService.OnLevelChanged += OnLevelChanged;
            
            ResetState();
        }

        public void Dispose()
        {
            _healthSystem.OnHealthChanged -= OnPlayerHealthChanged;
            _levelingService.OnXpChanged -= OnXpChanged;
            _levelingService.OnLevelChanged -= OnLevelChanged;
        }

        public void ResetState()
        {
            OnPlayerHealthChanged(_runtimeStats.CurrentHealth, _runtimeStats.MaxHealth);
            _view.UpdateXpBar(_levelingService.CurrentXp, _levelingService.XpToNextLevel);
            _view.UpdateLevelText(_levelingService.CurrentLevel);
        }

        private void OnPlayerHealthChanged(float currentHealth, float maxHealth)
        {
            int currentHp = Mathf.RoundToInt(currentHealth);
            int maxHp = Mathf.RoundToInt(maxHealth);
            
            _view.UpdateHealthBar(currentHp, maxHp);
        }
        
        private void OnXpChanged(int currentXp, int targetXp)
        {
            _view.UpdateXpBar(currentXp, targetXp);
        }
        
        private void OnLevelChanged(int currentLevel)
        {
            _view.UpdateLevelText(currentLevel);
        }
    }
}