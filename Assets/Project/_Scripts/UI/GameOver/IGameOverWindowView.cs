using System;

namespace ArenaShooter.UI.GameOver
{
    public interface IGameOverWindowView
    {
        event Action OnRestartRequested;
        void Show();
        void Hide();
    }
}