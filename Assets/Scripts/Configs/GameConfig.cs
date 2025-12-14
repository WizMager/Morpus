using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private int _maxEnemyNumber = 10;
        
        public int MaxEnemyNumber => _maxEnemyNumber;
    }
}