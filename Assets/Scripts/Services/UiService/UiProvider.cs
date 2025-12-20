using System;
using Configs;
using Scellecs.Morpeh;
using UniRx;
using Utils;
using Random = UnityEngine.Random;

namespace Services.UiService
{
    public class UiProvider : IUiProvider
    {
        private readonly ReactiveCommand<StatData> _statUpCommand = new ReactiveCommand<StatData>();
        private readonly ReactiveCommand<int> _killCounterUpdateCommand = new ReactiveCommand<int>();
        private readonly PlayerConfig _playerConfig;
        private readonly World _world;
        private readonly Filter _playerFilter;
        private readonly Stash<LevelUpComponent> _levelUpStash;
        private readonly Stash<MoveSpeedComponent> _moveSpeedStash;
        private readonly Stash<DamagePerSecondComponent> _damagePerSecondStash;
        private readonly Stash<AttackRadiusSqrComponent> _attackRadiusStash;

        public UiProvider(
            PlayerConfig playerConfig, 
            World world
        )
        {
            _playerConfig = playerConfig;
            _world = world;
            
            _playerFilter = _world.Filter.With<PlayerComponent>().Build();
            _levelUpStash = _world.GetStash<LevelUpComponent>();
            _moveSpeedStash = _world.GetStash<MoveSpeedComponent>();
            _damagePerSecondStash = _world.GetStash<DamagePerSecondComponent>();
            _attackRadiusStash = _world.GetStash<AttackRadiusSqrComponent>();
        }

        public IObservable<StatData> OnStatUp => _statUpCommand;
        public IObservable<int> OnKillCounterUpdate => _killCounterUpdateCommand;

        public void OnLevelUp()
        {
            var randomChance = Random.Range(0f, 100f);
            var selectedStat = GetLevelUpStat(randomChance);
            var statIncreaseValue = _playerConfig.LevelUpStep(selectedStat);
            var levelUpEntity = _world.CreateEntity();
            _levelUpStash.Add(levelUpEntity, new LevelUpComponent
            {
                statData = new StatData
                {
                    stat = selectedStat, 
                    value = statIncreaseValue
                }
            });
            
            statIncreaseValue += GetCurrentStat(selectedStat);
            _statUpCommand.Execute(new StatData
            {
                stat = selectedStat, value = statIncreaseValue
            });
        }

        private float GetCurrentStat(EStat stat)
        {
            var playerEntity = _playerFilter.First();
            
            switch (stat)
            {
                case EStat.MoveSpeed:
                    return _moveSpeedStash.Get(playerEntity).value;
                case EStat.DamagePerSecond:
                    return _damagePerSecondStash.Get(playerEntity).value;
                case EStat.AttackRadius:
                    return _attackRadiusStash.Get(playerEntity).value;
            }
            
            return -1f;
        }
        
        public void AddKillCounter(int killCount)
        {
            _killCounterUpdateCommand.Execute(killCount);
        }

        private EStat GetLevelUpStat(float randomValue)
        {
            var moveSpeedChance = _playerConfig.StatUpChance(EStat.MoveSpeed);
            var dpsChance = _playerConfig.StatUpChance(EStat.DamagePerSecond);
            var radiusChance = _playerConfig.StatUpChance(EStat.AttackRadius);
            
            var cumulative = 0f;
    
            cumulative += moveSpeedChance;
            if (randomValue < cumulative) 
                return EStat.MoveSpeed;
    
            cumulative += dpsChance;
            if (randomValue < cumulative) 
                return EStat.DamagePerSecond;
    
            cumulative += radiusChance;
            if (randomValue < cumulative) 
                return EStat.AttackRadius;
            
            return EStat.MoveSpeed;
        }
    }
}