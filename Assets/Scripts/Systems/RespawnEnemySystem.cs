using System;
using Configs.Enemy;
using Scellecs.Morpeh;
using Services.SpawnEnemyPosition;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class RespawnEnemySystem : ISystem 
{
    private readonly ISpawnEnemyPositionService _spawnEnemyPositionService;
    private readonly EnemyConfig _enemyConfig;
    
    private Filter _deadFilter;
    private Stash<DeadComponent> _deadStash;
    private Stash<RendererComponent> _rendererStash;
    private Stash<TransformComponent> _transformStash;
    private Stash<HealthComponent> _healthStash;
    
    private MaterialPropertyBlock _materialPropertyBlock;

    public RespawnEnemySystem(
        ISpawnEnemyPositionService spawnEnemyPositionService, 
        EnemyConfig enemyConfig
    )
    {
        _spawnEnemyPositionService = spawnEnemyPositionService;
        _enemyConfig = enemyConfig;
    }

    public World World { get; set;}

    public void OnAwake() 
    {
        _deadFilter = World.Filter.With<DeadComponent>().Build();
        _deadStash = World.GetStash<DeadComponent>();
        _rendererStash = World.GetStash<RendererComponent>();
        _transformStash = World.GetStash<TransformComponent>();
        _healthStash = World.GetStash<HealthComponent>();
        
        _materialPropertyBlock = new MaterialPropertyBlock();
    }

    public void OnUpdate(float deltaTime) 
    {
        foreach (var deadEntity in _deadFilter)
        {
            _deadStash.Remove(deadEntity);
            
            var randomEnemyType = (EEnemy)Random.Range(0, Enum.GetNames(typeof(EEnemy)).Length);
            var enemyData = _enemyConfig.GetEnemyData(randomEnemyType);
            
            SetHealth(deadEntity, enemyData.health);
            SetPosition(deadEntity);
            SetColor(deadEntity, enemyData.color);
        }
    }

    private void SetHealth(Entity enemyEntity, float newHealth)
    {
        ref var health = ref _healthStash.Get(enemyEntity);
        
        health.value = newHealth;
    }
    
    private void SetPosition(Entity enemyEntity)
    {
        ref var transform = ref _transformStash.Get(enemyEntity);
        
        transform.transform.position = _spawnEnemyPositionService.GetPosition();
    }
    
    private void SetColor(Entity entity, Color color)
    {
        ref var renderer = ref _rendererStash.Get(entity);
        
        renderer.value.GetPropertyBlock(_materialPropertyBlock);
        _materialPropertyBlock.SetColor("_BaseColor", color);
        renderer.value.SetPropertyBlock(_materialPropertyBlock);
    }
    
    public void Dispose()
    {
    }
}