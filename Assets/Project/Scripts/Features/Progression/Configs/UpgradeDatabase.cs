using UnityEngine;

namespace ArenaShooter.Features.Progression.Configs
{
    [CreateAssetMenu(fileName = "UpgradeDatabase", menuName = "Configs/Upgrades/UpgradeDatabase")]
    public class UpgradeDatabase : ScriptableObject
    {
        [SerializeField] private UpgradeConfig[] _allUpgrades;

        public UpgradeConfig[] AllUpgrades => _allUpgrades;

        private void OnValidate()
        {
            if (_allUpgrades == null || _allUpgrades.Length == 0)
            {
                Debug.LogError("[UpgradeDatabase] Database is empty! Assign upgrades.", this);
            }
        }
    }
}