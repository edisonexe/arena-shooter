using UnityEngine;
using UnityEngine.UI;

namespace ArenaShooter.UI.MainMenu
{
    public class MainMenuView : MonoBehaviour, IMainMenuView
    {
        [SerializeField] private GameObject _contentRoot;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _exitButton;
        
        public Button PlayButton => _playButton;
        public Button ExitButton => _exitButton;

        public void Show() => _contentRoot.SetActive(true);
        public void Hide() => _contentRoot.SetActive(false);

        private void OnValidate()
        {
            if (!_contentRoot) Debug.LogError("[MainMenuView] Content Root is not assigned!", this);
            if (!_playButton) Debug.LogError("[MainMenuView] Play Button missing!", this);
            if (!_exitButton) Debug.LogError("[MainMenuView] Exit Button missing!", this);
        }
    }
}