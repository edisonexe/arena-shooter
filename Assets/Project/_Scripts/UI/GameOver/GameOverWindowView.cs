using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ArenaShooter.UI.GameOver
{
    public class GameOverWindowView : MonoBehaviour, IGameOverWindowView, IInitializable
    {
        [SerializeField] private GameObject _contentRoot;
        [SerializeField] private Button _restartButton;

        public event Action OnRestartRequested;
        
        public void Initialize()
        {
            _restartButton.onClick.AddListener(HandleRestartClick);
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(HandleRestartClick);
        }
        
        private void OnValidate()
        {
            if (!_contentRoot) Debug.LogError("[GameOverWindowView] Content Root is not assigned!", this);
            if (!_restartButton) Debug.LogError("[GameOverWindowView] Restart Button is not assigned!", this);
        }

        public void Show() => _contentRoot.SetActive(true);
        public void Hide() => _contentRoot.SetActive(false);

        private void HandleRestartClick() => OnRestartRequested?.Invoke();
    }
}