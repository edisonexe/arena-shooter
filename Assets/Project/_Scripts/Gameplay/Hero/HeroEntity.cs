using System;
using ArenaShooter.Gameplay.Combat;
using ArenaShooter.Gameplay.Weapons;
using ArenaShooter.Infrastructure.Signals;
using ArenaShooter.Services.Input;
using ArenaShooter.Services.Progression;
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
        private readonly SignalBus _signalBus;
        private readonly HeroRuntimeStats _runtimeStats;
        private readonly HeroStatsModifierService _modifierService;

        private const float ROT_SPEED = 15f;
        private Vector3 _lookTargetPosition;
        private bool _hasLookTarget;
        
        private readonly Action<Vector3> _onTargetLockedCache;
        private Action<DamageTakenSignal> _onDamageSignalCache;
        public event Action<float> OnHealthChanged;

        public HeroEntity(
            IInputService inputService, 
            HeroMover mover, 
            HeroView view, 
            AutoCombatWeapon weapon, 
            SignalBus signalBus,
            HeroRuntimeStats runtimeStats,
            HeroStatsModifierService modifierService)
        {
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            _mover = mover ?? throw new ArgumentNullException(nameof(mover));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _weapon = weapon ?? throw new ArgumentNullException(nameof(weapon));
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
            _runtimeStats = runtimeStats ?? throw new ArgumentNullException(nameof(runtimeStats));
            _modifierService = modifierService ?? throw new ArgumentNullException(nameof(modifierService));

            _onTargetLockedCache = RotateVisualTowardsTarget;
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
            _mover.Move(_inputService.Axis, Time.fixedDeltaTime);
        }

        public void Tick()
        {
            _hasLookTarget = false;

            _weapon.TickWeapon(_view.transform.position, _view.FirePoint, _onTargetLockedCache);
            
            UpdateVisualRotation(Time.deltaTime);
        }
        public void TakeDamage(float amount)
        {
            if (_runtimeStats.CurrentHealth <= 0f) return;

            _modifierService.ApplyDamage(amount);

            if (_view.VisualTilt)
            {
                _view.VisualTilt.ApplyStrictBackwardTilt(_view.transform.forward);
            }
            
            float normalizedHealth = _runtimeStats.MaxHealth > 0f ? _runtimeStats.CurrentHealth / _runtimeStats.MaxHealth : 0f;
            OnHealthChanged?.Invoke(normalizedHealth);

            if (_runtimeStats.CurrentHealth <= 0f)
            {
                Die();
            }
        }

        private void RotateVisualTowardsTarget(Vector3 targetPosition)
        {
            _lookTargetPosition = targetPosition;
            _hasLookTarget = true;
        }

        private void UpdateVisualRotation(float deltaTime)
        {
            if (!_hasLookTarget || !_view.VisualRoot) return;

            Vector3 direction = _lookTargetPosition - _view.transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                _view.VisualRoot.rotation = Quaternion.Slerp(
                    _view.VisualRoot.rotation, 
                    targetRotation, 
                    ROT_SPEED * deltaTime
                );
            }
        }

        private void Die()
        {
            _signalBus.Fire(new PlayerDiedSignal());
            _view.gameObject.SetActive(false);
        }
        
        private void OnDamageSignalReceived(DamageTakenSignal signal)
        {
            TakeDamage(signal.Amount);
        }
    }
}