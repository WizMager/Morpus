using Configs;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class SearchTargetSystem : IFixedSystem //may be refactoring for use Spatial Hash if need too much enemies
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
        var playerPosition = _transformStash.Get(playerEntity).transform.position;
        ref var attackRadius = ref _attackRadiusStash.Get(playerEntity).value;
        var attackRadiusSqr = attackRadius * attackRadius;
        for (var i = 0; i < _distances.Length; i++)
        {
            _distances[i] = float.MaxValue;
        }
        
        var targetCounter = 0;
        
        foreach (var enemy in _enemyFilter)
        {
            var sqrDistance = (_transformStash.Get(enemy).transform.position - playerPosition).sqrMagnitude;
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
                
                if (targetCounter < _playerConfig.MaxTarget)
                    targetCounter++;
            }
        }
        
        if (targetCounter == 0)
            return;
        
        _targetCountStash.Set(playerEntity, new TargetCountComponent
        {
            currentTarget = targetCounter,
        });
        _targetsStash.Set(playerEntity, new TargetsComponent
        {
            targets = _targets
        });
    }

    public void Dispose()
    {
    }
}