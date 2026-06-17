using System;
using ArenaShooter.Gameplay.Hero;
using ArenaShooter.Services.Progression;
using Zenject;

namespace ArenaShooter.UI.HUD
{
    public class GameplayHUDPresenter : IInitializable, IDisposable
    {
        private readonly GameplayHUDView _view;
        private readonly HeroEntity _heroEntity;
        private readonly HeroRuntimeStats _runtimeStats;
        private readonly LevelingService _levelingService;
        
        public GameplayHUDPresenter(
            GameplayHUDView view, 
            HeroEntity heroEntity, 
            HeroRuntimeStats runtimeStats,
            LevelingService levelingService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _heroEntity = heroEntity ?? throw new ArgumentNullException(nameof(heroEntity));
            _runtimeStats = runtimeStats ?? throw new ArgumentNullException(nameof(runtimeStats));
            _levelingService = levelingService ?? throw new ArgumentNullException(nameof(levelingService));
        }

        public void Initialize()
        {
            _heroEntity.OnHealthChanged += OnPlayerHealthChanged;
            
            _levelingService.OnXpChanged += OnXpChanged;
            _levelingService.OnLevelChanged += OnLevelChanged;
            
            _view.UpdateHealthBar((int)_runtimeStats.CurrentHealth, (int)_runtimeStats.MaxHealth);
            _view.UpdateXpBar(_levelingService.CurrentXp, _levelingService.XpToNextLevel);
            _view.UpdateLevelText(_levelingService.CurrentLevel);
        }

        public void Dispose()
        {
            _heroEntity.OnHealthChanged -= OnPlayerHealthChanged;
            _levelingService.OnXpChanged -= OnXpChanged;
            _levelingService.OnLevelChanged -= OnLevelChanged;
        }

        private void OnPlayerHealthChanged(float normalizedHp)
        {
            int maxHp = (int)_runtimeStats.MaxHealth; 
            int currentHp = UnityEngine.Mathf.RoundToInt(normalizedHp * maxHp);
            
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