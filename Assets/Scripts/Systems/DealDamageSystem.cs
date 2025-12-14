using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class DealDamageSystem : ISystem 
{
    private Filter _playerFilter;
    private Stash<TargetsComponent> _targetStash;
    private Stash<TargetCountComponent> _targetCountStash;
    private Stash<HealthComponent> _healthStash;
    private Stash<DamagePerSecondComponent> _damagePerSecondStash;
    private Stash<DeadComponent> _deadStash;
    
    public World World { get; set;}

    public void OnAwake() 
    {
        _playerFilter = World.Filter.With<PlayerComponent>().With<TargetsComponent>().Build();
        _targetStash = World.GetStash<TargetsComponent>();
        _targetCountStash = World.GetStash<TargetCountComponent>();
        _healthStash = World.GetStash<HealthComponent>();
        _damagePerSecondStash = World.GetStash<DamagePerSecondComponent>();
        _deadStash = World.GetStash<DeadComponent>();
    }

    public void OnUpdate(float deltaTime) 
    {
        var playerEntity = _playerFilter.First();
        ref var targetCount = ref _targetCountStash.Get(playerEntity).currentTarget;
        
        if (targetCount == 0)
            return;
        
        ref var damagePerSecond = ref _damagePerSecondStash.Get(playerEntity).value;
        var damageInFrame = damagePerSecond * deltaTime;
        ref var targets = ref _targetStash.Get(playerEntity).targets;
        
        for (var i = 0; i < targetCount; i++)
        {
            if (_deadStash.Has(targets[i]))
                continue;
            
            var targetEntity = targets[i];
            ref var health = ref _healthStash.Get(targetEntity).value;
            health -= damageInFrame;
            
            if (health <= 0)
            {
                _deadStash.Add(targetEntity);
            }
        }
    }

    public void Dispose()
    {
    }
}