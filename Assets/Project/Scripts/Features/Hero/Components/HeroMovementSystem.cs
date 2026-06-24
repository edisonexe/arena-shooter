using System;
using ArenaShooter.Services.Input;
using Zenject;

namespace ArenaShooter.Features.Hero.Components
{
    public class HeroMovementSystem : IFixedTickable
    {
        private readonly IInputService _inputService;
        private readonly HeroMover _mover;

        public HeroMovementSystem(IInputService inputService, HeroMover mover)
        {
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            _mover = mover ?? throw new ArgumentNullException(nameof(mover));
        }

        public void FixedTick()
        {
            _mover.Move(_inputService.Axis, UnityEngine.Time.fixedDeltaTime);
        }
    }
}