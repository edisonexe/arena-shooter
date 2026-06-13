using ArenaShooter.Configs;
using ArenaShooter.Gameplay.Hero;
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

        public override void InstallBindings()
        {
            ValidateInInspector();
            
            Container.BindInstance(_heroConfig).AsSingle();
            
            Container.BindInterfacesAndSelfTo<NewInputService>().AsSingle().NonLazy();

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
        }
    }
}