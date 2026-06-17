using System;
using ArenaShooter.Configs;

namespace ArenaShooter.Gameplay.Hero
{
    public class HeroRuntimeStats
    {
        public float MoveSpeed { get; private set; }
        public float BulletDamage { get; private set; }
        public float WeaponCooldown { get; private set; }
        
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }

        public HeroRuntimeStats(HeroConfig heroConfig, WeaponConfig weaponConfig)
        {
            if (heroConfig == null) throw new ArgumentNullException(nameof(heroConfig));
            if (weaponConfig == null) throw new ArgumentNullException(nameof(weaponConfig));
            
            MoveSpeed = heroConfig.MoveSpeed;
            MaxHealth = heroConfig.MaxHealth;
            CurrentHealth = MaxHealth;

            BulletDamage = weaponConfig.Damage;
            WeaponCooldown = weaponConfig.FireCooldown;
        }
        
        public void SetCurrentHealth(float health) => CurrentHealth = health;
        public void SetMaxHealth(float health) => MaxHealth = health;
        public void SetMoveSpeed(float moveSpeed) => MoveSpeed = moveSpeed;
        public void SetBulletDamage(float damage) => BulletDamage = damage;
        public void SetWeaponCooldown(float cooldown) => WeaponCooldown = cooldown;
    }
}