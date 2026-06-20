using ArenaShooter.Configs.Upgrades;
using ArenaShooter.Gameplay.Hero;

namespace ArenaShooter.Services.Progression.UpgradesCalculation
{
    public class FireRateCalculation : IUpgradeCalculation
    {
        public UpgradeType TargetUpgradeType => UpgradeType.FireRateBoost;

        public void Apply(HeroRuntimeStats stats, float addFactor) 
            => stats.SetWeaponCooldown(stats.WeaponCooldown / (1f + addFactor));
    }
}