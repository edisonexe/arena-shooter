using System;
using UnityEngine.SceneManagement;
using Zenject;

namespace ArenaShooter.UI.GameOver
{
    public class GameOverPresenter : IInitializable, IDisposable
    {
        private readonly IGameOverWindowView _view;

        public GameOverPresenter(GameOverWindowView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void Initialize()
        {
            _view.OnRestartRequested += HandleRestart;
            _view.Hide();
        }

        public void Dispose()
        {
            _view.OnRestartRequested -= HandleRestart;
        }

        private void HandleRestart()
        {
            _view.Hide();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}