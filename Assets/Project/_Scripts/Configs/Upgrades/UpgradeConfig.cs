using UnityEngine;

namespace ArenaShooter.Configs.Upgrades
{
    [CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Configs/Upgrades/UpgradeConfig")]
    public class UpgradeConfig : ScriptableObject
    {
        [SerializeField] private string _title;
        [SerializeField] private string _description;
        [SerializeField] private UpgradeType _type;
        [SerializeField] private float _value;

        public string Title => _title;
        public string Description => _description;
        public UpgradeType Type => _type;
        public float Value => _value;
    }
}