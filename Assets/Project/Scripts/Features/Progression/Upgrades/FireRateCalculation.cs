using ArenaShooter.Features.Hero.Components;

namespace ArenaShooter.Features.Progression.Upgrades
{
    public class FireRateCalculation : IUpgradeCalculation
    {
        public UpgradeType TargetUpgradeType => UpgradeType.FireRateBoost;

        public void Apply(HeroRuntimeStats stats, float addFactor) 
            => stats.SetWeaponCooldown(stats.WeaponCooldown / (1f + addFactor));
    }
}