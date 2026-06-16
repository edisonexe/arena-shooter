using System;
using System.Collections.Generic;
using ArenaShooter.Configs.Upgrades;
using ArenaShooter.Services.Progression;
using Zenject;

namespace ArenaShooter.UI.Upgrades
{
    public class UpgradeWindowPresenter : IInitializable, IDisposable
    {
        private readonly UpgradeWindowView _view;
        private readonly LevelingService _levelingService;
        private readonly HeroStatsModifierService _modifierService;
        private readonly UpgradeDatabase _database;
        
        private const int CardsToShowCount = 3;
        
        private readonly List<UpgradeConfig> _chosenUpgradesBuffer = new (8);
        private readonly List<int> _randomIndexPool = new (32);
        
        private readonly Action<UpgradeConfig> _onUpgradeSelectedCache;

        public UpgradeWindowPresenter(
            UpgradeWindowView view, 
            LevelingService levelingService, 
            HeroStatsModifierService modifierService,
            UpgradeDatabase database)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _levelingService = levelingService ?? throw new ArgumentNullException(nameof(levelingService));
            _modifierService = modifierService ?? throw new ArgumentNullException(nameof(modifierService));
            _database = database ?? throw new ArgumentNullException(nameof(database));

            _onUpgradeSelectedCache = OnUpgradeSelected;
        }

        public void Initialize()
        {
            _view.Initialize(_onUpgradeSelectedCache);
            _view.Hide();

            _levelingService.OnLevelUp += OnLevelUpTriggered;
        }

        public void Dispose()
        {
            _levelingService.OnLevelUp -= OnLevelUpTriggered;
        }

        private void OnLevelUpTriggered()
        {
            UnityEngine.Time.timeScale = 0f;

            UpgradeConfig[] allUpgrades = _database.AllUpgrades;
            int totalUpgradesCount = allUpgrades.Length;

            _chosenUpgradesBuffer.Clear();
            
            if (totalUpgradesCount <= CardsToShowCount)
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
                
                for (int i = 0; i < CardsToShowCount; i++)
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
            UnityEngine.Time.timeScale = 1f;
        }
    }
}