using TMPro;
using UnityEngine;

namespace ArenaShooter.UI.HUD
{
    public class TimerView : MonoBehaviour, ITimerView
    {
        [SerializeField] private TextMeshProUGUI _timerText;
        
        private const string TIME_FORMAT = "{0:00}:{1:00}";
        
        public void SetTime(int minutes, int seconds)
        {
            _timerText.SetText(TIME_FORMAT, minutes, seconds);
        }

        private void OnValidate()
        {
            if (!_timerText) Debug.LogError("[TimerView] TimerText (TextMeshProUGUI) is not assigned!", this);
        }
    }
}