using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArenaShooter.UI
{
    public class GameplayHUDView : MonoBehaviour
    {
        [Header("HP Bar")]
        [SerializeField] private Image _hpFillImage;
        [SerializeField] private TextMeshProUGUI _hpText;
        
        [Header("XP Bar")]
        [SerializeField] private Image _xpFillImage;
        [SerializeField] private TextMeshProUGUI _xpText;
        
        [Header("Level Bar")]
        [SerializeField] private TextMeshProUGUI _levelText;
        
        private float _lastHpMax = 100f;
        private float _lastXpMax = 100f;
        
        private const string LVL_FORMAT = "LVL {0}";
        
        private void OnValidate()
        {
            if (!_hpFillImage) Debug.LogError("[GameplayHUDView] HpFillImage is not assigned!", this);
            if (!_hpText) Debug.LogError("[GameplayHUDView] HpText is not assigned!", this);
            if (!_xpFillImage) Debug.LogError("[GameplayHUDView] XPFillImage is not assigned!", this);
            if (!_xpText) Debug.LogError("[GameplayHUDView] XPText is not assigned!", this);
            if (!_levelText) Debug.LogError("[GameplayHUDView] LevelText is not assigned!", this);
        }

        public void UpdateHealthBar(float normalizedHealth)
        {
            _hpFillImage.fillAmount = normalizedHealth;

            int currentHp = Mathf.RoundToInt(normalizedHealth * _lastHpMax);
            _hpText.text = $"{currentHp} / {_lastHpMax}";
        }
        
        public void UpdateXpBar(float normalizedXp)
        {
            _xpFillImage.fillAmount = normalizedXp;
            
            int currentXp = Mathf.RoundToInt(normalizedXp * _lastXpMax);
            _xpText.text = $"{currentXp}/{_lastXpMax}%";
        }

        public void UpdateLevelText(int level)
        {
            _levelText.SetText(LVL_FORMAT, level);
        }
    }
}