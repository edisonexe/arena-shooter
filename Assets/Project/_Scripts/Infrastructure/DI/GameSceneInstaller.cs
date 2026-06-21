using ArenaShooter.Configs;
using ArenaShooter.Configs.Enemies;
using ArenaShooter.Configs.Upgrades;
using ArenaShooter.Gameplay.Enemies;
using ArenaShooter.Gameplay.Hero;
using ArenaShooter.Gameplay.Items;
using ArenaShooter.Gameplay.Weapons;
using ArenaShooter.Gameplay.Camera;
using ArenaShooter.Infrastructure.Reset;
using ArenaShooter.Services.Combat;
using ArenaShooter.Services.Gameplay;
using ArenaShooter.Services.Progression;
using ArenaShooter.Services.UpgradesCalculation;
using ArenaShooter.UI.GameOver;
using ArenaShooter.UI.HUD;
using ArenaShooter.UI.Upgrades;
using ArenaShooter.UI.Victory;
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
        [SerializeField] private UpgradeDatabase _upgradeDatabase;
        
        [Header("Prefabs & Spawn Points")] 
        [SerializeField] private HeroView _heroViewPrefab;
        [SerializeField] private Transform _heroSpawnPoint;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _bulletsParent;
        [SerializeField] private Transform _enemiesParent;
        [SerializeField] private XPGem _xpGemPrefab;
        [SerializeField] private Transform _xpGemsParent;
        
        [Header("Pool Capacities")] 
        [SerializeField, Min(0)] private int _initialBulletCapacity = 128;
        [SerializeField, Min(0)] private int _initialGemCapacity = 128;
        [SerializeField, Min(0)] private int _initialEnemiesCapacity = 32;
        
        [Header("UI Views")]
        [SerializeField] private GameplayHUDView _gameplayHUDView;
        [SerializeField] private UpgradeWindowView _upgradeWindowView;
        [SerializeField] private TimerView _timerView;
        [SerializeField] private WaveHUDView _waveHUDView;
        [SerializeField] private GameOverWindowView _gameOverWindowView;
        [SerializeField] private GameVictoryWindowView _gameVictoryWindowView;
        
        [Header("Render Settings")]
        [SerializeField] private Volume _globalVolume;
        
        [Header("Gameplay Services on Scene")]
        [SerializeField] private ArenaBoundsService _arenaBoundsService;
        
        public override void InstallBindings()
        {
            ValidateInInspector();
            
            InstallSceneInfrastructure();
            InstallCoreServices();
            InstallPools();
            
            InstallGameplayManagers();
            InstallHero();
            
            InstallHUDModules();
            InstallProgressionUI();
            InstallEndGameUI();
            
            InstallResetSystem();
        }

        private void InstallSceneInfrastructure()
        {
            Container.BindInterfacesAndSelfTo<CameraFollower>().AsSingle().NonLazy();
        }

        private void InstallCoreServices()
        {
            Container.BindInstance(_levelingConfig).AsSingle();
            Container.BindInstance(_arenaBoundsService).AsSingle();
            
            Container.Bind<SpatialCollisionService>().AsSingle();
            
            Container.Bind<IUpgradeCalculation>().To<MoveSpeedCalculation>().AsSingle();
            Container.Bind<IUpgradeCalculation>().To<DamageCalculation>().AsSingle();
            Container.Bind<IUpgradeCalculation>().To<FireRateCalculation>().AsSingle();
            Container.Bind<IUpgradeCalculation>().To<MaxHealthCalculation>().AsSingle();
            Container.Bind<IUpgradeCalculation>().To<RegenHealthCalculation>().AsSingle();
            Container.Bind<IUpgradeCalculation>().To<PickupRadiusCalculation>().AsSingle();
            
            Container.Bind<HeroStatsModifierService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<LevelingService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<HeroRegenSystem>().AsSingle().NonLazy();
        }

        private void InstallPools()
        {
            Container.Bind<Pooling.ObjectPool<Bullet>>()
                .AsSingle()
                .WithArguments(_bulletPrefab, _bulletsParent, _initialBulletCapacity);
  
            Container.Bind<Pooling.ObjectPool<XPGem>>()
                .AsSingle()
                .WithArguments(_xpGemPrefab, _xpGemsParent, _initialGemCapacity);
            
            Container.Bind<EnemyFactory>()
                .AsSingle()
                .WithArguments(_enemiesParent, _initialEnemiesCapacity);
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
            
            Container.Bind<HeroRotationSystem>().AsSingle();
            Container.Bind<ITickable>().To<HeroRotationSystem>().FromResolve();
            
            Container.BindInterfacesAndSelfTo<HeroCombatSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<HeroHealthSystem>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<HeroEntity>()
                .AsSingle()
                .WithArguments(_heroSpawnPoint)
                .NonLazy();
        }

        private void InstallHUDModules()
        {
            Container.BindInterfacesAndSelfTo<TimerView>().FromInstance(_timerView).AsSingle();
            Container.BindInterfacesAndSelfTo<TimerPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<WaveHUDView>().FromInstance(_waveHUDView).AsSingle();
            Container.BindInterfacesAndSelfTo<WaveHUDPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameplayHUDView>().FromInstance(_gameplayHUDView).AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayHUDPresenter>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<DamageVignetteVisual>()
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
        
        private void InstallEndGameUI()
        {
            Container.BindInterfacesAndSelfTo<GameOverWindowView>().FromInstance(_gameOverWindowView).AsSingle();
            Container.BindInterfacesAndSelfTo<GameOverPresenter>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<GameVictoryWindowView>().FromInstance(_gameVictoryWindowView).AsSingle();
            Container.BindInterfacesAndSelfTo<GameVictoryPresenter>().AsSingle().NonLazy();
        }
        
        private void InstallResetSystem()
        {
            Container.Bind<IResettable>().To<MatchDurationSystem>().FromResolve();
            Container.Bind<IResettable>().To<HeroEntity>().FromResolve();
            
            Container.BindInterfacesAndSelfTo<GameResetSystem>().AsSingle().NonLazy();
        }
        
        private void ValidateInInspector()
        {
            if (!_heroConfig) Debug.LogError("[GameSceneInstaller] HeroConfig is not assigned!", this);
            if (!_weaponConfig) Debug.LogError("[GameSceneInstaller] WeaponConfig is not assigned!", this);
            if (!_waveConfig) Debug.LogError("[GameSceneInstaller] WaveConfig is not assigned!", this);
            if (!_levelingConfig) Debug.LogError("[GameSceneInstaller] LevelingConfig is not assigned!", this);
            if (!_upgradeDatabase) Debug.LogError("[GameSceneInstaller] UpgradeDatabase is not assigned!", this);
            if (!_heroViewPrefab) Debug.LogError("[GameSceneInstaller] HeroViewPrefab is not assigned!", this);
            if (!_heroSpawnPoint) Debug.LogError("[GameSceneInstaller] HeroSpawnPoint is not assigned!", this);
            if (!_bulletPrefab) Debug.LogError("[GameSceneInstaller] BulletPrefab is not assigned!", this);
            if (!_bulletsParent) Debug.LogError("[GameSceneInstaller] BulletsParent is not assigned!", this);
            if (!_enemiesParent) Debug.LogError("[GameSceneInstaller] EnemiesParent is not assigned!", this);
            if (!_gameplayHUDView) Debug.LogError("[GameSceneInstaller] GameplayHUDView is not assigned!", this);
            if (!_upgradeWindowView) Debug.LogError("[GameSceneInstaller] UpgradeWindowView is not assigned!", this);
            if (!_timerView) Debug.LogError("[GameSceneInstaller] TimerView is not assigned!", this);
            if (!_waveHUDView) Debug.LogError("[GameSceneInstaller] WaveHUDView is not assigned!", this);
            if (!_gameOverWindowView) Debug.LogError("[GameSceneInstaller] GameOverWindowView is not assigned!", this);
            if (!_gameVictoryWindowView) Debug.LogError("[GameSceneInstaller] GameVictoryWindowView is not assigned!", this);
            if (!_xpGemPrefab) Debug.LogError("[GameSceneInstaller] XpGemPrefab is not assigned!", this);
            if (!_xpGemsParent) Debug.LogError("[GameSceneInstaller] XpGemsParent is not assigned!", this);
            if (!_globalVolume) Debug.LogError("[GameSceneInstaller] GlobalVolume is not assigned!", this);
            if (!_arenaBoundsService) Debug.LogError("[GameSceneInstaller] ArenaBoundsService is not assigned!", this);
        }
    }
}