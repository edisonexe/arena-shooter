using System;
using System.Collections.Generic;
using ArenaShooter.Gameplay.Hero;
using ArenaShooter.Infrastructure.Pooling;
using ArenaShooter.Infrastructure.Reset;
using ArenaShooter.Infrastructure.Signals;
using ArenaShooter.Services.Progression;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Gameplay.Items
{
    public class XPGemManager : IInitializable, ITickable, IDisposable, IResettable
    {
        private readonly ObjectPool<XPGem> _gemPool;
        private readonly HeroView _heroView;
        private readonly LevelingService _levelingService;
        private readonly SignalBus _signalBus;
        private readonly HeroRuntimeStats _runtimeStats;

        private readonly List<XPGem> _activeGems = new(256);
        private Action<EnemyKilledSignal> _onEnemyKilledCache;
        
        private const float COL_RAD_SQR = 0.6f * 0.6f;
        private const float MAGNET_SP = 10f;

        public XPGemManager(
            ObjectPool<XPGem> gemPool, 
            HeroView heroView, 
            LevelingService levelingService, 
            SignalBus signalBus,
            HeroRuntimeStats runtimeStats)
        {
            _gemPool = gemPool ?? throw new ArgumentNullException(nameof(gemPool));
            _heroView = heroView ?? throw new ArgumentNullException(nameof(heroView));
            _levelingService = levelingService ?? throw new ArgumentNullException(nameof(levelingService));
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
            _runtimeStats = runtimeStats ?? throw new ArgumentNullException(nameof(runtimeStats));
            
            _onEnemyKilledCache = HandleEnemyKilled;
        }

        public void Initialize()
        {
            _signalBus.Subscribe(_onEnemyKilledCache);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe(_onEnemyKilledCache);
        }
        
        public void Tick()
        {
            if (_activeGems.Count == 0) return;

            Vector3 heroPosition = _heroView.transform.position;
            float deltaTime = Time.deltaTime;

            float currentMagnetRadius = _runtimeStats.PickupRadius;
            float dynamicMagnetRadiusSqr = currentMagnetRadius * currentMagnetRadius;
            
            for (var i = _activeGems.Count - 1; i >= 0; i--)
            {
                XPGem gem = _activeGems[i];

                if (!gem.IsActive)
                {
                    _activeGems.RemoveAt(i);
                    _gemPool.Return(gem);
                    continue;
                }

                Vector3 gemPosition = gem.transform.position;
                
                heroPosition.y = gemPosition.y;
                
                float sqrDistance = (heroPosition - gemPosition).sqrMagnitude;

                if (sqrDistance <= COL_RAD_SQR)
                {
                    _levelingService.AddXp(gem.XpValue); 
                    
                    _activeGems.RemoveAt(i);
                    _gemPool.Return(gem);
                    continue;
                }

                if (sqrDistance <= dynamicMagnetRadiusSqr)
                {
                    gem.transform.position = Vector3.MoveTowards(gemPosition, heroPosition, MAGNET_SP * deltaTime);
                }
            }
        }

        public void ResetState()
        {
            for (int i = _activeGems.Count - 1; i >= 0; i--)
            {
                _gemPool.Return(_activeGems[i]);
            }
            _activeGems.Clear();
        }

        private void HandleEnemyKilled(EnemyKilledSignal signal)
        {
            XPGem gem = _gemPool.Get();
            gem.Initialize(signal.DeathPosition, signal.XpValue); 
            _activeGems.Add(gem);
        }
    }
}