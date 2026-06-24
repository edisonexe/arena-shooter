using System;
using ArenaShooter.Infrastructure.Reset;
using ArenaShooter.Infrastructure.Signals;
using ArenaShooter.Services.Gameplay;
using Zenject;

namespace ArenaShooter.UI.GameOver
{
    public class GameOverPresenter : IInitializable, IDisposable, IResettable
    {
        private readonly IGameOverWindowView _view;
        private readonly SignalBus _signalBus;
        private readonly GameNavigationService _navigationService;

        private Action<PlayerDiedSignal> _onPlayerDiedCache;
        public event Action OnRestartRequested;

        public GameOverPresenter(
            IGameOverWindowView view, 
            SignalBus signalBus, 
            GameNavigationService navigationService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public void Initialize()
        {
            _view.RestartButton.onClick.AddListener(HandleRestart);
            _view.MenuButton.onClick.AddListener(HandleMenu);
            
            _onPlayerDiedCache = OnPlayerDied;
            _signalBus.Subscribe(_onPlayerDiedCache);
            
            _view.Hide();
        }

        public void Dispose()
        {
            if (_view != null)
            {
                _view.RestartButton.onClick.RemoveListener(HandleRestart);
                _view.MenuButton.onClick.RemoveListener(HandleMenu);
            }
            _signalBus.Unsubscribe(_onPlayerDiedCache);
        }

        public void ResetState() => _view.Hide();

        private void OnPlayerDied(PlayerDiedSignal signal) => _view.Show();
        private void HandleRestart() => OnRestartRequested?.Invoke();
        private void HandleMenu() => _navigationService.ToMainMenu();
    }
}