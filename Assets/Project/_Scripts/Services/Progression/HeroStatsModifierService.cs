using System;
using System.Collections.Generic;
using ArenaShooter.Configs;
using ArenaShooter.Configs.Upgrades;
using ArenaShooter.Gameplay.Hero;
using ArenaShooter.Services.Progression.StatsCalculation;
using UnityEngine;

namespace ArenaShooter.Services.Progression
{
    public class HeroStatsModifierService
    {
        private readonly HeroConfig _heroConfig;
        private readonly WeaponConfig _weaponConfig;
        private readonly HeroRuntimeStats _runtimeStats;

        private readonly Dictionary<UpgradeType, float> _modifiers = new (8);
        private readonly Dictionary<UpgradeType, IStatCalculation> _upgrades = new (8);
        
        public event Action OnModifiersApplied;

        public HeroStatsModifierService(
            HeroConfig heroConfig, 
            WeaponConfig weaponConfig, 
            HeroRuntimeStats runtimeStats,
            List<IStatCalculation> upgrades)
        {
            _heroConfig = heroConfig ?? throw new ArgumentNullException(nameof(heroConfig));
            _weaponConfig = weaponConfig ?? throw new ArgumentNullException(nameof(weaponConfig));
            _runtimeStats = runtimeStats ?? throw new ArgumentNullException(nameof(runtimeStats));
            if (upgrades == null) throw new ArgumentNullException(nameof(upgrades));

            for (var i = 0; i < upgrades.Count; i++)
            {
                _upgrades[upgrades[i].TargetUpgradeType] = upgrades[i];
            }
            
            ResetModifiersToDefault();
        }

        public void ApplyUpgrade(UpgradeConfig upgrade)
        {
            _modifiers[upgrade.Type] += upgrade.Value;
            RecalculateAndApply();
        }

        public void ApplyDamage(float damageAmount)
        {
            float newHealth = Mathf.Max(_runtimeStats.CurrentHealth - damageAmount, 0f);
            _runtimeStats.SetCurrentHealth(newHealth);
        }
        
        public void ResetModifiersToDefault()
        {
            foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType)))
            {
                _modifiers[type] = 1.0f;
            }

            _runtimeStats.ResetToConfigs();
            RecalculateAndApply();
        }
        
        private void RecalculateAndApply()
        {
            var context = new StatCalculationContext(_heroConfig, _weaponConfig, _runtimeStats);

            foreach (var kvp in _modifiers)
            {
                if (_upgrades.TryGetValue(kvp.Key, out var statCalculation))
                {
                    statCalculation.CalculateAndApply(in context, kvp.Value);
                }
            }
            
            
            OnModifiersApplied?.Invoke();
        }
    }
}