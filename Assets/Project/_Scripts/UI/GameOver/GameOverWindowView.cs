using System;
using UnityEngine;
using UnityEngine.UI;

namespace ArenaShooter.UI.GameOver
{
    public class GameOverWindowView : MonoBehaviour, IGameOverWindowView
    {
        [SerializeField] private GameObject _contentRoot;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _menuButton;

        public Button RestartButton => _restartButton;
        public Button MenuButton => _menuButton;
        
        private void OnValidate()
        {
            if (!_contentRoot) Debug.LogError("[GameOverWindowView] Content Root is not assigned!", this);
            if (!_restartButton) Debug.LogError("[GameOverWindowView] Restart Button is not assigned!", this);
            if (!_menuButton) Debug.LogError("[GameOverWindowView] Menu Button is not assigned!", this);
        }

        public void Show() => _contentRoot.SetActive(true);
        public void Hide() => _contentRoot.SetActive(false);
    }
}