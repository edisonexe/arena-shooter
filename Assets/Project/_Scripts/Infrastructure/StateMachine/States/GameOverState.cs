using System;
using ArenaShooter.UI.GameOver;
using UnityEngine;

namespace ArenaShooter.Infrastructure.StateMachine.States
{
    public class GameOverState : IState
    {
        private readonly IGameOverWindowView _gameOverWindow;

        public GameOverState(IGameOverWindowView gameOverWindow)
        {
            _gameOverWindow = gameOverWindow ?? throw new ArgumentNullException(nameof(gameOverWindow));
        }

        public void Enter()
        {
            Time.timeScale = 0f;
            _gameOverWindow.Show();
        }

        public void Exit()
        {
            _gameOverWindow.Hide();
        }
    }
}