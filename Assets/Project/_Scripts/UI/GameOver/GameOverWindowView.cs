using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ArenaShooter.UI.GameOver
{
    public class GameOverWindowView : MonoBehaviour
    {
        [SerializeField] private GameObject _contentRoot;
        [SerializeField] private Button _restartButton;

        public Button.ButtonClickedEvent OnRestartClick => _restartButton.onClick;
        
        private void OnValidate()
        {
            if (!_contentRoot) Debug.LogError("[GameOverWindowView] Content Root is not assigned!", this);
            if (!_restartButton) Debug.LogError("[GameOverWindowView] Restart Button is not assigned!", this);
        }

        public void Show()
        {
            _contentRoot.SetActive(true);
            Time.timeScale = 0f;
        }

        public void Hide()
        {
            _contentRoot.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}