using System;
using ArenaShooter.Features.Weapons.Components;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Features.Hero.Components
{
    public class HeroCombatSystem : ITickable
    {
        private readonly AutoCombatWeapon _weapon;
        private readonly HeroView _view;
        private readonly HeroRotationSystem _rotationSystem;
        
        private readonly Action<Vector3> _onTargetLockedCache;

        public HeroCombatSystem(AutoCombatWeapon weapon, HeroView view, HeroRotationSystem rotationSystem)
        {
            _weapon = weapon ?? throw new ArgumentNullException(nameof(weapon));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _rotationSystem = rotationSystem ?? throw new ArgumentNullException(nameof(rotationSystem));
            
            _onTargetLockedCache = _rotationSystem.SetLookTarget;
        }

        public void Tick()
        {
            if (_view.VisualTilt) _view.VisualTilt.TickTilt(Time.deltaTime);
            
            _weapon.TickWeapon(_view.Rigidbody.position, _view.FirePoint, _onTargetLockedCache);
        }
    }
}