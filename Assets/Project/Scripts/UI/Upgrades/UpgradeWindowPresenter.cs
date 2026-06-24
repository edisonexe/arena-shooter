using System;
using System.Collections.Generic;
using ArenaShooter.Features.Hero.Components;
using ArenaShooter.Features.Progression.Configs;
using ArenaShooter.Features.Progression.Services;
using ArenaShooter.Infrastructure.Signals;
using Zenject;

namespace ArenaShooter.UI.Upgrades
{
    public class UpgradeWindowPresenter : IInitializable, IDisposable
    {
        private readonly IUpgradeWindowView _view;
        private readonly HeroRuntimeStats _runtimeStats;
        private readonly HeroStatsModifierService _modifierService;
        private readonly UpgradeDatabase _database;
        private readonly SignalBus _signalBus;
        
        private const int CARDS_TO_SHOW = 3;
        
        private readonly List<UpgradeConfig> _chosenUpgradesBuffer = new (8);
        private readonly List<int> _randomIndexPool = new (32);
        
        private readonly Action<UpgradeConfig> _onUpgradeSelectedCache;
        private readonly Action<ShowUpgradeWindowSignal> _onShowUIRequestCache;

        public UpgradeWindowPresenter(
            IUpgradeWindowView view,
            HeroStatsModifierService modifierService,
            UpgradeDatabase database,
            HeroRuntimeStats runtimeStats,
            SignalBus signalBus)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _modifierService = modifierService ?? throw new ArgumentNullException(nameof(modifierService));
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _runtimeStats = runtimeStats ?? throw new ArgumentNullException(nameof(runtimeStats));
            _signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
            
            _onUpgradeSelectedCache = OnUpgradeSelected;
            _onShowUIRequestCache = OnShowUIRequestReceived;
        }

        public void Initialize()
        {
            _view.Initialize(_onUpgradeSelectedCache);
            _view.Hide();
            
            _signalBus.Subscribe(_onShowUIRequestCache);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe(_onShowUIRequestCache);
        }

        private void OnShowUIRequestReceived(ShowUpgradeWindowSignal signal)
        {
            if (_runtimeStats.CurrentHealth <= 0f) return;
            
            UpgradeConfig[] allUpgrades = _database.AllUpgrades;
            int totalUpgradesCount = allUpgrades.Length;

            _chosenUpgradesBuffer.Clear();
            
            if (totalUpgradesCount <= CARDS_TO_SHOW)
            {
                for (var i = 0; i < totalUpgradesCount; i++)
                {
                    _chosenUpgradesBuffer.Add(allUpgrades[i]);
                }
            }
            else
            {
                _randomIndexPool.Clear();
                for (int i = 0; i < totalUpgradesCount; i++)
                {
                    _randomIndexPool.Add(i);
                }
                
                for (int i = 0; i < CARDS_TO_SHOW; i++)
                {
                    int poolIndex = UnityEngine.Random.Range(0, _randomIndexPool.Count);
                    int upgradeIndex = _randomIndexPool[poolIndex];
                    
                    _chosenUpgradesBuffer.Add(allUpgrades[upgradeIndex]);
                    _randomIndexPool.RemoveAt(poolIndex);
                }
            }
            
            _view.Show(_chosenUpgradesBuffer);
        }

        private void OnUpgradeSelected(UpgradeConfig selectedUpgrade)
        {
            _modifierService.ApplyUpgrade(selectedUpgrade);
            _view.Hide();
            
            _signalBus.Fire(new RequestGameplayStateSignal());
        }
    }
}