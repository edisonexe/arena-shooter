using System;
using ArenaShooter.Gameplay.Enemies;
using ArenaShooter.Infrastructure.Reset;
using Zenject;

namespace ArenaShooter.UI.HUD
{
    public class WaveHUDPresenter : IInitializable, IDisposable, IResettable
    {
        private readonly IWaveHUDView _view;
        private readonly EnemyWaveSpawner _waveSpawner;

        private const int ST_WAVE_NUM = 1;
        
        public WaveHUDPresenter(WaveHUDView view, EnemyWaveSpawner waveSpawner)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _waveSpawner = waveSpawner ?? throw new ArgumentNullException(nameof(waveSpawner));
        }

        public void Initialize()
        {
            _waveSpawner.OnWaveChanged += HandleWaveChanged;
            _view.UpdateWaveText(ST_WAVE_NUM);
        }

        private void HandleWaveChanged(int newWaveNumber) => _view.UpdateWaveText(newWaveNumber);
        

        public void Dispose() => _waveSpawner.OnWaveChanged -= HandleWaveChanged;

        public void ResetState() => _view.UpdateWaveText(ST_WAVE_NUM);
    }
}