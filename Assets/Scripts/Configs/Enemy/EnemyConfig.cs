using UnityEngine;
using Utils;

namespace Configs.Enemy
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] private EnemyData[] _enemiesData;

        public EnemyData GetEnemyData(EEnemy enemy)
        {
            foreach (var enemyData in _enemiesData)
            {
                if (enemyData.enemyType != enemy)
                    continue;
                
                return enemyData;
            }
            
            Debug.LogError($"[{nameof(EnemyConfig)}]: There is no enemy data for {enemy}");
            
            return new EnemyData();
        }
    }
}