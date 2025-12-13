using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class PlayerInitializeSystem : IInitializer 
{
    public World World { get; set;}

    public void OnAwake()
    {
        var players = World.Filter.With<PlayerComponent>().Build();
        var directionStash = World.GetStash<MoveDirectionComponent>();

        directionStash.Add(players.First(), new MoveDirectionComponent
        {
            direction = Vector2.zero
        });
    }

    public void Dispose()
    {

    }
}