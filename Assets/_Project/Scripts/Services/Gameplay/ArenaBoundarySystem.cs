using System;
using ArenaShooter.Features.Enemies.Components;
using ArenaShooter.Features.Hero.Components;
using UnityEngine;
using Zenject;

namespace ArenaShooter.Services.Gameplay
{
    public class ArenaBoundarySystem : ITickable
    {
        private readonly HeroView _heroView;
        private readonly EnemyManager _enemyManager;
        private readonly ArenaBoundsService _boundsService;

        public ArenaBoundarySystem(
            HeroView heroView, 
            EnemyManager enemyManager, 
            ArenaBoundsService boundsService)
        {
            _heroView = heroView ?? throw new ArgumentNullException(nameof(heroView));
            _enemyManager = enemyManager ?? throw new ArgumentNullException(nameof(enemyManager));
            _boundsService = boundsService ?? throw new ArgumentNullException(nameof(boundsService));
        }

        public void Tick()
        {
            Rigidbody heroRb = _heroView.Rigidbody;
            Vector3 clampedHeroPos = _boundsService.ClampToArena(heroRb.position);
            if (clampedHeroPos != heroRb.position)
            {
                heroRb.position = clampedHeroPos;
            }
            
            var enemies = _enemyManager.ActiveEnemies;
            for (int i = 0; i < enemies.Count; i++)
            {
                EnemyEntity enemy = enemies[i];
                if (enemy.IsActive)
                {
                    Transform enemyTransform = enemy.View.Transform;
                    enemyTransform.position = _boundsService.ClampToArena(enemyTransform.position);
                }
            }
        }
    }
}