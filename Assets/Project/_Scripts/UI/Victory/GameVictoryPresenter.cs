using System;
using ArenaShooter.Services.Gameplay;
using UnityEngine;
using Zenject;

namespace ArenaShooter.UI.Victory
{
    public class GameVictoryPresenter : IInitializable, IDisposable
    {
        private readonly IGameVictoryWindowView _view;
        private readonly MatchDurationSystem _durationSystem;
        private readonly GameNavigationService _navigationService;

        public GameVictoryPresenter(
            IGameVictoryWindowView view, 
            MatchDurationSystem durationSystem, 
            GameNavigationService navigationService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _durationSystem = durationSystem ?? throw new ArgumentNullException(nameof(durationSystem));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public void Initialize()
        {
            _view.RestartButton.onClick.AddListener(HandleRestart);
            _view.MenuButton.onClick.AddListener(HandleMenu);
            _durationSystem.OnTimeout += HandleTimeout;
            
            _view.Hide();
        }

        public void Dispose()
        {
            if (_view != null)
            {
                _view.RestartButton.onClick.RemoveListener(HandleRestart);
                _view.MenuButton.onClick.RemoveListener(HandleMenu);
            }
            _durationSystem.OnTimeout -= HandleTimeout;
        }

        private void HandleTimeout() => _view.Show();
        private void HandleRestart() => _navigationService.ToGameplay();
        private void HandleMenu() => _navigationService.ToMainMenu();
    }
}