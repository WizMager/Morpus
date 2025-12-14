using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private int _maxEnemyNumber = 10;
        [SerializeField] private float _enemySpawnRadius;
        [SerializeField] private float _safeRadius = 1.5f;
        
        public int MaxEnemyNumber => _maxEnemyNumber;
        public float EnemySpawnRadius => _enemySpawnRadius;
        public float SafeRadius => _safeRadius;
    }
}