using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArenaShooter.Features.Enemies.Configs
{
    [Serializable]
    public struct EnemySpawnSetup
    {
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private int _count;

        public EnemyConfig EnemyConfig => _enemyConfig;
        public int Count => _count;
    }

    [Serializable]
    public struct WaveSetup
    {
        [SerializeField] private int _waveDurationSeconds;
        [SerializeField] private float _spawnInterval;
        [SerializeField] private List<EnemySpawnSetup> _enemiesToSpawn;

        public int WaveDurationSeconds => _waveDurationSeconds;
        public float SpawnInterval => _spawnInterval;
        public List<EnemySpawnSetup> EnemiesToSpawn => _enemiesToSpawn;
    }

    [CreateAssetMenu(fileName = "WaveConfig", menuName = "Configs/Enemies/WaveConfig")]
    public class WaveConfig : ScriptableObject
    {
        [SerializeField] private List<WaveSetup> _waves;

        public List<WaveSetup> Waves => _waves;
    }
}