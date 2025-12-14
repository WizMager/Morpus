using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "PrefabsConfig", menuName = "Configs/PrefabsConfig")]
    public class PrefabsConfig : ScriptableObject
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _enemyPrefab;
        
        public GameObject PlayerPrefab => _playerPrefab;
        public GameObject EnemyPrefab => _enemyPrefab;
    }
}