using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public struct EnemyData
    {
        public EEnemy enemyType;
        public float health;
        public Color color;
    }
}