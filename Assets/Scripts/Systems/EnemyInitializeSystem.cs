using Configs;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class EnemyInitializeSystem : IInitializer
{
    private readonly GameConfig _gameConfig;
    private readonly EnemyConfig _enemyConfig;
    
    public World World { get; set;}

    public EnemyInitializeSystem(
        GameConfig gameConfig, 
        EnemyConfig enemyConfig
    )
    {
        _gameConfig = gameConfig;
        _enemyConfig = enemyConfig;
    }

    public void OnAwake()
    {
        
        
    }

    public void Dispose()
    {

    }
}