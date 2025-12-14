using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class DeadSystem : ISystem 
{
    private Filter _deadFilter;
    private Stash<DeadComponent> _deadStash;
    private Stash<EnemyComponent> _enemyStash;
    private Stash<DIsableComponent> _disableStash;
    
    public World World { get; set;}

    public void OnAwake() 
    {
        _deadFilter = World.Filter.With<DeadComponent>().Without<DIsableComponent>().Build();
        _deadStash = World.GetStash<DeadComponent>();
        _enemyStash = World.GetStash<EnemyComponent>();
        _disableStash = World.GetStash<DIsableComponent>();
    }

    public void OnUpdate(float deltaTime) 
    {
        foreach (var deadEntity in _deadFilter)
        {
            ref var enemyEntity = ref _enemyStash.Get(deadEntity);
            enemyEntity.enemyView.VisibleEnable(false);
            _deadStash.Remove(deadEntity);
            _disableStash.Add(deadEntity);
        }
    }

    public void Dispose()
    {
    }
}