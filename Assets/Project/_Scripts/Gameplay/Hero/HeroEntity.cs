using System;
using ArenaShooter.Gameplay.Weapons;
using ArenaShooter.Services.Input;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Gameplay.Hero
{
    public class HeroEntity : ITickable, IFixedTickable
    {
        private readonly IInputService _inputService;
        private readonly HeroMover _mover;
        private readonly HeroView _view;
        private readonly AutoCombatWeapon _weapon;

        private readonly Action<Vector3> _onTargetLockedCache;
        
        public HeroEntity(IInputService inputService, HeroMover mover, HeroView view, AutoCombatWeapon weapon)
        {
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService), "[HeroEntity] InputService cannot be null!");
            _mover = mover ?? throw new ArgumentNullException(nameof(mover), "[HeroEntity] Mover cannot be null!");
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _weapon = weapon ?? throw new ArgumentNullException(nameof(weapon));

            _onTargetLockedCache = RotateHeroTowardsTarget;
        }

        public void FixedTick()
        {
            _mover.Move(_inputService.Axis, UnityEngine.Time.fixedDeltaTime);
        }
        
        public void Tick()
        {
            _weapon.TickWeapon(_view.Rigidbody.position, _view.FirePoint, _onTargetLockedCache);
        }

        private void RotateHeroTowardsTarget(UnityEngine.Vector3 targetPosition)
        {
            _mover.RotateTowards(targetPosition);
        }
    }
}