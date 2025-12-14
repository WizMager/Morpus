using System;
using Configs;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class PlayerInitializeSystem : IInitializer 
{
    private readonly PrefabsConfig _prefabsConfig;
    private readonly PlayerConfig _playerConfig;
    
    public World World { get; set;}

    public PlayerInitializeSystem(
        PrefabsConfig prefabsConfig,
        PlayerConfig playerConfig
    )
    {
        _prefabsConfig = prefabsConfig;
        _playerConfig = playerConfig;
    }
    
    public void OnAwake()
    {
        var playerInstance = Object.Instantiate(_prefabsConfig.PlayerPrefab);
        var playerEntity = World.CreateEntity();
        
        AddPlayer(playerEntity);
        AddTargets(playerEntity);
        AddTargetsCount(playerEntity);
        AddTransform(playerEntity, playerInstance.transform);
        AddDirection(playerEntity);
        AddMoveSpeed(playerEntity);
        AddDamagePerSecond(playerEntity);
        AddAttackRadius(playerEntity);
        
        SetupCamera(playerInstance.transform);
    }

    private void AddPlayer(Entity playerEntity)
    {
        var playerStash = World.GetStash<PlayerComponent>();
        
        playerStash.Add(playerEntity, new PlayerComponent());
    }
    
    private void AddTargets(Entity playerEntity)
    {
        var targetsStash = World.GetStash<TargetsComponent>();
        
        targetsStash.Add(playerEntity, new TargetsComponent
        {
            targets = new Entity[_playerConfig.MaxTarget]
        });
    }
    
    private void AddTargetsCount(Entity playerEntity)
    {
        var targetCountStash = World.GetStash<TargetCountComponent>();
        
        targetCountStash.Add(playerEntity, new TargetCountComponent
        {
            currentTarget = 0,
        });
    }
    
    private void AddTransform(Entity playerEntity, Transform playerTransform)
    {
        var transformStash = World.GetStash<TransformComponent>();
        
        transformStash.Add(playerEntity, new TransformComponent
        {
            transform = playerTransform
        });
    }
    
    private void AddDirection(Entity playerEntity)
    {
        var directionStash = World.GetStash<MoveDirectionComponent>();
        
        directionStash.Add(playerEntity, new MoveDirectionComponent
        {
            direction = Vector2.zero
        });
    }
    
    private void AddMoveSpeed(Entity playerEntity)
    {
        var moveSpeedStash = World.GetStash<MoveSpeedComponent>();
        
        moveSpeedStash.Add(playerEntity, new MoveSpeedComponent
        {
            value = _playerConfig.MoveSpeed
        });
    }
    
    private void AddDamagePerSecond(Entity playerEntity)
    {
        var damagePerSecondStash = World.GetStash<DamagePerSecondComponent>();
        
        damagePerSecondStash.Add(playerEntity, new DamagePerSecondComponent
        {
            value = _playerConfig.DamagePerSecond
        });
    }

    private void AddAttackRadius(Entity playerEntity)
    {
        var damageRadiusStash = World.GetStash<AttackRadiusComponent>();
        
        damageRadiusStash.Add(playerEntity, new AttackRadiusComponent
        {
            value = _playerConfig.DamageRadius
        });
    }

    private void SetupCamera(Transform playerTransform)
    {
        var virtualCameraEntity = World.Filter.With<VirtualCameraComponent>().Build().First();
        var virtualCameraStash = World.GetStash<VirtualCameraComponent>();
        ref var virtualCameraComponent = ref virtualCameraStash.Get(virtualCameraEntity);
        
        virtualCameraComponent.virtualCamera.Follow = playerTransform;
        virtualCameraComponent.virtualCamera.LookAt = playerTransform;
    }
    
    public void Dispose()
    {

    }
}