using UnityEngine.UI;

namespace ArenaShooter.UI.MainMenu
{
    public interface IMainMenuView
    {
        Button PlayButton { get; }
        Button ExitButton { get; }
        
        void Show();
        void Hide();
    }
}