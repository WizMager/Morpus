using System;
using UnityEngine;
using Utils;

namespace Configs.Enemy
{
    [Serializable]
    public struct EnemyData
    {
        public EEnemy enemyType;
        public float health;
        public Color color;
    }
}