using System;
using System.Collections.Generic;
using ArenaShooter.Infrastructure.StateMachine;
using ArenaShooter.Infrastructure.StateMachine.States;
using ArenaShooter.UI.GameOver;
using ArenaShooter.UI.Victory;
using Zenject;

namespace ArenaShooter.Infrastructure.Reset
{
    public class GameResetSystem : IInitializable, IDisposable
    {
        private readonly List<IResettable> _resettables;
        private readonly GameStateMachine _stateMachine;
        private readonly GameOverPresenter _gameOverPresenter;
        private readonly GameVictoryPresenter _gameVictoryPresenter;

        public GameResetSystem(
            List<IResettable> resettables,
            GameStateMachine stateMachine,
            GameOverPresenter gameOverPresenter,
            GameVictoryPresenter gameVictoryPresenter)
        {
            _resettables = resettables ?? throw new ArgumentNullException(nameof(resettables));
            _stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
            _gameOverPresenter = gameOverPresenter ?? throw new ArgumentNullException(nameof(gameOverPresenter));
            _gameVictoryPresenter = gameVictoryPresenter ?? throw new ArgumentNullException(nameof(gameVictoryPresenter));
        }

        public void Initialize()
        {
            _gameOverPresenter.OnRestartRequested += ResetGame;
            _gameVictoryPresenter.OnRestartRequested += ResetGame;
        }
        
        public void Dispose()
        {
            _gameOverPresenter.OnRestartRequested -= ResetGame;
            _gameVictoryPresenter.OnRestartRequested -= ResetGame;
        }

        public void ResetGame()
        {
            ResetAllSystemsOnly();
            _stateMachine.TransitionTo<GameplayState>();
        }

        public void ResetAllSystemsOnly()
        {
            for (int i = 0; i < _resettables.Count; i++)
            {
                _resettables[i].ResetState();
            }
        }
    }
}