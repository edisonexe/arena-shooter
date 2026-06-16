using System;
using System.Collections.Generic;
using ArenaShooter.Configs.Upgrades;
using UnityEngine;

namespace ArenaShooter.UI.Upgrades
{
    public class UpgradeWindowView : MonoBehaviour
    {
        [SerializeField] private UpgradeCardView[] _cards;

        private void OnValidate()
        {
            if (_cards == null || _cards.Length == 0) Debug.LogError("[UpgradeWindowView] No cards have been set.", this);
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
            gameObject.SetActive(true);

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

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}