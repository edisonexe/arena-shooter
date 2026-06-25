using System;
using ArenaShooter.Services.Gameplay;
using Zenject;

namespace ArenaShooter.UI.MainMenu
{
    public class MainMenuPresenter : IInitializable, IDisposable
    {
        private readonly IMainMenuView _view;
        private readonly GameNavigationService _navigationService;

        public MainMenuPresenter(IMainMenuView view, GameNavigationService navigationService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public void Initialize()
        {
            _view.PlayButton.onClick.AddListener(HandlePlay);
            
#if UNITY_WEBGL
            _view.ExitButton.gameObject.SetActive(false);
#else
            _view.ExitButton.onClick.AddListener(HandleExit);
#endif
            
            _view.Show();
        }

        public void Dispose()
        {
            if (_view != null)
            {
                _view.PlayButton.onClick.RemoveListener(HandlePlay);
#if !UNITY_WEBGL
                _view.ExitButton.onClick.RemoveListener(HandleExit);
#endif
            }
        }

        private void HandlePlay() => _navigationService.ToGameplay();

        private void HandleExit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}