using UnityEngine;

namespace ArenaShooter.Configs.Enemies
{
    [CreateAssetMenu(fileName = "WaveConfig", menuName = "Configs/Enemies/WaveConfig")]
    public class WaveConfig : ScriptableObject
    {
        [Header("Spawn Intervals")]
        [SerializeField, Min(1f)] private float _baseSpawnInterval = 2.0f;
        [SerializeField, Min(10f)] private float _difficultyScalingFactor = 0.05f;

        [Header("Wave Limits")]
        [SerializeField] private int _baseEnemiesPerWave = 5;
        [SerializeField] private int _waveDurationSeconds = 60;

        public float BaseSpawnInterval => _baseSpawnInterval;
        public float DifficultyScalingFactor => _difficultyScalingFactor;
        public int BaseEnemiesPerWave => _baseEnemiesPerWave;
        public int WaveDurationSeconds => _waveDurationSeconds;
    }
}