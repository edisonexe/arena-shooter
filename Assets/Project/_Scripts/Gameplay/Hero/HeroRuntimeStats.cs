using System;

namespace ArenaShooter.Gameplay.Hero
{
    public class HeroRuntimeStats
    {
        public float MoveSpeed { get; private set; }
        public float BulletDamage { get; private set; }
        public float WeaponCooldown { get; private set; }

        public void UpdateStats(float moveSpeed, float bulletDamage, float weaponCooldown)
        {
            MoveSpeed = moveSpeed;
            BulletDamage = bulletDamage;
            WeaponCooldown = weaponCooldown;
        }
    }
}