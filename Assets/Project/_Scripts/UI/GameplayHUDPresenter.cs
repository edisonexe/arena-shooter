using System;
using ArenaShooter.Gameplay.Hero;
using ArenaShooter.Services.Progression;
using Zenject;

namespace ArenaShooter.UI
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
            
            _view.UpdateHealthBar(1f);
            _view.UpdateXpBar(0f);
            _view.UpdateLevelText(1);
        }

        public void Dispose()
        {
            _heroEntity.OnHealthChanged -= OnPlayerHealthChanged;
            _levelingService.OnXpChanged -= OnXpChanged;
        }

        private void OnPlayerHealthChanged(float normalizedHp)
        {
            _view.UpdateHealthBar(normalizedHp);
        }
        
        private void OnXpChanged(int currentLevel, float normalizedXp)
        {
            _view.UpdateXpBar(normalizedXp);
            _view.UpdateLevelText(currentLevel);
        }
    }
}