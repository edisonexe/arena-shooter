using System;
using System.Collections.Generic;
using ArenaShooter.Configs.Upgrades;
using UnityEngine;

namespace ArenaShooter.Services.Progression
{
    public class HeroStatsModifierService
    {
        private readonly Dictionary<UpgradeType, float> _modifiers = new (8);

        public HeroStatsModifierService()
        {
            foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
            {
                _modifiers[type] = 1.0f;
            }
        }

        public void ApplyUpgrade(UpgradeConfig upgrade)
        {
            _modifiers[upgrade.Type] += upgrade.Value;
            Debug.LogWarning($"[Stats] Applied {upgrade.Title}. New modifier for {upgrade.Type}: {_modifiers[upgrade.Type]}");
        }

        public float GetModifiedValue(UpgradeType type, float baseValue)
        {
            if (_modifiers.TryGetValue(type, out float modifier))
            {
                return baseValue * modifier;
            }
            return baseValue;
        }
    }
}