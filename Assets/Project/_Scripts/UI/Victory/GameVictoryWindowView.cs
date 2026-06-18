using System;
using UnityEngine;
using UnityEngine.UI;

namespace ArenaShooter.UI.Victory
{
    public class GameVictoryWindowView : MonoBehaviour, IGameVictoryWindowView
    {
        [SerializeField] private GameObject _contentRoot;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _menuButton;

        public Button RestartButton => _restartButton;
        public Button MenuButton => _menuButton;

        public void Show() => _contentRoot.SetActive(true);
        public void Hide() => _contentRoot.SetActive(false);

        private void OnValidate()
        {
            if (!_contentRoot) Debug.LogError("[GameVictoryWindowView] Content Root is not assigned!", this);
            if (!_restartButton) Debug.LogError("[GameVictoryWindowView] Restart Button missing!", this);
            if (!_menuButton) Debug.LogError("[GameVictoryWindowView] Menu Button missing!", this);
        }
    }
}