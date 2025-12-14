using System;
using Configs;
using Configs.Enemy;
using Scellecs.Morpeh;
using Services.SpawnEnemyPosition;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using Utils;
using Views;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class EnemyInitializeSystem : IInitializer
{
    private readonly GameConfig _gameConfig;
    private readonly EnemyConfig _enemyConfig;
    private readonly PrefabsConfig _prefabsConfig;
    private readonly ISpawnEnemyPositionService _spawnEnemyPositionService;
    
    public World World { get; set;}

    public EnemyInitializeSystem(
        GameConfig gameConfig, 
        EnemyConfig enemyConfig, 
        PrefabsConfig prefabsConfig, 
        ISpawnEnemyPositionService spawnEnemyPositionService
    )
    {
        _gameConfig = gameConfig;
        _enemyConfig = enemyConfig;
        _prefabsConfig = prefabsConfig;
        _spawnEnemyPositionService = spawnEnemyPositionService;
    }

    public void OnAwake()
    {
        var materialPropertyBlock = new MaterialPropertyBlock();
        
        for (var i = 0; i < _gameConfig.MaxEnemyNumber; i++)
        {
            var randomEnemyType = (EEnemy)Random.Range(0, Enum.GetNames(typeof(EEnemy)).Length);
            var enemyData = _enemyConfig.GetEnemyData(randomEnemyType);
            
            var enemyView = Object.Instantiate(_prefabsConfig.EnemyPrefab, _spawnEnemyPositionService.GetPosition(), Quaternion.identity).GetComponent<EnemyView>();
            var enemyEntity = World.CreateEntity();

            AddEnemy(enemyEntity, enemyView);
            AddHealth(enemyEntity, enemyData.health);
            AddTransform(enemyEntity, enemyView.transform);
            
            SetColor(enemyView, enemyData.color, materialPropertyBlock);
        }
    }

    private void AddEnemy(Entity enemyEntity, EnemyView enemyView)
    {
        var enemyStash = World.GetStash<EnemyComponent>();
        
        enemyStash.Add(enemyEntity, new EnemyComponent
        {
            enemyView = enemyView
        });
    }
    
    private void AddHealth(Entity enemyEntity, float health)
    {
        var healthStash = World.GetStash<HealthComponent>();
        
        healthStash.Add(enemyEntity, new HealthComponent
        {
            value = health
        });
    }
    
    private void AddTransform(Entity enemyEntity, Transform transform)
    {
        var transformStash = World.GetStash<TransformComponent>();
        
        transformStash.Add(enemyEntity, new TransformComponent
        {
            transform = transform
        });
    }
    
    private void SetColor(EnemyView enemyView, Color color, MaterialPropertyBlock materialPropertyBlock)
    {
        enemyView.Renderer.GetPropertyBlock(materialPropertyBlock);
        materialPropertyBlock.SetColor("_BaseColor", color);
        enemyView.Renderer.SetPropertyBlock(materialPropertyBlock);
    }

    public void Dispose()
    {

    }
}