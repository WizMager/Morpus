using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class PlayerMoveSystem : ISystem
{
    private Filter _filter;
    private Stash<MoveDirectionComponent> _directionStash;
    private Stash<PlayerComponent> _playerStash;

    public World World { get; set; }

    public void OnAwake()
    {
        _filter = World.Filter.With<PlayerComponent>().With<MoveDirectionComponent>().Build();
        _directionStash = World.GetStash<MoveDirectionComponent>();
        _playerStash = World.GetStash<PlayerComponent>();
    }

    public void OnUpdate(float deltaTime)
    {
        ref var directionComponent = ref _directionStash.Get(_filter.First());
        
        if (directionComponent.direction == Vector2.zero)
            return;

        ref var playerComponent = ref _playerStash.Get(_filter.First());
        playerComponent.playerTransform.Translate(new Vector3(directionComponent.direction.x, 0, directionComponent.direction.y) * playerComponent.playerConfig.MoveSpeed * deltaTime);
    }

    public void Dispose()
    {
    }
}