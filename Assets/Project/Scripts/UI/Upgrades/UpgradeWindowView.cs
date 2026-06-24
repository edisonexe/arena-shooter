using System;
using System.Collections.Generic;
using ArenaShooter.Features.Progression.Configs;
using UnityEngine;

namespace ArenaShooter.UI.Upgrades
{
    public class UpgradeWindowView : MonoBehaviour, IUpgradeWindowView
    {
        [SerializeField] private UpgradeCardView[] _cards;
        [SerializeField] private GameObject _contentRoot;
        
        private void OnValidate()
        {
            if (_cards == null || _cards.Length == 0) 
                Debug.LogError("[UpgradeWindowView] No cards have been set.", this);
            
            if (!_contentRoot) Debug.LogError("[UpgradeWindowView] Content Root is not assigned!", this);
        }

        public void Initialize(Action<UpgradeConfig> onUpgradeSelected)
        {
            for (var i = 0; i < _cards.Length; i++)
            {
                _cards[i].Initialize(onUpgradeSelected);
            }
        }

        public void Show(List<UpgradeConfig> availableUpgrades)
        {
            _contentRoot.SetActive(true);

            int upgradesCount = availableUpgrades.Count;

            for (int i = 0; i < _cards.Length; i++)
            {
                if (i < upgradesCount)
                {
                    _cards[i].Setup(availableUpgrades[i]);
                    _cards[i].gameObject.SetActive(true);
                }
                else
                {
                    _cards[i].gameObject.SetActive(false);
                }
            }
        }

        public void Hide() => _contentRoot.SetActive(false);
    }
}