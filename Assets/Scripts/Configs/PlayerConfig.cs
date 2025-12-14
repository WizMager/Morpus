using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private float _moveSpeed = 1;
        [SerializeField] private float _damagePerSecond = 10;
        [SerializeField] private float _damageRadius = 2;
        [SerializeField] private int _maxTarget = 3;
        
        public float MoveSpeed => _moveSpeed;
        public float DamagePerSecond => _damagePerSecond;
        public float DamageRadius => _damageRadius;
        public int MaxTarget => _maxTarget;
    }
}