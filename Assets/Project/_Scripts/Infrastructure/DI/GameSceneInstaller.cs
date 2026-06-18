using ArenaShooter.Configs;
using ArenaShooter.Configs.Enemies;
using ArenaShooter.Configs.Upgrades;
using ArenaShooter.Gameplay.Enemies;
using ArenaShooter.Gameplay.Hero;
using ArenaShooter.Gameplay.Items;
using ArenaShooter.Gameplay.Weapons;
using ArenaShooter.Gameplay.Camera;
using ArenaShooter.Infrastructure.Signals;
using ArenaShooter.Infrastructure.StateMachine;
using ArenaShooter.Services.Combat;
using ArenaShooter.Services.Gameplay;
using ArenaShooter.Services.Input;
using ArenaShooter.Services.Progression;
using ArenaShooter.UI.GameOver;
using ArenaShooter.UI.HUD;
using ArenaShooter.UI.Upgrades;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace ArenaShooter.Infrastructure.DI
{
    public class GameSceneInstaller : MonoInstaller
    {
        [Header("Configs")] 
        [SerializeField] private HeroConfig _heroConfig;
        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private WaveConfig _waveConfig;
        [SerializeField] private LevelingConfig _levelingConfig;
        [SerializeField] private MatchConfig _matchConfig;
        
        [Header("Prefabs & Spawn Points")] 
        [SerializeField] private HeroView _heroViewPrefab;
        [SerializeField] private Transform _heroSpawnPoint;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _bulletsParent;
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Transform _enemiesParent;
        [SerializeField] private XPGem _xpGemPrefab;
        [SerializeField] private Transform _xpGemsParent;
        
        [Header("Pool Capacities")] 
        [SerializeField, Min(0)] private int _initialBulletCapacity = 128;
        [SerializeField, Min(0)] private int _initialEnemyCapacity = 1288;
        [SerializeField, Min(0)] private int _initialGemCapacity = 128;
        
        [Header("UI Views")]
        [SerializeField] private GameplayHUDView _gameplayHUDView;
        [SerializeField] private UpgradeWindowView _upgradeWindowView;
        [SerializeField] private TimerView _timerView;
        [SerializeField] private WaveHUDView _waveHUDView;
        [SerializeField] private GameOverWindowView _gameOverWindowView;
        
        [Header("Render Settings")]
        [SerializeField] private Volume _globalVolume;
        
        [Header("Progression Databases")]
        [SerializeField] private UpgradeDatabase _upgradeDatabase;

        [Header("Gameplay Services on Scene")]
        [SerializeField] private ArenaBoundsService _arenaBoundsService;
        
        public override void InstallBindings()
        {
            ValidateInInspector();

            InstallInfrastructure();
            InstallCoreServices();
            InstallPools();
            InstallGameplayManagers();
            InstallHero();
            InstallHUDModules();
            InstallProgressionUI();
            InstallGameOverUI();
        }

        
        private void InstallInfrastructure()
        {
            Container.Bind<SignalBus>().AsSingle();

            Container.BindInterfacesAndSelfTo<NewInputService>().AsSingle().NonLazy();
            Container.Bind<StateFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<CameraFollower>().AsSingle().NonLazy();
        }

        private void InstallCoreServices()
        {
            Container.BindInstance(_levelingConfig).AsSingle();
            Container.BindInstance(_matchConfig).AsSingle();
            Container.BindInstance(_arenaBoundsService).AsSingle();
            
            Container.Bind<SpatialCollisionService>().AsSingle();
            Container.Bind<HeroStatsModifierService>().AsSingle();
            Container.Bind<LevelingService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<MatchDurationSystem>().AsSingle().NonLazy();
        }

        private void InstallPools()
        {
            Container.Bind<Pooling.ObjectPool<Bullet>>()
                .AsSingle()
                .WithArguments(_bulletPrefab, _bulletsParent, _initialBulletCapacity);

            Container.Bind<Pooling.ObjectPool<Enemy>>()
                .AsSingle()
                .WithArguments(_enemyPrefab, _enemiesParent, _initialEnemyCapacity);
            
            Container.Bind<Pooling.ObjectPool<XPGem>>().
                AsSingle().
                WithArguments(_xpGemPrefab, _xpGemsParent, _initialGemCapacity);
        }
        
        private void InstallGameplayManagers()
        {
            Container.BindInstance(_waveConfig).AsSingle();
            
            Container.BindInterfacesAndSelfTo<BulletManager>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<EnemyManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<EnemyWaveSpawner>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<XPGemManager>().AsSingle().NonLazy();
        }

        private void InstallHero()
        {
            Container.BindInstance(_heroConfig).AsSingle();
            Container.BindInstance(_weaponConfig).AsSingle();
            Container.Bind<HeroRuntimeStats>().AsSingle();
            
            Container.Bind<HeroView>()
                .FromComponentInNewPrefab(_heroViewPrefab)
                .UnderTransform(_heroSpawnPoint)
                .AsSingle();

            Container.Bind<Rigidbody>()
                .FromResolveGetter<HeroView>(view => view.Rigidbody)
                .AsSingle();
            
            Container.Bind<HeroMover>().AsSingle();
            Container.Bind<AutoCombatWeapon>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<HeroMovementSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<HeroRotationSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<HeroCombatSystem>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<HeroHealthSystem>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<HeroEntity>().AsSingle().NonLazy();
        }

        private void InstallHUDModules()
        {
            Container.BindInterfacesAndSelfTo<TimerView>().FromInstance(_timerView).AsSingle();
            Container.BindInterfacesAndSelfTo<TimerPresenter>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<WaveHUDView>().FromInstance(_waveHUDView).AsSingle();
            Container.BindInterfacesAndSelfTo<WaveHUDPresenter>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<GameplayHUDView>().FromInstance(_gameplayHUDView).AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayHUDPresenter>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<VolumeVignetteVisual>()
                .AsSingle()
                .WithArguments(_globalVolume)
                .NonLazy();
        }

        private void InstallProgressionUI()
        {
            Container.BindInstance(_upgradeDatabase).AsSingle();
            Container.BindInterfacesTo<UpgradeWindowView>().FromInstance(_upgradeWindowView).AsSingle();
            Container.BindInterfacesAndSelfTo<UpgradeWindowPresenter>().AsSingle().NonLazy();
        }
        
        private void InstallGameOverUI()
        {
            Container.BindInterfacesAndSelfTo<GameOverWindowView>().FromInstance(_gameOverWindowView).AsSingle();
            Container.BindInterfacesAndSelfTo<GameOverPresenter>().AsSingle().NonLazy();
        }
        
        private void ValidateInInspector()
        {
            if (!_heroConfig) Debug.LogError("[GameSceneInstaller] HeroConfig is not assigned!", this);
            if (!_weaponConfig) Debug.LogError("[GameSceneInstaller] WeaponConfig is not assigned!", this);
            if (!_waveConfig) Debug.LogError("[GameSceneInstaller] WaveConfig is not assigned!", this);
            if (!_levelingConfig) Debug.LogError("[GameSceneInstaller] LevelingConfig is not assigned!", this);
            if (!_matchConfig) Debug.LogError("[GameSceneInstaller] MatchConfig is not assigned!", this);
            if (!_heroViewPrefab) Debug.LogError("[GameSceneInstaller] HeroViewPrefab is not assigned!", this);
            if (!_heroSpawnPoint) Debug.LogError("[GameSceneInstaller] HeroSpawnPoint Transform is not assigned!", this);
            if (!_bulletPrefab) Debug.LogError("[GameSceneInstaller] BulletPrefab is not assigned!", this);
            if (!_bulletsParent) Debug.LogError("[GameSceneInstaller] BulletsParent is not assigned!", this);
            if (!_enemyPrefab) Debug.LogError("[GameSceneInstaller] EnemyPrefab is not assigned!", this);
            if (!_enemiesParent) Debug.LogError("[GameSceneInstaller] EnemiesParent is not assigned!", this);
            if (!_gameplayHUDView) Debug.LogError("[GameSceneInstaller] HUDView is not assigned!", this);
            if (!_upgradeDatabase) Debug.LogError("[GameSceneInstaller] UpgradeDatabase is not assigned!", this);
            if (!_upgradeWindowView) Debug.LogError("[GameSceneInstaller] UpgradeWindowView is not assigned!", this);
            if (!_timerView) Debug.LogError("[GameSceneInstaller] TimerView is not assigned!", this);
            if (!_waveHUDView) Debug.LogError("[GameSceneInstaller] WaveHUDView is not assigned!", this);
            if (!_gameOverWindowView) Debug.LogError("[GameSceneInstaller] GameOverWindowView is not assigned!", this);
            if (!_xpGemPrefab) Debug.LogError("[GameSceneInstaller] XpGemPrefab is missing!", this);
            if (!_xpGemsParent) Debug.LogError("[GameSceneInstaller] XpGemsParent is missing!", this);
            if (!_globalVolume) Debug.LogError("[GameSceneInstaller] GlobalVolume is not assigned!", this);
            if (!_arenaBoundsService) Debug.LogError("[GameSceneInstaller] ArenaBoundsService is not assigned!", this);
        }
    }
}