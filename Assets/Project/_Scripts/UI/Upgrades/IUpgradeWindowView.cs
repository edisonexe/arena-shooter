using System;
using System.Collections.Generic;
using ArenaShooter.Configs.Upgrades;

namespace ArenaShooter.UI.Upgrades
{
    public interface IUpgradeWindowView
    {
        void Initialize(Action<UpgradeConfig> onUpgradeSelected);
        void Show(List<UpgradeConfig> availableUpgrades);
        void Hide();
    }
}