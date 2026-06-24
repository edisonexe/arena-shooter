using System;
using System.Collections.Generic;
using ArenaShooter.Features.Progression.Configs;

namespace ArenaShooter.UI.Upgrades
{
    public interface IUpgradeWindowView
    {
        void Initialize(Action<UpgradeConfig> onUpgradeSelected);
        void Show(List<UpgradeConfig> availableUpgrades);
        void Hide();
    }
}