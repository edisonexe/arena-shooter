using System;
using System.Collections.Generic;
using ArenaShooter.Configs;
using ArenaShooter.Configs.Upgrades;
using ArenaShooter.Gameplay.Hero;
using UnityEngine;

namespace ArenaShooter.Services.Progression
{
    public class HeroStatsModifierService
    {
        private readonly HeroConfig _heroConfig;
        private readonly WeaponConfig _weaponConfig;
        private readonly HeroRuntimeStats _runtimeStats;

        private readonly Dictionary<UpgradeType, float> _modifiers = new (8);

        public HeroStatsModifierService(HeroConfig heroConfig, WeaponConfig weaponConfig, HeroRuntimeStats runtimeStats)
        {
            _heroConfig = heroConfig ?? throw new ArgumentNullException(nameof(heroConfig));
            _weaponConfig = weaponConfig ?? throw new ArgumentNullException(nameof(weaponConfig));
            _runtimeStats = runtimeStats ?? throw new ArgumentNullException(nameof(runtimeStats));
            
            ResetModifiersToDefault();
        }

        public void ApplyUpgrade(UpgradeConfig upgrade)
        {
            _modifiers[upgrade.Type] += upgrade.Value;
            RecalculateAndApply();
            
            Debug.LogWarning($"[Stats] Applied {upgrade.Title}. New modifier for {upgrade.Type}: {_modifiers[upgrade.Type]}");
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
        }
        
        private void RecalculateAndApply()
        {
            float finalSpeed = _heroConfig.MoveSpeed * _modifiers[UpgradeType.MoveSpeedBoost];
            float finalDamage = _weaponConfig.Damage * _modifiers[UpgradeType.DamageBoost];
            float finalCooldown = _weaponConfig.FireCooldown / _modifiers[UpgradeType.FireRateBoost];
            
            _runtimeStats.SetMoveSpeed(finalSpeed);
            _runtimeStats.SetBulletDamage(finalDamage);
            _runtimeStats.SetWeaponCooldown(finalCooldown);
        }
    }
}