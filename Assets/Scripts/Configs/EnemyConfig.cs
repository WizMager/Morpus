using UnityEngine;
using Utils;

namespace Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] private EEnemy _enemyType;
        [SerializeField] private float _health;
        
        public EEnemy EnemyType => _enemyType;
        public float Health => _health;
    }
}