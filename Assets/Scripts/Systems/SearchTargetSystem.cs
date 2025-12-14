using Configs;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class SearchTargetSystem : IFixedSystem //need refactoring for use Spatial Hash if need too much enemies
{
    private readonly PlayerConfig _playerConfig;
    
    private Filter _playerFilter;
    private Filter _enemyFilter;
    private Stash<TransformComponent> _transformStash;
    private Stash<TargetsComponent> _targetsStash;
    private Stash<TargetCountComponent> _targetCountStash;
    private Stash<AttackRadiusComponent> _attackRadiusStash;
    
    private float[] _distances;
    private Entity[] _targets;

    public SearchTargetSystem(PlayerConfig playerConfig)
    {
        _playerConfig = playerConfig;
    }

    public World World { get; set;}

    public void OnAwake() 
    {
        _playerFilter = World.Filter.With<PlayerComponent>().With<TargetsComponent>().With<TargetCountComponent>().Build();
        _enemyFilter = World.Filter.With<EnemyComponent>().Without<DeadComponent>().Build();
        _transformStash = World.GetStash<TransformComponent>();
        _targetsStash = World.GetStash<TargetsComponent>();
        _targetCountStash = World.GetStash<TargetCountComponent>();
        _attackRadiusStash = World.GetStash<AttackRadiusComponent>();
        
        _distances = new float[_playerConfig.MaxTarget];
        _targets = new Entity[_playerConfig.MaxTarget];
    }

    public void OnUpdate(float deltaTime)
    {
        var playerEntity = _playerFilter.First();
        var playerPosition = _transformStash.Get(playerEntity).value.position;
        ref var attackRadius = ref _attackRadiusStash.Get(playerEntity).value;
        var attackRadiusSqr = attackRadius * attackRadius;
        for (var i = 0; i < _distances.Length; i++)
        {
            _distances[i] = float.MaxValue;
        }
        
        ref var targetCount = ref _targetCountStash.Get(playerEntity);
        targetCount.targetsNumber = 0;
        
        foreach (var enemy in _enemyFilter)
        {
            var sqrDistance = (_transformStash.Get(enemy).value.position - playerPosition).sqrMagnitude;
            if (sqrDistance > attackRadiusSqr)
                continue;
            
            for (var i = 0; i < _distances.Length; i++)
            {
                if (sqrDistance >= _distances[i])
                    continue;
                
                for (var j = _distances.Length - 1; j > i; j--)
                {
                    _distances[j] = _distances[j - 1];
                    _targets[j] = _targets[j - 1];
                }

                _distances[i] = sqrDistance;
                _targets[i] = enemy;
                
                break;
            }
        }

        var hasTargets = false;
        var targets = 0;
        ref var targetsComponent = ref _targetsStash.Get(playerEntity);
        
        for (var i = 0; i < _distances.Length; i++)
        {
            if (_distances[i] > attackRadiusSqr)
                continue;

            targetsComponent.activeTarget[i] = true;
            hasTargets = true;
            targets++;
        }
        
        if (!hasTargets)
            return;
        
        targetCount.targetsNumber = targets;
        targetsComponent.targets = _targets;
    }

    public void Dispose()
    {
    }
}