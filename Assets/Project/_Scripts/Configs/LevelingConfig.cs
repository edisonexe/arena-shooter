using UnityEngine;

namespace ArenaShooter.Configs
{
    [CreateAssetMenu(fileName = "LevelingConfig", menuName = "Configs/Progression/LevelingConfig")]
    public class LevelingConfig : ScriptableObject
    {
        [SerializeField, Min(10)] private int _baseXpToNextLevel = 100;
        [SerializeField, Min(1.05f)] private float _xpMultiplier = 1.2f;

        public int BaseXpToNextLevel => _baseXpToNextLevel;
        public float XpMultiplier => _xpMultiplier;
    }
}