using ArenaShooter.Configs.Upgrades;
using ArenaShooter.Gameplay.Hero;

namespace ArenaShooter.Services.Progression.UpgradesCalculation
{
    public class DamageCalculation : IUpgradeCalculation
    {
        public UpgradeType TargetUpgradeType => UpgradeType.DamageBoost;

        public void Apply(HeroRuntimeStats stats, float addFactor) 
            => stats.SetBulletDamage(stats.BulletDamage * (1f + addFactor));
    }
}