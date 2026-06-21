using System;
using System.Collections.Generic;
using ArenaShooter.Configs.Upgrades;
using ArenaShooter.Gameplay.Hero;
using ArenaShooter.Services.UpgradesCalculation;

namespace ArenaShooter.Services.Progression
{
    public class HeroStatsModifierService
    {
        private readonly HeroRuntimeStats _runtimeStats;
        
        private readonly Dictionary<UpgradeType, IUpgradeCalculation> _upgrades = new (8);
        
        public HeroStatsModifierService(
            HeroRuntimeStats runtimeStats,
            List<IUpgradeCalculation> calculations)
        {
            _runtimeStats = runtimeStats ?? throw new ArgumentNullException(nameof(runtimeStats));
            if (calculations == null) throw new ArgumentNullException(nameof(calculations));
            
            for (int i = 0; i < calculations.Count; i++)
            {
                _upgrades[calculations[i].TargetUpgradeType] = calculations[i];
            }
            
            ResetModifiersToDefault();
        }
        
        public void ApplyUpgrade(UpgradeConfig upgrade)
        {
            if (upgrade == null) return;
            
            if (_upgrades.TryGetValue(upgrade.Type, out var strategy))
            {
                strategy.Apply(_runtimeStats, upgrade.AddFactor);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(upgrade.Type));
            }
        }
        
        public void ResetModifiersToDefault() => _runtimeStats.ResetToConfigs();
    }
}