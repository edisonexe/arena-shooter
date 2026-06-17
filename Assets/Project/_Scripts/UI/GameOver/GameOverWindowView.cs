using UnityEngine;
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
        }

        public void Hide()
        {
            _contentRoot.SetActive(false);
        }
    }
}