using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArenaShooter.UI.HUD
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
       
        private const string HP_FORMAT = "{0} / {1}";
        private const string XP_FORMAT = "{0} / {1}";
        private const string LVL_FORMAT = "LVL {0}";
        
        private void OnValidate()
        {
            if (!_hpFillImage) Debug.LogError("[GameplayHUDView] HpFillImage is not assigned!", this);
            if (!_hpText) Debug.LogError("[GameplayHUDView] HpText is not assigned!", this);
            if (!_xpFillImage) Debug.LogError("[GameplayHUDView] XPFillImage is not assigned!", this);
            if (!_xpText) Debug.LogError("[GameplayHUDView] XPText is not assigned!", this);
            if (!_levelText) Debug.LogError("[GameplayHUDView] LevelText is not assigned!", this);
        }

        public void UpdateHealthBar(int currentHp, int maxHp)
        {
            _hpFillImage.fillAmount = maxHp > 0 ? (float)currentHp / maxHp : 0f;

            _hpText.SetText(HP_FORMAT, currentHp, maxHp);
        }
        
        public void UpdateXpBar(int currentXp, int targetXp)
        {
            _xpFillImage.fillAmount = targetXp > 0 ? (float)currentXp / targetXp : 0f;
            
            _xpText.SetText(XP_FORMAT, currentXp, targetXp);
        }

        public void UpdateLevelText(int level)
        {
            _levelText.SetText(LVL_FORMAT, level);
        }
    }
}