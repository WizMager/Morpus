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
    private Stash<TransformComponent> _transformStash;
    private Stash<MoveSpeedComponent> _moveSpeedStash;

    public World World { get; set; }

    public void OnAwake()
    {
        _filter = World.Filter.With<PlayerComponent>().With<MoveDirectionComponent>().Build();
        _directionStash = World.GetStash<MoveDirectionComponent>();
        _transformStash = World.GetStash<TransformComponent>();
        _moveSpeedStash = World.GetStash<MoveSpeedComponent>();
    }

    public void OnUpdate(float deltaTime)
    {
        var playerEntity = _filter.First();
        ref var directionComponent = ref _directionStash.Get(playerEntity);
        
        if (directionComponent.direction == Vector2.zero)
            return;

        ref var transformComponent = ref _transformStash.Get(playerEntity);
        ref var moveSpeedComponent = ref _moveSpeedStash.Get(playerEntity);
        transformComponent.value.Translate(new Vector3(directionComponent.direction.x, 0, directionComponent.direction.y) * moveSpeedComponent.value * deltaTime);
    }

    public void Dispose()
    {
    }
}