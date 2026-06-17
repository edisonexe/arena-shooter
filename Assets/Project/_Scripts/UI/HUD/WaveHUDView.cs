using TMPro;
using UnityEngine;

namespace ArenaShooter.UI.HUD
{
    public class WaveHUDView : MonoBehaviour, IWaveHUDView
    {
        [SerializeField] private TextMeshProUGUI _waveText;

        private const string WaveFormat = "WAVE: {0}";
        
        public void UpdateWaveText(int waveNumber)
        {
            _waveText.SetText(WaveFormat, waveNumber);
        }

        private void OnValidate()
        {
            if (!_waveText) Debug.LogError("[WaveHUDView] WaveText (TextMeshProUGUI) is not assigned!", this);
        }
    }
}