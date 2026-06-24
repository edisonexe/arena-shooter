using UnityEngine.UI;

namespace ArenaShooter.UI.GameOver
{
    public interface IGameOverWindowView
    {
        public Button RestartButton { get; }
        public Button MenuButton { get; }
    
        void Show();
        void Hide();
    }
}