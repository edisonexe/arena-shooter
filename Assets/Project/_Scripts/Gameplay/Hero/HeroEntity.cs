using System;
using ArenaShooter.Configs;
using ArenaShooter.Gameplay.Combat;
using ArenaShooter.Gameplay.Weapons;
using ArenaShooter.Infrastructure.Signals;
using ArenaShooter.Services.Input;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Gameplay.Hero
{
    public class HeroEntity : ITickable, IFixedTickable, IDamageable, IInitializable, IDisposable
    {
        private readonly IInputService _inputService;
        private readonly HeroMover _mover;
        private readonly HeroView _view;
        private readonly AutoCombatWeapon _weapon;
        private readonly HeroConfig _config;
        private readonly SignalBus _signalBus;
        
        private float _currentHealth;

        private readonly Action<Vector3> _onTargetLockedCache;
        private Action<DamageTakenSignal> _onDamageSignalCache;
        public event Action<float> OnHealthChanged;

        public HeroEntity(
            IInputService inputService, 
            HeroMover mover, 
            HeroView view, 
            AutoCombatWeapon weapon, 
            HeroConfig config,
            SignalBus signalBus)
        {
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            _mover = mover ?? throw new ArgumentNullException(nameof(mover));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _weapon = weapon ?? throw new ArgumentNullException(nameof(weapon));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));

            _onTargetLockedCache = RotateHeroTowardsTarget;
            _currentHealth = _config.MaxHealth;
        }

        public void Initialize()
        {
            _onDamageSignalCache = OnDamageSignalReceived;
            _signalBus.Subscribe(_onDamageSignalCache);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe(_onDamageSignalCache);
        }
        
        public void FixedTick()
        {
            _mover.Move(_inputService.Axis, UnityEngine.Time.fixedDeltaTime);
        }

        public void Tick()
        {
            _weapon.TickWeapon(_view.Rigidbody.position, _view.FirePoint, _onTargetLockedCache);
        }

        public void TakeDamage(float amount)
        {
            if (_currentHealth <= 0f) return;

            _currentHealth -= amount;

            _currentHealth = Mathf.Max(_currentHealth, 0f);

            OnHealthChanged?.Invoke(_currentHealth / _config.MaxHealth);

            if (_currentHealth <= 0f)
            {
                Die();
            }
        }

        private void RotateHeroTowardsTarget(UnityEngine.Vector3 targetPosition)
        {
            _mover.RotateTowards(targetPosition);
        }

        private void Die()
        {
            Debug.LogError("[HeroEntity] Game Over! Hero is dead.");
            _view.gameObject.SetActive(false);
        }
        
        private void OnDamageSignalReceived(DamageTakenSignal signal)
        {
            TakeDamage(signal.Amount);
        }
    }
}