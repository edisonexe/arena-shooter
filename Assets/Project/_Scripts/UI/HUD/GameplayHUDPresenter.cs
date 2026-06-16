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
        private readonly LevelingService _levelingService;
        
        public GameplayHUDPresenter(GameplayHUDView view, HeroEntity heroEntity, LevelingService levelingService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _heroEntity = heroEntity ?? throw new ArgumentNullException(nameof(heroEntity));
            _levelingService = levelingService ?? throw new ArgumentNullException(nameof(levelingService));
        }

        public void Initialize()
        {
            _heroEntity.OnHealthChanged += OnPlayerHealthChanged;
            
            _levelingService.OnXpChanged += OnXpChanged;
            
            _view.UpdateHealthBar(100, 100);
            _view.UpdateXpBar(0, 100);
            _view.UpdateLevelText(1);
        }

        public void Dispose()
        {
            _heroEntity.OnHealthChanged -= OnPlayerHealthChanged;
            _levelingService.OnXpChanged -= OnXpChanged;
        }

        private void OnPlayerHealthChanged(float normalizedHp)
        {
            int maxHp = 100; 
            int currentHp = UnityEngine.Mathf.RoundToInt(normalizedHp * maxHp);
            
            _view.UpdateHealthBar(currentHp, maxHp);
        }
        
        private void OnXpChanged(int currentLevel, float normalizedXp)
        {
            int targetXp = 100; 
            int currentXp = UnityEngine.Mathf.RoundToInt(normalizedXp * targetXp);

            _view.UpdateXpBar(currentXp, targetXp);
            _view.UpdateLevelText(currentLevel);
        }
    }
}