using System;
using Services.UiService;
using UnityEngine;

namespace Configs.Player
{
    [Serializable]
    public struct StatUpData
    {
        public EStat stat;
        [Range(0, 100f)]
        public float chance;
    }
}