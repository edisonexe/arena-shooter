using ArenaShooter.Features.Hero.Components;

namespace ArenaShooter.Features.Progression.Upgrades
{
    public class DamageCalculation : IUpgradeCalculation
    {
        public UpgradeType TargetUpgradeType => UpgradeType.DamageBoost;

        public void Apply(HeroRuntimeStats stats, float addFactor) 
            => stats.SetBulletDamage(stats.BulletDamage * (1f + addFactor));
    }
}