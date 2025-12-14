using System;
using Configs.Enemy;
using Scellecs.Morpeh;
using Services.SpawnEnemyPosition;
using Services.UiService;
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
    private readonly IUiProvider _uiProvider;
    
    private Filter _deadFilter;
    private Filter _playerFilter;
    private Stash<DeadComponent> _deadStash;
    private Stash<RendererComponent> _rendererStash;
    private Stash<TransformComponent> _transformStash;
    private Stash<HealthComponent> _healthStash;
    private Stash<KillCounterComponent> _killCounterStash;
    private Stash<TargetCountComponent> _targetCountStash;
    private Stash<TargetsComponent> _targetsStash;
    
    private MaterialPropertyBlock _materialPropertyBlock;

    public RespawnEnemySystem(
        ISpawnEnemyPositionService spawnEnemyPositionService, 
        EnemyConfig enemyConfig, 
        IUiProvider uiProvider
    )
    {
        _spawnEnemyPositionService = spawnEnemyPositionService;
        _enemyConfig = enemyConfig;
        _uiProvider = uiProvider;
    }

    public World World { get; set;}

    public void OnAwake() 
    {
        _deadFilter = World.Filter.With<DeadComponent>().Build();
        _playerFilter = World.Filter.With<PlayerComponent>().Build();
        _deadStash = World.GetStash<DeadComponent>();
        _rendererStash = World.GetStash<RendererComponent>();
        _transformStash = World.GetStash<TransformComponent>();
        _healthStash = World.GetStash<HealthComponent>();
        _killCounterStash = World.GetStash<KillCounterComponent>();
        _targetCountStash = World.GetStash<TargetCountComponent>();
        _targetsStash = World.GetStash<TargetsComponent>();
        
        _materialPropertyBlock = new MaterialPropertyBlock();
    }

    public void OnUpdate(float deltaTime) 
    {
        foreach (var deadEntity in _deadFilter)
        {
            ref var targets = ref _targetsStash.Get(_playerFilter.First());
            ref var targetCount = ref _targetCountStash.Get(_playerFilter.First());

            for (var i = 0; i < targets.targets.Length; i++)
            {
                if (targets.targets[i] != deadEntity)
                    continue;
                
                targets.activeTarget[i] = false;
                targetCount.targets--;
            }
            
            _deadStash.Remove(deadEntity);
            
            ref var killCounter = ref _killCounterStash.Get(_playerFilter.First());
            killCounter.value++;
            _uiProvider.AddKillCounter(killCounter.value);
            
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