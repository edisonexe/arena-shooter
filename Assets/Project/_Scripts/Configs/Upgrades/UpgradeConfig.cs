using UnityEngine;

namespace ArenaShooter.Configs.Upgrades
{
    [CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Configs/Upgrades/UpgradeConfig")]
    public class UpgradeConfig : ScriptableObject
    {
        [SerializeField] private string _title;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private UpgradeType _type;
        [SerializeField] private float _addPercent;

        public string Title => _title;
        public string Description => _description;
        public Sprite Icon => _icon;
        public UpgradeType Type => _type;
        public float AddFactor => _addPercent / 100f;
    }
}