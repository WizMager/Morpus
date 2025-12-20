using System;
using System.Collections.Generic;
using Configs;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class SearchTargetSystem : IFixedSystem
{
    private readonly PlayerConfig _playerConfig;
    
    private Filter _playerFilter;
    private Filter _enemyFilter;
    private Filter _currentTargetsFilter;
    private Stash<TransformComponent> _transformStash;
    private Stash<AttackRadiusSqrComponent> _attackRadiusStash;
    private Stash<TargetComponent> _targetStash;

    private List<(Entity entity, float sqrDistance)> _candidates;

    public SearchTargetSystem(PlayerConfig playerConfig)
    {
        _playerConfig = playerConfig;
        _candidates = new List<(Entity entity, float sqrDistance)>(_playerConfig.MaxTarget);
    }

    public World World { get; set;}

    public void OnAwake() 
    {
        _playerFilter = World.Filter.With<PlayerComponent>().Build();
        _enemyFilter = World.Filter.With<EnemyComponent>().Without<DeadComponent>().Build();
        _currentTargetsFilter = World.Filter.With<EnemyComponent>().With<TargetComponent>().Without<DeadComponent>().Build();
        _transformStash = World.GetStash<TransformComponent>();
        _attackRadiusStash = World.GetStash<AttackRadiusSqrComponent>();
        _targetStash = World.GetStash<TargetComponent>();
    }

    public void OnUpdate(float deltaTime)
    {
        var playerEntity = _playerFilter.First();
        ref var playerTransform = ref _transformStash.Get(playerEntity).value;
        ref var attackRadiusSqr = ref _attackRadiusStash.Get(playerEntity).value;
        _candidates.Clear();
        
        foreach (var enemy in _enemyFilter)
        {
            var sqrDistance = (_transformStash.Get(enemy).value.position - playerTransform.position).sqrMagnitude;
            if (sqrDistance <= attackRadiusSqr)
            {
                _candidates.Add((enemy, sqrDistance));
            }
        }
        
        _candidates.Sort((a, b) => a.sqrDistance.CompareTo(b.sqrDistance));
        var targetsToSelect = Mathf.Min(_playerConfig.MaxTarget, _candidates.Count);
        
        var newTargets = new HashSet<Entity>();
        for (var i = 0; i < targetsToSelect; i++)
        {
            newTargets.Add(_candidates[i].entity);
        }
        
        var currentTargets = new HashSet<Entity>();
        foreach (var currentTarget in _currentTargetsFilter)
        {
            currentTargets.Add(currentTarget);
        }
        
        foreach (var currentTarget in currentTargets)
        {
            if (!newTargets.Contains(currentTarget))
            {
                _targetStash.Remove(currentTarget);
            }
        }
        
        foreach (var newTarget in newTargets)
        {
            if (!currentTargets.Contains(newTarget))
            {
                _targetStash.Add(newTarget);
            }
        }
    }

    public void Dispose()
    {
    }
}