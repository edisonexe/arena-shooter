using System;
using ArenaShooter.Gameplay.Enemies;
using UnityEngine;
using Zenject;

namespace ArenaShooter.UI.HUD
{
    public class TimerPresenter : IInitializable, IDisposable
    {
        private readonly TimerView _view;
        private readonly EnemyWaveSpawner _waveSpawner;
        
        private int _lastUpdatedSecond = -1;
        
        public TimerPresenter(TimerView view, EnemyWaveSpawner waveSpawner)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _waveSpawner = waveSpawner ?? throw new ArgumentNullException(nameof(waveSpawner));
        }

        public void Initialize()
        {
            _waveSpawner.OnGameTimeUpdated += HandleGameTimeUpdated;
            _view.SetTime(0, 0);
        }

        private void HandleGameTimeUpdated(float totalSeconds)
        {
            int totalIntSeconds = Mathf.FloorToInt(totalSeconds);

            if (totalIntSeconds != _lastUpdatedSecond)
            {
                _lastUpdatedSecond = totalIntSeconds;
                
                int minutes = totalIntSeconds / 60;
                int seconds = totalIntSeconds % 60;

                _view.SetTime(minutes, seconds);
            }
        }

        public void Dispose()
        {
            _waveSpawner.OnGameTimeUpdated -= HandleGameTimeUpdated;
        }
    }
}