using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class DealDamageSystem : ISystem 
{
    private Filter _playerFilter;
    private Filter _targetsFilter;
    private Stash<HealthComponent> _healthStash;
    private Stash<DamagePerSecondComponent> _damagePerSecondStash;
    private Stash<DeadComponent> _deadStash;
    private Stash<TargetComponent> _targetStash;
    
    public World World { get; set;}

    public void OnAwake() 
    {
        _playerFilter = World.Filter.With<PlayerComponent>().Build();
        _targetsFilter = World.Filter.With<TargetComponent>().With<HealthComponent>().Without<DeadComponent>().Build();
        _healthStash = World.GetStash<HealthComponent>();
        _damagePerSecondStash = World.GetStash<DamagePerSecondComponent>();
        _deadStash = World.GetStash<DeadComponent>();
        _targetStash = World.GetStash<TargetComponent>();
    }

    public void OnUpdate(float deltaTime) 
    {
        var playerEntity = _playerFilter.First();
        ref var damagePerSecond = ref _damagePerSecondStash.Get(playerEntity);
        var damageInFrame = damagePerSecond.value * deltaTime;

        foreach (var targetEntity in _targetsFilter)
        {
            ref var health = ref _healthStash.Get(targetEntity);
            
            health.value -= damageInFrame;
            
            if (health.value <= 0)
            {
                _deadStash.Add(targetEntity);
                _targetStash.Remove(targetEntity);
            }
        }
    }

    public void Dispose()
    {
    }
}