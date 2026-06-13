using System;
using ArenaShooter.Gameplay.Hero;
using Zenject;

namespace ArenaShooter.UI
{
    public class GameplayHUDPresenter : IInitializable, IDisposable
    {
        private readonly GameplayHUDView _view;
        private readonly HeroEntity _heroEntity;

        public GameplayHUDPresenter(GameplayHUDView view, HeroEntity heroEntity)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _heroEntity = heroEntity ?? throw new ArgumentNullException(nameof(heroEntity));
        }

        public void Initialize()
        {
            _heroEntity.OnHealthChanged += OnPlayerHealthChanged;
            
            _view.UpdateHealthBar(1f);
        }

        public void Dispose()
        {
            _heroEntity.OnHealthChanged -= OnPlayerHealthChanged;
        }

        private void OnPlayerHealthChanged(float normalizedHealth)
        {
            _view.UpdateHealthBar(normalizedHealth);
        }
    }
}