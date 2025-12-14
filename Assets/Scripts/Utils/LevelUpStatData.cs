using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public struct LevelUpStatData
    {
        public EStat stat;
        public float value;
        [Range(0, 100f)]
        public float chance;
    }
}