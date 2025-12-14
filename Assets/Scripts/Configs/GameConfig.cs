using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private int _maxEnemyNumber = 10;
        [SerializeField] private float _enemySpawnRadius;
        
        public int MaxEnemyNumber => _maxEnemyNumber;
        public float EnemySpawnRadius => _enemySpawnRadius;
    }
}