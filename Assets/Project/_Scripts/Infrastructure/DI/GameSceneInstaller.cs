using ArenaShooter.Configs;
using ArenaShooter.Gameplay.Hero;
using ArenaShooter.Gameplay.Weapons;
using ArenaShooter.Infrastructure.Pooling;
using ArenaShooter.Services.Input;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Infrastructure.DI
{
    public class GameSceneInstaller : MonoInstaller
    {
        [Header("Configs")]
        [SerializeField] private HeroConfig _heroConfig;

        [Header("Prefabs & Spawn Points")]
        [SerializeField] private HeroView _heroViewPrefab;
        [SerializeField] private Transform _heroSpawnPoint;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _bulletsParent;

        [Header("Pool Capacities")]
        [SerializeField] private int _initialBulletCapacity = 64;
        
        public override void InstallBindings()
        {
            ValidateInInspector();
            
            Container.BindInstance(_heroConfig).AsSingle();
            
            Container.BindInterfacesAndSelfTo<NewInputService>().AsSingle().NonLazy();

            Container.Bind<ObjectPool<Bullet>>()
                .AsSingle()
                .WithArguments(_bulletPrefab, _bulletsParent, _initialBulletCapacity);
            
            Container.BindInterfacesAndSelfTo<BulletManager>().AsSingle().NonLazy();
            
            HeroView heroViewInstance = Container.InstantiatePrefabForComponent<HeroView>(
                _heroViewPrefab, 
                _heroSpawnPoint.position, 
                Quaternion.identity, 
                null
            );
            Container.BindInstance(heroViewInstance).AsSingle();

            Container.Bind<HeroMover>()
                .AsSingle()
                .WithArguments(heroViewInstance.Rigidbody);

            Container.BindInterfacesAndSelfTo<HeroEntity>()
                .AsSingle()
                .NonLazy();
        }

        private void ValidateInInspector()
        {
            if (!_heroConfig) Debug.LogError("[GameSceneInstaller] HeroConfig is not assigned in the Inspector!", this);
            if (!_heroViewPrefab) Debug.LogError("[GameSceneInstaller] HeroViewPrefab is not assigned in the Inspector!", this);
            if (!_heroSpawnPoint) Debug.LogError("[GameSceneInstaller] HeroSpawnPoint Transform is not assigned in the Inspector!", this);
            if (!_bulletPrefab) Debug.LogError("[GameSceneInstaller] BulletPrefab is not assigned!", this);
            if (!_bulletsParent) Debug.LogError("[GameSceneInstaller] BulletsParent is not assigned!", this);
        }
    }
}