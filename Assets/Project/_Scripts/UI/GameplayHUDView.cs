using UnityEngine;
using UnityEngine.UI;

namespace ArenaShooter.UI
{
    public class GameplayHUDView : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;

        private void OnValidate()
        {
            if (!_healthSlider) Debug.LogError("[GameplayHUDView] HealthSlider is not assigned!", this);
        }

        public void UpdateHealthBar(float normalizedHealth)
        {
            _healthSlider.value = normalizedHealth;
        }
    }
}