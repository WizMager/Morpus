using System;
using Services.UiService;

namespace Configs.Player
{
    [Serializable]
    public struct LevelUpStep
    {
        public EStat stat;
        public float value;
    }
}