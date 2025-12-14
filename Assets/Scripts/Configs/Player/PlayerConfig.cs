using Services.UiService;
using UnityEngine;

namespace Configs.Player
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private float _moveSpeed = 1;
        [SerializeField] private float _damagePerSecond = 10;
        [SerializeField] private float _damageRadius = 2;
        [SerializeField] private int _maxTarget = 3;
        [SerializeField] private LevelUpStep[] _levelUpStep;
        [SerializeField] private StatUpData[] _statUpData;
        
        public float MoveSpeed => _moveSpeed;
        public float DamagePerSecond => _damagePerSecond;
        public float DamageRadius => _damageRadius;
        public int MaxTarget => _maxTarget;

        public float LevelUpStep(EStat stat)
        {
            foreach (var levelUpStep in _levelUpStep)
            {
                if (levelUpStep.stat != stat)
                    continue;
                
                return levelUpStep.value;
            }

            return -1;
        }

        public float StatUpChance(EStat stat)
        {
            foreach (var statUpData in _statUpData)
            {
                if (statUpData.stat != stat)
                    continue;
                
                return statUpData.chance;
            }

            return -1;
        }
    }
}