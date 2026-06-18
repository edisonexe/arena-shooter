using UnityEngine.UI;

namespace ArenaShooter.UI.Victory
{
    public interface IGameVictoryWindowView
    {
        public Button RestartButton { get; }
        public Button MenuButton { get; }
        
        void Show();
        void Hide();
    }
}