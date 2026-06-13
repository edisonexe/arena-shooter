using System;
using ArenaShooter.Services.Input;
using Zenject;

namespace ArenaShooter.Gameplay.Hero
{
    public class HeroEntity : IFixedTickable
    {
        private readonly IInputService _inputService;
        private readonly HeroMover _mover;

        public HeroEntity(IInputService inputService, HeroMover mover)
        {
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService), "[HeroEntity] InputService cannot be null!");
            _mover = mover ?? throw new ArgumentNullException(nameof(mover), "[HeroEntity] Mover cannot be null!");
        }

        public void FixedTick()
        {
            _mover.Move(_inputService.Axis, UnityEngine.Time.fixedDeltaTime);
        }
    }
}