using System;
using ArenaShooter.Features.Progression.Services;
using ArenaShooter.Infrastructure.Reset;
using UnityEngine;

namespace ArenaShooter.Features.Hero.Components
{
    public class HeroEntity : IResettable
    {
        private readonly HeroHealthSystem _healthSystem;
        private readonly HeroView _view;
        private readonly HeroStatsModifierService _modifierService;
        private readonly Transform _spawnPoint;

        public HeroEntity(
            HeroHealthSystem healthSystem, 
            HeroView view, 
            HeroStatsModifierService modifierService,
            Transform spawnPoint)
        {
            _healthSystem = healthSystem ?? throw new ArgumentNullException(nameof(healthSystem));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _modifierService = modifierService ?? throw new ArgumentNullException(nameof(modifierService));
            _spawnPoint = spawnPoint ?? throw new ArgumentNullException(nameof(spawnPoint));
        }
        
        public void ResetState()
        {
            _view.gameObject.SetActive(true);

            _view.Rigidbody.linearVelocity = Vector3.zero;
            _view.Rigidbody.angularVelocity = Vector3.zero;
            _view.Rigidbody.position = _spawnPoint.position;
            
            if (_view.VisualRoot)
            {
                _view.VisualRoot.rotation = _spawnPoint.rotation;
            }
            
            _modifierService.ResetModifiersToDefault();
            _healthSystem.ResetHealth();
        }
    }
}