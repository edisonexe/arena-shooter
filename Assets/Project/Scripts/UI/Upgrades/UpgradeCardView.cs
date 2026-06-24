using System;
using ArenaShooter.Features.Progression.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArenaShooter.UI.Upgrades
{
    public class UpgradeCardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Button _selectButton;

        private UpgradeConfig _currentConfig;
        private Action<UpgradeConfig> _onClickedCallback;

        public void Initialize(Action<UpgradeConfig> onClickedCallback)
        {
            _onClickedCallback = onClickedCallback ?? throw new ArgumentNullException(nameof(onClickedCallback));
            _selectButton.onClick.AddListener(HandleClick);
        }

        public void Setup(UpgradeConfig config)
        {
            _currentConfig = config;
            _titleText.text = config.Title;
            _descriptionText.text = config.Description;
            
            if (config.Icon) _iconImage.sprite = config.Icon;
        }

        private void HandleClick()
        {
            if (_currentConfig)
            {
                _onClickedCallback?.Invoke(_currentConfig);
            }
        }

        private void OnDestroy()
        {
            _selectButton.onClick.RemoveListener(HandleClick);
        }

        private void OnValidate()
        {
            if (!_titleText) Debug.LogError("[UpgradeCardView] Title Text is not assigned", this);
            if (!_descriptionText) Debug.LogError("[UpgradeCardView] Description Text is not assigned", this);
            if (!_iconImage) Debug.LogError("[UpgradeCardView] Icon Image is not assigned", this);
            if (!_selectButton) Debug.LogError("[UpgradeCardView] Selection Button is not assigned", this);
        }
    }
}


