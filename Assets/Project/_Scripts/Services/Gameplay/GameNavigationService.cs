using System;
using ArenaShooter.Infrastructure.SceneLoading;
using ArenaShooter.Infrastructure.StateMachine;
using ArenaShooter.Infrastructure.StateMachine.States;

namespace ArenaShooter.Services.Gameplay
{
    public class GameNavigationService
    {
        private readonly SceneLoaderService _sceneLoader;
        private readonly GameStateMachine _stateMachine;
        private readonly MatchDurationSystem _matchDurationSystem;
        
        private const string MENU_SCENE = "Menu";
        private const string GAME_SCENE = "Game";

        public event Action OnEnteredMenu;
        public event Action OnEnteredGameplay;
        
        public GameNavigationService(SceneLoaderService sceneLoader, GameStateMachine stateMachine)
        {
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
            _stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
        }

        public void ToGameplay()
        {
            _sceneLoader.LoadScene(GAME_SCENE, () =>
            {
                _stateMachine.TransitionTo<GameplayState>();
                OnEnteredGameplay?.Invoke();
            });
        }

        public void ToMainMenu()
        {
            _sceneLoader.LoadScene(MENU_SCENE, () =>
            {
                _stateMachine.TransitionTo<GameMenuState>();
                OnEnteredMenu?.Invoke();
            });
        }
    }
}