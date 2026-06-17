using System;
using ArenaShooter.Infrastructure.Signals;
using UnityEngine.SceneManagement;
using Zenject;

namespace ArenaShooter.UI.GameOver
{
    public class GameOverPresenter : IInitializable, IDisposable
    {
        private readonly GameOverWindowView _view;
        private readonly SignalBus _signalBus;

        public GameOverPresenter(GameOverWindowView view, SignalBus signalBus)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
        }

        public void Initialize()
        {
            _view.OnRestartClick.AddListener(HandleRestartRequest);
            _view.Hide();
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        private void OnPlayerDied(PlayerDiedSignal signal)
        {
            _view.Show();
        }

        private void HandleRestartRequest()
        {
            _view.Hide();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
            _view.OnRestartClick.RemoveListener(HandleRestartRequest);
        }
    }
}