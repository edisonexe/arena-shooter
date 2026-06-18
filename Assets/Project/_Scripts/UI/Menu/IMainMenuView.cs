using System;
using UnityEngine.UI;

namespace ArenaShooter.UI.Menu
{
    public interface IMainMenuView
    {
        Button PlayButton { get; }
        Button ExitButton { get; }
        
        void Show();
        void Hide();
    }
}