using ArenaShooter.Configs;
using ArenaShooter.Configs.Upgrades;
using ArenaShooter.Gameplay.Enemies;
using ArenaShooter.Gameplay.Hero;
using ArenaShooter.Gameplay.Weapons;
using ArenaShooter.Infrastructure.Pooling;
using ArenaShooter.Infrastructure.Signals;
using ArenaShooter.Services.Combat;
using ArenaShooter.Services.Input;
using ArenaShooter.Services.Progression;
using ArenaShooter.UI;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Infrastructure.DI
{
    public class GameSceneInstaller : MonoInstaller
    {
        [Header("Configs")] [SerializeField] private HeroConfig _heroConfig;

        [Header("Prefabs & Spawn Points")] [SerializeField]
        private HeroView _heroViewPrefab;

        [SerializeField] private Transform _heroSpawnPoint;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _bulletsParent;
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Transform _enemiesParent;

        [Header("Pool Capacities")] 
        [SerializeField] private int _initialBulletCapacity = 128;
        [SerializeField] private int _initialEnemyCapacity = 64;

        [Header("UI Views")]
        [SerializeField] private GameplayHUDView _hudView;
        
        [Header("Progression Databases")]
        [SerializeField] private UpgradeDatabase _upgradeDatabase;
        
        public override void InstallBindings()
        {
            ValidateInInspector();

            Container.BindInstance(_heroConfig).AsSingle();

            Container.BindInterfacesAndSelfTo<NewInputService>().AsSingle().NonLazy();

            Container.Bind<SignalBus>().AsSingle();
            
            Container.Bind<ObjectPool<Bullet>>()
                .AsSingle()
                .WithArguments(_bulletPrefab, _bulletsParent, _initialBulletCapacity);

            Container.Bind<SpatialCollisionService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BulletManager>().AsSingle().NonLazy();
            Container.Bind<AutoCombatWeapon>().AsSingle();

            Container.Bind<ObjectPool<Enemy>>()
                .AsSingle()
                .WithArguments(_enemyPrefab, _enemiesParent, _initialEnemyCapacity);

            Container.BindInterfacesAndSelfTo<EnemyManager>().AsSingle().NonLazy();

            Container.Bind<HeroView>()
                .FromComponentInNewPrefab(_heroViewPrefab)
                .UnderTransform(_heroSpawnPoint)
                .AsSingle();

            Container.Bind<HeroMover>().AsSingle();

            Container.BindInterfacesAndSelfTo<HeroEntity>().AsSingle().NonLazy();
            
            Container.BindInstance(_hudView).AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayHUDPresenter>().AsSingle().NonLazy();
            
            Container.BindInstance(_upgradeDatabase).AsSingle();
            Container.Bind<HeroStatsModifierService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<LevelingService>().AsSingle().NonLazy();
        }

        private void ValidateInInspector()
        {
            if (!_heroConfig) Debug.LogError("[GameSceneInstaller] HeroConfig is not assigned!", this);
            if (!_heroViewPrefab) Debug.LogError("[GameSceneInstaller] HeroViewPrefab is not assigned!", this);
            if (!_heroSpawnPoint) Debug.LogError("[GameSceneInstaller] HeroSpawnPoint Transform is not assigned!", this);
            if (!_bulletPrefab) Debug.LogError("[GameSceneInstaller] BulletPrefab is not assigned!", this);
            if (!_bulletsParent) Debug.LogError("[GameSceneInstaller] BulletsParent is not assigned!", this);
            if (!_enemyPrefab) Debug.LogError("[GameSceneInstaller] EnemyPrefab is not assigned!", this);
            if (!_enemiesParent) Debug.LogError("[GameSceneInstaller] EnemiesParent is not assigned!", this);
            if (!_hudView) Debug.LogError("[GameSceneInstaller] HUDView is not assigned!", this);
            if (!_upgradeDatabase) Debug.LogError("[GameSceneInstaller] UpgradeDatabase is not assigned!", this);
        }
    }
}