using Scellecs.Morpeh;
using Services.UiService;
using Unity.IL2CPP.CompilerServices;
using Utils;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class LevelUpSystem : ISystem
{
    private Filter _levelUpFilter;
    private Filter _playerFilter;
    private Stash<LevelUpComponent> _levelUpStash;
    private Stash<MoveSpeedComponent> _moveSpeedStash;
    private  Stash<DamagePerSecondComponent> _damagePerSecondStash;
    private Stash<AttackRadiusSqrComponent> _attackRadiusStash;
    
    
    public World World { get; set;}

    public void OnAwake() 
    {
        _levelUpFilter = World.Filter.With<LevelUpComponent>().Build();
        _playerFilter = World.Filter.With<PlayerComponent>().Build();
        _levelUpStash = World.GetStash<LevelUpComponent>();
        _moveSpeedStash = World.GetStash<MoveSpeedComponent>();
        _damagePerSecondStash = World.GetStash<DamagePerSecondComponent>();
        _attackRadiusStash = World.GetStash<AttackRadiusSqrComponent>();
    }

    public void OnUpdate(float deltaTime) 
    {
        foreach (var entity in _levelUpFilter)
        {
            var playerEntity = _playerFilter.First();
            ref var levelUpComponent = ref _levelUpStash.Get(entity);
            
            switch (levelUpComponent.statData.stat)
            {
                case EStat.MoveSpeed:
                    ref var moveSpeedComponent = ref _moveSpeedStash.Get(playerEntity);
                    moveSpeedComponent.value += levelUpComponent.statData.value;
                    break;
                case EStat.DamagePerSecond:
                    ref var dps = ref _damagePerSecondStash.Get(playerEntity);
                    dps.value += levelUpComponent.statData.value;
                    break;
                case EStat.AttackRadius:
                    ref var radius = ref _attackRadiusStash.Get(playerEntity);
                    radius.value += levelUpComponent.statData.value;
                    break;
            }

            World.RemoveEntity(entity);
        }
    }

    public void Dispose()
    {
    }
}